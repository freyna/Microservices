using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Servicios.Api.Datos.Entities;

namespace Servicios.Api.Datos.Repository
{
    public class MongoRepository <TDocument> : IMongoRepository<TDocument> where TDocument : IDocument
    {
        private readonly IMongoCollection<TDocument> collection;

        public MongoRepository(IOptions<MongoSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            var db = client.GetDatabase(options.Value.Database);

            collection = db.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        }

        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(typeof(BsonCollectionAttribute), true).FirstOrDefault()).CollectionName;
        }

        public async Task<IEnumerable<TDocument>> GetAll()
        {
            return await collection.Find(x => true).ToListAsync();
        }

        public async Task<TDocument> GetById(string id)
        {
            var filter = Builders<TDocument>.Filter.Eq(x => x.Id, id);
            return await collection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task InsertDocument(TDocument document)
        {
            await collection.InsertOneAsync(document);
        }

        public async Task UpdateDocument(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(x => x.Id, document.Id);
            await collection.FindOneAndReplaceAsync(filter, document);
        }

        public async Task DeleteById(string id)
        {
            var filter = Builders<TDocument>.Filter.Eq(x => x.Id, id);
            await collection.FindOneAndDeleteAsync(filter);
        }

        public async Task<Pagination<TDocument>> PaginationBy(Expression<Func<TDocument, bool>> filterExpression, Pagination<TDocument> pagination)
        {
            var sort = Builders<TDocument>.Sort.Ascending(pagination.Sort);
            if (pagination.SortDirection == "desc")
            {
                sort = Builders<TDocument>.Sort.Descending(pagination.Sort);
            }

            if (string.IsNullOrEmpty(pagination.Filter))
            {
                pagination.Data = await collection.Find(x => true)
                    .Sort(sort)
                    .Skip( (pagination.Page - 1) * pagination.PageSize)
                    .Limit(pagination.PageSize)
                    .ToListAsync();
            }
            else
            {
                pagination.Data = await collection.Find(filterExpression)
                    .Sort(sort)
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Limit(pagination.PageSize)
                    .ToListAsync();
            }

            var totalDocuments = await collection.CountDocumentsAsync(FilterDefinition<TDocument>.Empty);
            pagination.PagesQuantity = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalDocuments/pagination.PageSize)));

            return pagination;
        }

        public async Task<Pagination<TDocument>> PaginationByFilter(Pagination<TDocument> pagination)
        {
            var sort = Builders<TDocument>.Sort.Ascending(pagination.Sort);
            if (pagination.SortDirection == "desc")
            {
                sort = Builders<TDocument>.Sort.Descending(pagination.Sort);
            }

            var totalDocuments = 0;
            if (pagination.FilterValue == null)
            {
                pagination.Data = await collection.Find(x => true)
                    .Sort(sort)
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Limit(pagination.PageSize)
                    .ToListAsync();
                totalDocuments = (await collection.Find(x => true).ToListAsync()).Count;
            }
            else
            {
                //Expression to find a value in a string. It is similar to like in sql
                var filterValue = ".*" + pagination.FilterValue.Value+ ".*";
                // When we have the expression, it is necessary to create a builder passing the property and the filter value.
                var filter = Builders<TDocument>.Filter.Regex(pagination.FilterValue.Property,new BsonRegularExpression(filterValue,"i"));

                pagination.Data = await collection.Find(filter)
                    .Sort(sort)
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Limit(pagination.PageSize)
                    .ToListAsync();

                totalDocuments = (await collection.Find(filter).ToListAsync()).Count;
            }

            //var totalDocuments = await collection.CountDocumentsAsync(FilterDefinition<TDocument>.Empty);
            pagination.PagesQuantity = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalDocuments / pagination.PageSize)));

            pagination.TotalRows = Convert.ToInt32(totalDocuments);

            return pagination;
        }
    }
}

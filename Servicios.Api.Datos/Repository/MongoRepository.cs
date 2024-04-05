using System.Linq.Expressions;
using Microsoft.Extensions.Options;
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
                sort = Builders<TDocument>.Sort.Ascending(pagination.Sort);
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
    }
}

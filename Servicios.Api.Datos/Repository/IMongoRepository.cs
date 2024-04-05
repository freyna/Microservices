using System.Linq.Expressions;
using Servicios.Api.Datos.Entities;

namespace Servicios.Api.Datos.Repository
{
    public interface IMongoRepository<TDocument> where TDocument : IDocument
    {
        Task<IEnumerable<TDocument>> GetAll();
        Task<TDocument> GetById(string id);
        Task InsertDocument(TDocument document);
        Task UpdateDocument(TDocument document);
        Task DeleteById(string id);

        Task<Pagination<TDocument>> PaginationBy(Expression<Func<TDocument, bool>> filterExpression,
            Pagination<TDocument> pagination);
    }
}

using MongoDB.Driver;
using Servicios.Api.Datos.Entities;

namespace Servicios.Api.Datos.ContextMongoDB
{
    public interface IAutorContext
    {
        IMongoCollection<Autor> Autores { get; }
    }
}

using Servicios.Api.Datos.Entities;

namespace Servicios.Api.Datos.Repository
{
    public interface IAutorRepository
    {
        Task<IEnumerable<Autor>> GetAutores();
    }
}

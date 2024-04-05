using Servicios.Api.BusinessRules.ViewModels;
using Servicios.Api.Datos.Entities;

namespace Servicios.Api.BusinessRules.Autores
{
    public interface IAutoresBusinessRules
    {
        Task<IEnumerable<AutorVM>> Autores();
        Task<IEnumerable<AutorVM>> AutoresGenerico();
        Task<AutorVM> AutorById(string id);
        Task InsertAutor(AutorEntity autor);
        Task UpdateAutor(AutorEntity autor);
        Task DeleteAutorById(string id);
    }
}

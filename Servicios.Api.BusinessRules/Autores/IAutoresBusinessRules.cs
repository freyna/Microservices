using Servicios.Api.BusinessRules.ViewModels;

namespace Servicios.Api.BusinessRules.Autores
{
    public interface IAutoresBusinessRules
    {
        Task<IEnumerable<AutorVM>> Autores();
    }
}

using Servicios.Api.BusinessRules.ViewModels;
using Servicios.Api.Datos.Repository;

namespace Servicios.Api.BusinessRules.Autores
{
    public class AutoresBusinessRules : IAutoresBusinessRules
    {
        private readonly IAutorRepository _autorRepository;
        public AutoresBusinessRules(IAutorRepository autorRepository)
        {
            _autorRepository = autorRepository;
        }

        public async Task<IEnumerable<AutorVM>> Autores()
        {
            var autores = await _autorRepository.GetAutores();

            return autores.Select(x => new AutorVM
            {
                Nombre = x.Nombre,
                Apellido = x.Apellido,
                GradoAcademico = x.GradoAcademico
            });
        }
    }
}

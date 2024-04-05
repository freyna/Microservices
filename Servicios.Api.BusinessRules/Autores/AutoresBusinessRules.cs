using Servicios.Api.BusinessRules.ViewModels;
using Servicios.Api.Datos.Entities;
using Servicios.Api.Datos.Repository;

namespace Servicios.Api.BusinessRules.Autores
{
    public class AutoresBusinessRules : IAutoresBusinessRules
    {
        private readonly IAutorRepository _autorRepository;
        private readonly IMongoRepository<AutorEntity> _repository;
        public AutoresBusinessRules(IAutorRepository autorRepository, IMongoRepository<AutorEntity> repository)
        {
            _autorRepository = autorRepository;
            _repository = repository;
        }

        public async Task<IEnumerable<AutorVM>> Autores()
        {
            var autores = await _autorRepository.GetAutores();

            return autores.Select(x => new AutorVM
            {
                Id = x.Id,
                Nombre = x.Nombre,
                Apellido = x.Apellido,
                GradoAcademico = x.GradoAcademico
            });
        }

        public async Task<IEnumerable<AutorVM>> AutoresGenerico()
        {
            var autores = await _repository.GetAll();

            return autores.Select(x => new AutorVM
            {
                Id = x.Id,
                Nombre = x.Nombre,
                Apellido = x.Apellido,
                GradoAcademico = x.GradoAcademico,
                CreatedDate = x.CreatedDate
            });
        }

        public async Task<AutorVM> AutorById(string id)
        {
            var autor = await _repository.GetById(id);
            return new AutorVM
            {
                Id = autor.Id,
                Nombre = autor.Nombre,
                Apellido = autor.Apellido,
                GradoAcademico = autor.GradoAcademico,
                CreatedDate = autor.CreatedDate
            };
        }

        public async Task InsertAutor(AutorEntity autor)
        {
            await _repository.InsertDocument(autor);
        }

        public async Task UpdateAutor(AutorEntity autor)
        {
            await _repository.UpdateDocument(autor);
        }

        public async Task DeleteAutorById(string id)
        {
            await _repository.DeleteById(id);
        }
    }
}

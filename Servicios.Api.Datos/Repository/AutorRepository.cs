using MongoDB.Driver;
using Servicios.Api.Datos.ContextMongoDB;
using Servicios.Api.Datos.Entities;

namespace Servicios.Api.Datos.Repository
{
    public class AutorRepository : IAutorRepository
    {
        private readonly IAutorContext _context;
        public AutorRepository(IAutorContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Autor>> GetAutores()
        {
            return await _context.Autores.Find(x => true).ToListAsync();
        }
    }
}

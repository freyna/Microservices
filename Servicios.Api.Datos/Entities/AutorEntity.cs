using MongoDB.Bson.Serialization.Attributes;

namespace Servicios.Api.Datos.Entities
{
    [BsonCollection("Autor")]
    public class AutorEntity : Document
    {
        [BsonElement("Nombre")]
        public string Nombre { get; set; }

        [BsonElement("Apellido")]
        public string Apellido { get; set; }

        [BsonElement("GradoAcademico")]
        public string GradoAcademico { get; set; }
    }
}

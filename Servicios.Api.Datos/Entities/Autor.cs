using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Servicios.Api.Datos.Entities
{
    public class Autor
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Nombre")]
        public string Nombre { get; set; }

        [BsonElement("Apellido")]
        public string Apellido { get; set; }

        [BsonElement("GradoAcademico")]
        public string GradoAcademico { get; set; }
        
    }
}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BasicDotNet6API.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
    }
}

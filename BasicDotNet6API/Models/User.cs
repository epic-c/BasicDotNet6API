using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BasicDotNet6API.Models
{
    public class User: UserResquest
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}

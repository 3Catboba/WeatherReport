using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LearningSystem.Models
{
    public class UserRecord
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("defaultCity")]
        public string DefaultCity { get; set; }

        [BsonElement("backgroundColor")]
        public string BackgroundColor { get; set; }
    }
}

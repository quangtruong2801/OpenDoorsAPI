using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OpenDoorsAPI.Models
{
    public class Team
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("teamName")]
        public string TeamName { get; set; }

        [BsonElement("members")]
        public int Members { get; set; }

        [BsonElement("createdDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedDate { get; set; }
    }
}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OpenDoorsAPI.Models
{
    public class Member
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("avatar")]
        public string Avatar { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("startDate")]
        public string StartDate { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("jobType")]
        public string JobType { get; set; }

        [BsonElement("team")]
        public string Team { get; set; }

        [BsonElement("teamId")]
        public string TeamId { get; set; }
    }
}

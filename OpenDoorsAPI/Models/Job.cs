using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OpenDoorsAPI.Models
{
    public class Job
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? JobId { get; set; }

        [BsonElement("jobName")]
        public string JobName { get; set; }

        [BsonElement("jobType")]
        public string JobType { get; set; }

        [BsonElement("skills")]
        public string Skills { get; set; }

        [BsonElement("requirement")]
        public string Requirement { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }
    }
}

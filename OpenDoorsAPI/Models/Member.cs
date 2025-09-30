using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace OpenDoorsAPI.Models
{
    public class Social
    {
        [BsonElement("platform")]
        public string Platform { get; set; }

        [BsonElement("url")]
        public string Url { get; set; }
    }

    public class Member
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("avatar")]
        public string Avatar { get; set; }

        [BsonElement("avatarPublicId")]
        public string AvatarPublicId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("birthday")]
        public string Birthday { get; set; }

        [BsonElement("hobbies")]
        public string Hobbies { get; set; }

        [BsonElement("socials")]
        public List<Social> Socials { get; set; } = new List<Social>(); 

        [BsonElement("startDate")]
        public string StartDate { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("jobType")]
        public List<string> JobType { get; set; } = new List<string>();

        [BsonElement("team")]
        public string Team { get; set; }

        [BsonElement("teamId")]
        public string TeamId { get; set; }
    }
}

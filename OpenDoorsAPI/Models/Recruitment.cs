using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace OpenDoorsAPI.Models
{
    public class Recruitment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }         

        [BsonElement("salary")]
        public string Salary { get; set; }      

        [BsonElement("location")]
        public string Location { get; set; }    

        [BsonElement("experience")]
        public int Experience { get; set; }        

        [BsonElement("deadline")]
        public DateTime Deadline { get; set; }     

        [BsonElement("description")]
        public string Description { get; set; }   
        [BsonElement("requirements")]
        public string Requirements { get; set; }   

        [BsonElement("benefits")]
        public string Benefits { get; set; }       

        [BsonElement("companyName")]
        public string CompanyName { get; set; }   
    }
}

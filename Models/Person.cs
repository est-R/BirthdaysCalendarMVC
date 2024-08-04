
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BirthdayCalendarMVC.Models
{
    public class Person
    {
        private string _name = "Имя не задано";

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? BsonId { get; set; }

        [BsonElement("Name")]
        public string Name
        {
            get { return _name; }
            set { _name = value ?? _name; }
        }

        [BsonElement("Date")]
        public DateTime Date { get; set; }

        public IFormFile? Image { get; set; }

    }
}

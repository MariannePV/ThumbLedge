using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace APIMongoDB.DAL.Model
{
    public class Intelligence
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String _id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        public Intelligence(String name)
        {
            this.Name = name;
        }

        public Intelligence() { }
    }
}

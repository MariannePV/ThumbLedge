using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThumbLedge.Entities
{
    public class Inteligencia
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String _id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        public Inteligencia(String name)
        {
            this.Name = name;
        }

        public Inteligencia() { }

        public override string ToString()
        {
            return String.Format("id: {0}\nname: {1}\n", this._id, this.Name);
        }
    }
}

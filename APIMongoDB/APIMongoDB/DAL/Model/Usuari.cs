using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace APIMongoDB.DAL.Model
{
    public class Usuari
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String _id { get; set; }

        [BsonElement("nom")]
        public String Nom { get; set; }

        [BsonElement("password")]
        public String Password { get; set; }

        public Usuari(string nom, string password)
        {
            this.Nom = nom;
            this.Password = password;
        }

        public Usuari() { }
    }
}

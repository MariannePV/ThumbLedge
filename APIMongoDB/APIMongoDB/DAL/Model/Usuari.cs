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

        [BsonElement("username")]
        public String Username { get; set; }

        [BsonElement("password")]
        public String Password { get; set; }

        [BsonElement("fullName")]
        public String FullName { get; set; }

        [BsonElement("email")]
        public String Email { get; set; }

        public Usuari(string username, string password, string fullName, string email)
        {
            this.Username = username;
            this.Password = password;
            this.FullName = fullName;
            this.Email = email;
        }

        public Usuari() { }
    }
}

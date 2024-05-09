using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThumbLedge.Entities
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

        public override string ToString()
        {
            return String.Format ("id: {0}\nusername: {1}\npassword: {2}\nfull name: {3}\nemail: {4}", this._id, this.Username, this.Password, this.FullName, this.Email);
        }
    }
}

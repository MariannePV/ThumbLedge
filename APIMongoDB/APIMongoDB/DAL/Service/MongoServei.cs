using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using APIMongoDB.DAL.Model;

namespace APIMongoDB.DAL.Service
{
    public class MongoServei
    {
        private MongoClient mongoClient;
        public IMongoCollection<Usuari> usuariCollection { get; set; }

        public MongoServei(string collection)
        {
            mongoClient = new MongoClient("mongodb+srv://mpulgar:Mp860511*@thumbledge.6u8qa0n.mongodb.net/?retryWrites=true&w=majority&appName=ThumbLedge");
            var database = mongoClient.GetDatabase("ThumbLedge");

            if (collection == "usuaris")
            {
                usuariCollection = database.GetCollection<Usuari>(collection);
            }
        }
    }
}

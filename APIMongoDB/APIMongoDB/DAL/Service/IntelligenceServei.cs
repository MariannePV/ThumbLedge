using APIMongoDB.DAL.Model;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace APIMongoDB.DAL.Service
{
    public class IntelligenceServei
    {
        //Obtenir totes les intel·ligències
        public static IEnumerable<Intelligence> GetAll()
        {
            MongoServei MS = new MongoServei("inteligencies");
            List<Intelligence> result = MS.intelligenceCollection.AsQueryable<Intelligence>().ToList();

            return result;
        }

        //Obtenir intel·ligència segons nom
        public Intelligence Get(string name)
        {
            MongoServei MS = new MongoServei("inteligencies");
            List<Intelligence> result = MS.intelligenceCollection.AsQueryable().Where(i => i.Name == name).ToList();

            try
            {
                return result[0];
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

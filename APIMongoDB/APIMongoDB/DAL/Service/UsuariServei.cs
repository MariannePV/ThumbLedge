using APIMongoDB.DAL.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace APIMongoDB.DAL.Service
{
    public class UsuariServei
    {
        public static IEnumerable<Usuari> GetAll()
        {
            MongoServei MS = new MongoServei("usuaris");
            List<Usuari> result = MS.usuariCollection.AsQueryable<Usuari>().ToList();

            return result;
        }

        //public int Add(Usuari usuari)
        //{
        //    MongoServei MS = new MongoServei("usuaris");
        //    if (MS.treballadorCollection.AsQueryable<Usuari>.Where(u => u.))
        //}

        //public Usuari GetUsuari(String nusr)
        //{

        //}
    }
}

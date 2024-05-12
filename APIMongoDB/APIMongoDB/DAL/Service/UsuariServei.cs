using APIMongoDB.DAL.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace APIMongoDB.DAL.Service
{
    public class UsuariServei
    {
        //Obtenir tots els usuaris
        public static IEnumerable<Usuari> GetAll()
        {
            MongoServei MS = new MongoServei("usuaris");
            List<Usuari> result = MS.usuariCollection.AsQueryable<Usuari>().ToList();

            return result;
        }

        //Obtenir usuari segons compte de correu
        public Usuari Get(string email)
        {
            MongoServei MS = new MongoServei("usuaris");
            List<Usuari> result = MS.usuariCollection.AsQueryable().Where(u => u.Email == email).ToList();

            try
            {
                return result[0];
            } catch (Exception)
            {
                return null;
            }
        }

        //Afegir usuari
        public int Add(Usuari usuari)
        {
            MongoServei MS = new MongoServei("usuaris");

            //Comprovem que no hi ha més usuaris amb aquest correu
            if (MS.usuariCollection.AsQueryable<Usuari>().Where(u => u.Email == usuari.Email).Count() == 0)
            {
                MS.usuariCollection.InsertOne(usuari);
                return 1;
            }

            return 0;
        }

        //Actualitzar usuari
        public int Update(Usuari usuari, string email)
        {
            MongoServei MS = new MongoServei("usuaris");

            if (MS.usuariCollection.AsQueryable<Usuari>().Where(u => u.Email == usuari.Email).ToList().Count == 0 ||
                usuari.Email == email)
            {
                var filter = Builders<Usuari>.Filter.Eq("email", email);
                var update = Builders<Usuari>.Update
                    .Set("username", usuari.Username)
                    .Set("password", usuari.Password)
                    .Set("fullName", usuari.FullName)
                    .Set("email", usuari.Email);

                MS.usuariCollection.UpdateOne(filter, update);

                return 1;
            }

            return 0;
        }

        //Eliminar usuari
        public int Delete(string email)
        {
            MongoServei MS = new MongoServei("usuaris");
            var result = MS.usuariCollection.DeleteOne(u => u.Email == email);

            try
            {
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}

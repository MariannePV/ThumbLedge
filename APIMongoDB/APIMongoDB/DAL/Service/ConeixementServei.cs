using APIMongoDB.DAL.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace APIMongoDB.DAL.Service
{
    public class ConeixementServei
    {
        //Obtenir tots els coneixements
        public static IEnumerable<Coneixement> GetAll()
        {
            MongoServei MS = new MongoServei("coneixements");
            List<Coneixement> result = MS.coneixementCollection.AsQueryable<Coneixement>().ToList();

            return result;
        }

        //Obtenir coneixement segons nom
        public Coneixement Get(string nom)
        {
            MongoServei MS = new MongoServei("coneixements");
            List<Coneixement> allConeixements = MS.coneixementCollection.AsQueryable().ToList();

            // Flatten the nested structure into a single list
            List<Coneixement> flattenedList = new List<Coneixement>();
            FlattenConeixements(allConeixements, flattenedList);

            // Find the Coneixement with the matching name
            return flattenedList.FirstOrDefault(c => c.NomConeixement == nom);
        }

        // Helper method to flatten the nested Coneixements
        private void FlattenConeixements(List<Coneixement> coneixements, List<Coneixement> flattenedList)
        {
            foreach (var coneixement in coneixements)
            {
                flattenedList.Add(coneixement);
                if (coneixement.Coneixements != null && coneixement.Coneixements.Any())
                {
                    FlattenConeixements(coneixement.Coneixements.ToList(), flattenedList);
                }
            }
        }

        //Afegir coneixement
        public int Add(Coneixement coneixement)
        {
            MongoServei MS = new MongoServei("coneixements");

            //Comprovem que no hi ha més usuaris amb aquest correu
            if (MS.coneixementCollection.AsQueryable<Coneixement>().Where(c => c.NomConeixement == coneixement.NomConeixement).Count() == 0)
            {
                MS.coneixementCollection.InsertOne(coneixement);
                return 1;
            }

            return 0;
        }

        //Actualitzar coneixement
        public int Update(Coneixement coneixement, string nom)
        {
            MongoServei MS = new MongoServei("coneixements");

            if (MS.coneixementCollection.AsQueryable<Coneixement>().Where(c => c.NomConeixement == coneixement.NomConeixement).ToList().Count == 0 ||
                coneixement.NomConeixement == nom)
            {
                var filter = Builders<Coneixement>.Filter.Eq("nomConeixement", nom);
                var update = Builders<Coneixement>.Update
                    .Set("descripcio", coneixement.Descripcio)
                    .Set("valors", coneixement.Valors);

                MS.coneixementCollection.UpdateOne(filter, update);

                return 1;
            }

            return 0;
        }

        //Actualitzem els valors i les seves dates
        public int UpdateValors(string nom, int[] valors, DateTime[] datesValors)
        {
            MongoServei MS = new MongoServei("coneixements");

            var filter = Builders<Coneixement>.Filter.Eq("nomConeixement", nom);
            var update = Builders<Coneixement>.Update
                .Set("valors", valors)
                .Set("datesValors", datesValors);

            var result = MS.coneixementCollection.UpdateOne(filter, update);

            //Depenent del resultat
            return result.IsAcknowledged ? 1 : 0;
        }

        //Actualitzem els coneixements
        public int UpdateConeixements(string nom, Coneixement[] newConeixements)
        {
            MongoServei MS = new MongoServei("coneixements");

            var filter = Builders<Coneixement>.Filter.Eq("nomConeixement", nom);
            var update = Builders<Coneixement>.Update
                .Set("coneixements", newConeixements);

            var result = MS.coneixementCollection.UpdateOne(filter, update);

            //Depenent del resultat
            return result.IsAcknowledged ? 1 : 0;
        }

        //Eliminar coneixement
        public int Delete(string nom)
        {
            MongoServei MS = new MongoServei("coneixements");
            var result = MS.coneixementCollection.DeleteOne(c => c.NomConeixement == nom);

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

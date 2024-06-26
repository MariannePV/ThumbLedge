﻿using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace APIMongoDB.DAL.Model
{
    public class Coneixement
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String _id { get; set; }

        [BsonElement("nomConeixement")]
        public string NomConeixement { get; set; }

        [BsonElement("dataCreacio")]
        public DateTime DataCreacio { get; set; }

        [BsonElement("descripcio")]
        public string Descripcio { get; set; }

        [BsonElement("valors")]
        public int[] Valors { get; set; }

        [BsonElement("datesValors")]
        public DateTime[] DatesValors { get; set; }

        [BsonElement("coneixements")]
        public Coneixement[] Coneixements { get; set; }

        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("intelligence")]
        public string Intelligence { get; set; }

        public Coneixement(string nomConeixement, DateTime dataCreacio, string descripcio, int[] valors, DateTime[] datesValors, Coneixement[] coneixements, string intelligence, string username)
        {
            this.NomConeixement = nomConeixement;
            this.DataCreacio = dataCreacio;
            this.Descripcio = descripcio;
            this.Valors = valors;
            this.DatesValors = datesValors;
            this.Coneixements = coneixements;
            this.Intelligence = intelligence;
            this.Username = username;
        }

        public Coneixement() { }
    }
}

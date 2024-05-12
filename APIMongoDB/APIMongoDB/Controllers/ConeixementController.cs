using APIMongoDB.DAL.Model;
using APIMongoDB.DAL.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace APIMongoDB.Controllers
{
    [Route("api/coneixement")]
    [ApiController]
    public class ConeixementController
    {
        [HttpGet]
        public List<Coneixement> Get()
        {
            return ConeixementServei.GetAll().ToList();
        }

        [HttpGet("{id}")]
        public Coneixement Get(string id)
        {
            ConeixementServei con = new ConeixementServei();
            return con.Get(id);
        }

        [HttpPost]
        public void Post([FromBody] Coneixement coneixement)
        {
            ConeixementServei con = new ConeixementServei();
            con.Add(coneixement);
        }

        [HttpPut("{id}")]
        public void Put([FromBody] Coneixement coneixement, string id)
        {
            ConeixementServei con = new ConeixementServei();
            con.Update(coneixement, id);
        }

        [HttpPatch("values/{id}")]
        public void PatchValues([FromBody] PatchValuesModel model, string id)
        {
            ConeixementServei con = new ConeixementServei();
            con.UpdateValors(id, model.Valors, model.DatesValors);
        }

        [HttpPatch("coneixements/{id}")]
        public void PatchConeixements([FromBody] Coneixement[] coneixements, string id)
        {
            ConeixementServei con = new ConeixementServei();
            con.UpdateConeixements(id, coneixements);
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            ConeixementServei con = new ConeixementServei();
            con.Delete(id);
        }
    }
}

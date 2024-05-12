using APIMongoDB.DAL.Model;
using APIMongoDB.DAL.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace APIMongoDB.Controllers
{
    [Route("api/inteligencia")]
    [ApiController]
    public class IntelligenceController
    {
        [HttpGet]
        public List<Intelligence> Get()
        {
            return IntelligenceServei.GetAll().ToList();
        }

        [HttpGet("{id}")]
        public Intelligence Get(string id)
        {
            IntelligenceServei intelligence = new IntelligenceServei();
            return intelligence.Get(id);
        }        
    }
}

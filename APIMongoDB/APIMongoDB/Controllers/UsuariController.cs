using APIMongoDB.DAL.Model;
using APIMongoDB.DAL.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace APIMongoDB.Controllers
{
    [Route("api/usuari")]
    [ApiController]
    public class UsuariController
    {
        [HttpGet]
        public List<Usuari> Get()
        {
            return UsuariServei.GetAll().ToList();
        }

        [HttpGet("{id}")]
        public Usuari Get(String id)
        {
            UsuariServei us = new UsuariServei();
            return us.Get(id);
        }

        [HttpPost]
        public void Post([FromBody] Usuari user)
        {
            UsuariServei us = new UsuariServei();
            us.Add(user);
        }

        [HttpPut("{id}")]
        public void Put([FromBody] Usuari user, string id)
        {
            UsuariServei us = new UsuariServei();
            us.Update(user, id);
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            UsuariServei us = new UsuariServei();
            us.Delete(id);
        }
    }
}

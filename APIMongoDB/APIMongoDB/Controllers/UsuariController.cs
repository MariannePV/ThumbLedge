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
        // GET: api/<UsuariController>
        [HttpGet]
        public List<Usuari> Get()
        {
            return UsuariServei.GetAll().ToList();
        }

        //// GET api/<UsuariController>/5
        //[HttpGet("{id}")]
        //public Usuari Get(String id)
        //{
        //    UsuariServei us = new UsuariServei();
        //    //return us.ge
        //}

        //// POST api/<UsuariController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<UsuariController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<UsuariController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}

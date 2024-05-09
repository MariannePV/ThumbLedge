using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ThumbLedge.Entities;

namespace ThumbLedge.API
{
    public class UsuariAPI
    {
        string BaseURI;

        public UsuariAPI()
        {
            BaseURI = ConfigurationManager.AppSettings["BaseUri"];
        }

        //GET tots els usuaris
        public async Task<List<Usuari>> GetUsuarisAsync()
        {
            List<Usuari> usuaris = new List<Usuari>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURI);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Enviem una petició GET a /usuari
                HttpResponseMessage response = await client.GetAsync("usuari");
                if (response.IsSuccessStatusCode)
                {
                    //Obtenim el resultat i el posem al llistat
                    usuaris = await response.Content.ReadAsAsync<List<Usuari>>();
                    response.Dispose();
                } else
                {
                    usuaris = null;
                }
            }
            return usuaris;
        }

        //GET segons Id (email)
        public async Task<Usuari> GetUsuariAsync(String Id)
        {
            Usuari usuari = new Usuari();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURI);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Enviem petició GET a /usuari/Id
                HttpResponseMessage response = await client.GetAsync($"usuari/{Id}");
                if (response.IsSuccessStatusCode)
                {
                    //Resposta 204 quan no s'han trobat dades
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        usuari = null;
                    } else
                    {
                        //Obtenim el resultat
                        usuari = await response.Content.ReadAsAsync<Usuari>();
                        response.Dispose();
                    }
                } else
                {
                    usuari = null;
                }
            }

            return usuari;
        }

        //POST (Afegir) un usuari
        public async Task AddAsync(Usuari usuari)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURI);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Enviem petició POST a /usuari
                HttpResponseMessage response = await client.PostAsJsonAsync("usuari", usuari);
                response.EnsureSuccessStatusCode();
            }
        }

        //PUT (Modificar) un usuari
        public async Task UpdateAsync(Usuari usuari, string Id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURI);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Enviem petició PUT a /usuari/Id
                HttpResponseMessage response = await client.PutAsJsonAsync($"usuari/{Id}", usuari);
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task DeleteAsync(string Id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURI);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Enviem petició DELETE a /usuari/Id
                HttpResponseMessage response = await client.DeleteAsync($"usuari/{Id}");
                response.EnsureSuccessStatusCode();
            }
        }
    }
}

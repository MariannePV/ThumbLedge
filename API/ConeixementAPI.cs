using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ThumbLedge.Entities;
using ThumbLedge.Model;
using Newtonsoft.Json;

namespace ThumbLedge.API
{
    public class ConeixementAPI
    {
        string BaseURI;

        public ConeixementAPI()
        {
            BaseURI = ConfigurationManager.AppSettings["BaseUri"];
        }

        //GET tots els coneixements
        public async Task<List<Coneixement>> GetConeixementsAsync()
        {
            List<Coneixement> coneixements = new List<Coneixement>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURI);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Enviem una petició GET a /coneixement
                HttpResponseMessage response = await client.GetAsync("coneixement");
                if (response.IsSuccessStatusCode)
                {
                    //Obtenim el resultat i el posem al llistat
                    coneixements = await response.Content.ReadAsAsync<List<Coneixement>>();
                    response.Dispose();
                }
                else
                {
                    coneixements = null;
                }
            }
            return coneixements;
        }

        //GET segons Id (nom)
        public async Task<Coneixement> GetConeixementAsync(String Id)
        {
            Coneixement coneixement = new Coneixement();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURI);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Enviem petició GET a /coneixement/Id
                HttpResponseMessage response = await client.GetAsync($"coneixement/{Id}");
                if (response.IsSuccessStatusCode)
                {
                    //Resposta 204 quan no s'han trobat dades
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        coneixement = null;
                    }
                    else
                    {
                        //Obtenim el resultat
                        coneixement = await response.Content.ReadAsAsync<Coneixement>();
                        response.Dispose();
                    }
                }
                else
                {
                    coneixement = null;
                }
            }

            return coneixement;
        }

        //POST (Afegir) un coneixement
        public async Task AddAsync(Coneixement coneixement)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURI);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Enviem petició POST a /coneixement
                HttpResponseMessage response = await client.PostAsJsonAsync("coneixement", coneixement);
                response.EnsureSuccessStatusCode();
            }
        }

        //PUT (Modificar) un coneixement
        public async Task UpdateAsync(Coneixement coneixement, string Id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURI);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Enviem petició PUT a /coneixement/Id
                HttpResponseMessage response = await client.PutAsJsonAsync($"coneixement/{Id}", coneixement);
                response.EnsureSuccessStatusCode();
            }
        }

        //PATCH modificar els valors i la seva data
        public async Task UpdateValorsAsync(string Id, int[] valors, DateTime[] dateTimes)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURI);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var patchModel = new PatchModel
                {
                    NomConeixement = Id,
                    Valors = valors,
                    DatesValors = dateTimes
                };

                //Enviem petició PATCH a /coneixement/Id
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"coneixement/values/{Id}");
                request.Content = new StringContent(JsonConvert.SerializeObject(patchModel), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
            }
        }

        //PATCH modificar els coneixements
        public async Task UpdateConeixementsAsync(string Id, Coneixement[] coneixements)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURI);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Enviem petició PATCH a /coneixement/Id
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"coneixement/coneixements/{Id}");
                request.Content = new StringContent(JsonConvert.SerializeObject(coneixements), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.SendAsync(request);
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

                //Enviem petició DELETE a /coneixement/Id
                HttpResponseMessage response = await client.DeleteAsync($"coneixement/{Id}");
                response.EnsureSuccessStatusCode();
            }
        }
    }
}

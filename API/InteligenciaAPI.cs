using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ThumbLedge.Entities;

namespace ThumbLedge.API
{
    public class InteligenciaAPI
    {
        string BaseURI;

        public InteligenciaAPI()
        {
            BaseURI = ConfigurationManager.AppSettings["BaseUri"];
        }

        //GET tots els coneixements
        public async Task<List<Inteligencia>> GetInteligenciesAsync()
        {
            List<Inteligencia> inteligencies = new List<Inteligencia>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURI);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Enviem una petició GET a /inteligencia
                HttpResponseMessage response = await client.GetAsync("inteligencia");
                if (response.IsSuccessStatusCode)
                {
                    //Obtenim el resultat i el posem al llistat
                    inteligencies = await response.Content.ReadAsAsync<List<Inteligencia>>();
                    response.Dispose();
                }
                else
                {
                    inteligencies = null;
                }
            }
            return inteligencies;
        }

        //GET segons Id (nom)
        public async Task<Inteligencia> GetInteligenciaAsync(String Id)
        {
            Inteligencia inteligencia = new Inteligencia();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURI);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Enviem petició GET a /coneixement/Id
                HttpResponseMessage response = await client.GetAsync($"inteligencia/{Id}");
                if (response.IsSuccessStatusCode)
                {
                    //Resposta 204 quan no s'han trobat dades
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        inteligencia = null;
                    }
                    else
                    {
                        //Obtenim el resultat
                        inteligencia = await response.Content.ReadAsAsync<Inteligencia>();
                        response.Dispose();
                    }
                }
                else
                {
                    inteligencia = null;
                }
            }

            return inteligencia;
        }

    }
}

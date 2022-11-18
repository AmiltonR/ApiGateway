using ApiGateway.DTO;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Ocelot.Middleware;
using Ocelot.Multiplexer;
using System.Net;
using System.Net.Http.Headers;


namespace ApiGateway.Agregators
{
    public class NoInscritosAggregator : IDefinedAggregator
    {
        public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            //TODO
            //Agregar Try-catch
            var participantes = await responses[0].Items.DownstreamResponse().Content.ReadFromJsonAsync<List<UsuariosIDDTO>>();
            var usuarios = await responses[1].Items.DownstreamResponse().Content.ReadFromJsonAsync<List<UsuariosDTO>>();


            //recorremos
            participantes?.ForEach(participante =>
            {
                UsuariosDTO user = usuarios.Where(u => u.Id == participante.IdUsuario).FirstOrDefault();
                usuarios.Remove(user);
                

            });


            var jsonString = JsonConvert.SerializeObject(usuarios, Formatting.Indented, new JsonConverter[] { new StringEnumConverter() });

            var stringContent = new StringContent(jsonString)
            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            };

            return new DownstreamResponse(stringContent, HttpStatusCode.OK, new List<KeyValuePair<string, IEnumerable<string>>>(), "OK");
        }
    }
}

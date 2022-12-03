using ApiGateway.DTO;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Ocelot.Middleware;
using Ocelot.Multiplexer;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using Ocelot.Responses;

namespace ApiGateway.Agregators
{
    public class PrestamoEncabezadoAggregator:IDefinedAggregator
    {
        public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            //TODO: REVISAR RESPUESTA DIFERENTE DE NULL
            try
            {
                var prestamo = await responses[0].Items.DownstreamResponse().Content.ReadFromJsonAsync<List<EncabezadoPrestamoDTO>>();
                var usuarios = await responses[1].Items.DownstreamResponse().Content.ReadFromJsonAsync<List<UsuariosDTO>>();

                if (prestamo.Count != 0)
                {
                    EncabezadoPrestamoRespuestaDTO response = new EncabezadoPrestamoRespuestaDTO();

                    prestamo.ForEach(p =>
                    {
                        p.Estudiante.AddRange(usuarios.Where(a => a.Id == p.IdUsuario));
                        p.Bibliotecario.AddRange(usuarios.Where(a => a.Id == p.IdUsuarioBibliotecario));
                        //break cuando se encuentren
                    });



                    var jsonString = JsonConvert.SerializeObject(prestamo, Formatting.Indented, new JsonConverter[] { new StringEnumConverter() });

                    var stringContent = new StringContent(jsonString)
                    {
                        Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
                    };
                    return new DownstreamResponse(stringContent, HttpStatusCode.OK, new List<KeyValuePair<string, IEnumerable<string>>>(), "OK");
                }

                string r = "No hay Datos";
                var jsonString1 = JsonConvert.SerializeObject(r, Formatting.Indented, new JsonConverter[] { new StringEnumConverter() });

                var stringContent1 = new StringContent(jsonString1);

                return new DownstreamResponse(stringContent1, HttpStatusCode.OK, new List<KeyValuePair<string, IEnumerable<string>>>(), "OK");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

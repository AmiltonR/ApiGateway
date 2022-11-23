using ApiGateway.DTO;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Ocelot.Middleware;
using System.Net;
using Ocelot.Multiplexer;
using System.Net.Http.Headers;

namespace ApiGateway.Agregators
{
    public class SolicitudesAggregator : IDefinedAggregator
    {
        public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            //TODO
            //Agregar Try-catch
            var solicitudes = await responses[0].Items.DownstreamResponse().Content.ReadFromJsonAsync<List<SolicitudDTO>>();
            var usuarios = await responses[1].Items.DownstreamResponse().Content.ReadFromJsonAsync<List<UsuariosDTO>>();
            var talleres = await responses[2].Items.DownstreamResponse().Content.ReadFromJsonAsync<List<TallerSolicitudDTO>>();

            SolicitudesRespuesta listado = new SolicitudesRespuesta();

            //recorremos
            solicitudes?.ForEach(solicitud =>
            {
                solicitud.usuario.AddRange(usuarios.Where(a => a.Id == solicitud.IdUsuario));
                solicitud.Talleres.AddRange(talleres.Where(t => t.Id == solicitud.IdTallerprogramacion));
            });

            listado.Respuesta.AddRange(solicitudes);

            var jsonString = JsonConvert.SerializeObject(listado, Formatting.Indented, new JsonConverter[] { new StringEnumConverter() });

            var stringContent = new StringContent(jsonString)
            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            };

            return new DownstreamResponse(stringContent, HttpStatusCode.OK, new List<KeyValuePair<string, IEnumerable<string>>>(), "OK");
        }
    }
}

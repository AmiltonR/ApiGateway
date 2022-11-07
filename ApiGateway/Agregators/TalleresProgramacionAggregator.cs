using ApiGateway.DTO;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Ocelot.Middleware;
using Ocelot.Multiplexer;
using System.Net;
using System.Net.Http.Headers;
using System.Collections;

namespace ApiGateway.Agregators
{
    public class TalleresProgramacionAggregator: IDefinedAggregator
    {
        public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            //TODO
            //Agregar Try-catch
            var talleres = await responses[0].Items.DownstreamResponse().Content.ReadFromJsonAsync<List<TallerProgramacionDTO>>();
            var usuarios = await responses[1].Items.DownstreamResponse().Content.ReadFromJsonAsync<List<UsuariosTProgramacionDTO>>();

            TallerProgramacionRespuestaDTO listado = new TallerProgramacionRespuestaDTO();

            talleres?.ForEach(taller =>
            {
                taller.usuario.AddRange(usuarios.Where(a => a.Id == taller.IdUsuarioInstructor));
              
            });

            listado.programacionRespuesta.AddRange(talleres);

            var jsonString = JsonConvert.SerializeObject(listado, Formatting.Indented, new JsonConverter[] { new StringEnumConverter() });

            var stringContent = new StringContent(jsonString)
            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            };

            return new DownstreamResponse(stringContent, HttpStatusCode.OK, new List<KeyValuePair<string, IEnumerable<string>>>(), "OK");
        }
    }
}

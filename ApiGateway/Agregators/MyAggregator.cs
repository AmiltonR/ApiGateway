using ApiGateway.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Ocelot.Middleware;
using Ocelot.Multiplexer;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;

namespace ApiGateway.Agregators
{
    public class MyAggregator: IDefinedAggregator
    {
        public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            var talleres = await responses[0].Items.DownstreamResponse().Content.ReadFromJsonAsync<List<TalleresDTO>>();
            var usuarios = await responses[1].Items.DownstreamResponse().Content.ReadFromJsonAsync<List<UsuariosDTO>>();

            TalleresParticipantesRespuestaDTO listado = new TalleresParticipantesRespuestaDTO();
            listado.IdTallerProgramacion = talleres[0].IdTallerProgramacion;

            talleres?.ForEach(taller =>
            {
               taller.usuarios.AddRange(usuarios.Where(a => a.Id == taller.IdUsuario));
                listado.usuarios.AddRange(taller.usuarios);
            });


            var jsonString = JsonConvert.SerializeObject(listado, Formatting.Indented, new JsonConverter[] { new StringEnumConverter() });

            var stringContent = new StringContent(jsonString)
            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            };

            return new DownstreamResponse(stringContent, HttpStatusCode.OK, new List<KeyValuePair<string, IEnumerable<string>>>(), "OK");
        }
    }
}

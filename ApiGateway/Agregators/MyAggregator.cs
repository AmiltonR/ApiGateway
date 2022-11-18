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
        //DEVEUELVE LOS PARTICIPANTES POR TALLER
        public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            //TODO: REVISAR RESPUESTA DIFERENTE DE NULL
            try
            {
                var talleres = await responses[0].Items.DownstreamResponse().Content.ReadFromJsonAsync<TalleresDTO>();
                var usuarios = await responses[1].Items.DownstreamResponse().Content.ReadFromJsonAsync<List<UsuariosDTO>>();

                TalleresParticipantesRespuestaDTO listado = new TalleresParticipantesRespuestaDTO();
                talleres.usuarios.ForEach(taller =>
                {
                    listado.usuarios.AddRange(usuarios.Where(a => a.Id == taller.IdUsuario));
                });

                listado.IdTallerProgramacion = talleres.IdTallerProgramacion;
                listado.NombreTaller = talleres.NombreTaller;
                listado.NumeroParticipantes = talleres.NumeroParticipantes;
                listado.IdUsuarioInstructor = talleres.IdUsuarioInstructor;
                listado.Cupos = talleres.Cupos;

                UsuariosDTO instructor = usuarios.Where(u => u.Id == talleres.IdUsuarioInstructor).FirstOrDefault();

                listado.NombreInstructor = instructor.NombreUsuario + " " + instructor.ApellidoUsuario;

                var jsonString = JsonConvert.SerializeObject(listado, Formatting.Indented, new JsonConverter[] { new StringEnumConverter() });

                var stringContent = new StringContent(jsonString)
                {
                    Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
                };

                return new DownstreamResponse(stringContent, HttpStatusCode.OK, new List<KeyValuePair<string, IEnumerable<string>>>(), "OK");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

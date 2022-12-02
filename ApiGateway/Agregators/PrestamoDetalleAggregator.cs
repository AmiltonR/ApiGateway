using ApiGateway.DTO;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Ocelot.Middleware;
using Ocelot.Multiplexer;
using System.Net;
using System.Net.Http.Headers;

namespace ApiGateway.Agregators
{
    public class PrestamoDetalleAggregator:IDefinedAggregator
    {
        public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            //TODO: REVISAR RESPUESTA DIFERENTE DE NULL
            try
            {
                var prestamo = await responses[0].Items.DownstreamResponse().Content.ReadFromJsonAsync<List<DetallePrestamoDTO>>();
                var libros = await responses[1].Items.DownstreamResponse().Content.ReadFromJsonAsync<List<DetallePrestamoLibrosDTO>>();

                prestamo.ForEach(p =>
                {
                    p.Libro.AddRange(libros.Where(l => l.Id == p.IdLibro));
                });

                var jsonString = JsonConvert.SerializeObject(prestamo, Formatting.Indented, new JsonConverter[] { new StringEnumConverter() });

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

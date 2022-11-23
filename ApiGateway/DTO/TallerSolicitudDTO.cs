namespace ApiGateway.DTO
{
    public class TallerSolicitudDTO
    {
        public int Id { get; set; }
        public int IdTaller { get; set; }
        public string NombreTaller { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFinal { get; set; }
    }
}

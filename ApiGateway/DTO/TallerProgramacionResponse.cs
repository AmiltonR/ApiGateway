namespace ApiGateway.DTO
{
    public class TallerProgramacionResponse
    {
        public int Id { get; set; }
        public int IdTaller { get; set; }
        public string NombreTaller { get; set; }
        public string NombreUsuario { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public int NumeroParticipantes { get; set; }
        public string Publico { get; set; }
        public double Costo { get; set; }
        public string NombrePatrocinador { get; set; }
        public int NumeroSesiones { get; set; }
    }
}

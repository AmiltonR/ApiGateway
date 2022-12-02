namespace ApiGateway.DTO
{
    public class DetallePrestamoLibrosDTO
    {
        public int Id { get; set; }
        public string NombreLibro { get; set; }
        public string Isbn { get; set; }
        public string Condicion { get; set; }
        public string Editorial { get; set; }
        public string Detalle { get; set; }
        public string FechaIngreso { get; set; }
    }
}

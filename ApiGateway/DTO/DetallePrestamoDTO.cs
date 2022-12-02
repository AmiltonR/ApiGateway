namespace ApiGateway.DTO
{
    public class DetallePrestamoDTO
    {
        public int Id { get; set; }
        public int IdLibro { get; set; }
        public List<DetallePrestamoLibrosDTO> Libro = new();
    }
}

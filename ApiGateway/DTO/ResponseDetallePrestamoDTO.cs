namespace ApiGateway.DTO
{
    public class ResponseDetallePrestamoDTO
    {
        public List<DetallePrestamoDTO> General = new();
        public List<DetallePrestamoLibrosDTO> Libro = new();
    }
}

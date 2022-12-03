namespace ApiGateway.DTO
{
    public class EncabezadoPrestamoRespuestaDTO
    {
        public List<EncabezadoPrestamoDTO> encabezado = new();
        public List<UsuariosDTO> Estudiante = new();
        public List<UsuariosDTO> Bibliotecario = new();
    }
}

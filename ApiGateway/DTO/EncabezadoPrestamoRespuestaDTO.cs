namespace ApiGateway.DTO
{
    public class EncabezadoPrestamoRespuestaDTO
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdUsuarioBibliotecario { get; set; }
        public string FechaHora { get; set; }
        public List<UsuariosDTO> Estudiante = new();
        public List<UsuariosDTO> Bibliotecario = new();
    }
}

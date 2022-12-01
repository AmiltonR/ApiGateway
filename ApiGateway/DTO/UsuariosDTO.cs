namespace ApiGateway.DTO
{
    public class UsuariosDTO
    {
        public int Id { get; set; }
        public string Carnet { get; set; } = string.Empty;
        public string NombreUsuario { get; set; }
        public string ApellidoUsuario { get; set; }
    }
}

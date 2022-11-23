namespace ApiGateway.DTO
{
    public class SolicitudDTO
    {
        public int Id { get; set; }
        public int IdTallerprogramacion { get; set; }
        public List<TallerSolicitudDTO> Talleres = new();
        public int IdUsuario { get; set; }
        public List<UsuariosDTO> usuario = new();
    }
}

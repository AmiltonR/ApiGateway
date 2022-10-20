namespace ApiGateway.DTO
{
    public class TalleresDTO
    {
        public int Id { get; set; }
        public int IdTallerProgramacion { get; set; }
        public int IdUsuario { get; set; }

        public List<UsuariosDTO> usuarios = new();
    }
}

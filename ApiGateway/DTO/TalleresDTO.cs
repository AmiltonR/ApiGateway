namespace ApiGateway.DTO
{
    public class TalleresDTO
    {
        public int IdTallerProgramacion { get; set; }
        public int IdUsuarioInstructor { get; set; }
        public string NombreTaller { get; set; }
        public int NumeroParticipantes { get; set; }

        public List<UsuariosIDDTO> usuarios { get; set; }
    }
}

namespace ApiGateway.DTO
{
    public class TalleresParticipantesRespuestaDTO
    {
        public int IdTallerProgramacion { get; set; }
        public int IdUsuarioInstructor { get; set; }
        public string  NombreInstructor { get; set; }
        public string NombreTaller { get; set; }
        public int NumeroParticipantes { get; set; }

        public List<UsuariosDTO> usuarios = new();
    }
}

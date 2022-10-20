namespace ApiGateway.DTO
{
    public class TalleresParticipantesRespuestaDTO
    {
        public int IdTallerProgramacion { get; set; }
        public List<UsuariosDTO> usuarios = new();
    }
}

namespace FarmaciaAPI.Domain.DTOs.Usuario
{
    public class UsuarioAtualizarDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
    }
}

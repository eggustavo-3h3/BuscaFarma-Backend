using FarmaciaAPI.Enumerators;

namespace FarmaciaAPI.DTOs.Usuario
{
    public class UsuarioAdicionar
    {
        public string Nome { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string ConfirmarSenha { get; set; } = string.Empty;
        public EnumTipoUsuario Tipo { get; set; }
    }
}

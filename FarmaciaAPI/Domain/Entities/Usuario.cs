using FarmaciaAPI.Domain.Enumerators;
using Microsoft.EntityFrameworkCore;

namespace FarmaciaAPI.Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public EnumTipoUsuario Tipo { get; set; }

    }
}

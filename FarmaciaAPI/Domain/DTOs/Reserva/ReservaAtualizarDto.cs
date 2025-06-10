using FarmaciaAPI.Domain.Enumerators;

namespace FarmaciaAPI.Domain.DTOs.Reserva
{
    public class ReservaAtualizarDto
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid MedicamentoId { get; set; }
        public DateTime DataReserva { get; set; }
        public string ImagemReceita { get; set; } = string.Empty;
        public EnumTipoAtendimento EnumTipoAtendimento { get; set; }
        public EnumStatusReserva Status { get; set; }
        public DateTime? DataRetirada { get; set; }
        public string RetiranteNome { get; set; }
        public string RetiranteCpf { get; set; }
    }
}

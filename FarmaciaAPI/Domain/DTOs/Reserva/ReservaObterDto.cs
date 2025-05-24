using FarmaciaAPI.Domain.Enumerators;

namespace FarmaciaAPI.Domain.DTOs.Reserva
{
    public class ReservaObterDto
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid MedicamentoId { get; set; }
        public DateTime DataReserva { get; set; }
        public string ImagemReceita { get; set; } = string.Empty;
        public EnumTipoAtendimento EnumTipoAtendimento { get; set; }
        public EnumStatusReserva Status { get; set; }
        public DateTime DataRetirada { get; set; }
        public Guid RetiranteNome { get; set; }
        public Guid RetiranteCpf { get; set; }
        public string Usuario { get; set; }
        public string Medicamento { get; set; }
    }
}

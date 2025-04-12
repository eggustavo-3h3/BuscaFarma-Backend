using FarmaciaAPI.Domain.Enumerators;

namespace FarmaciaAPI.Domain.DTOs.Reserva
{
    public class ReservaAtualizar
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid MedicamentoId { get; set; }
        public DateTime DataReserva { get; set; }
        public string ImagemReceita { get; set; } = string.Empty;
        public EnumTipoAtendimento EnumTipoAtendimento { get; set; }
        public EnumStatusReserva Status { get; set; }
    }
}

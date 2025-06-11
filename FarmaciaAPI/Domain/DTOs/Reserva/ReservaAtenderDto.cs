using FarmaciaAPI.Domain.Enumerators;

namespace FarmaciaAPI.Domain.DTOs.Reserva
{
    public class ReservaAtenderDto
    {
        public EnumTipoAtendimento EnumTipoAtendimento { get; set; }
        public EnumStatusReserva Status { get; set; }
        public string? RetiranteNome { get; set; }
        public string? RetiranteCpf { get; set; }
    }
}
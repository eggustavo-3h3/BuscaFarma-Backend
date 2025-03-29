using FarmaciaAPI.Enumerators;

namespace FarmaciaAPI.DTOs.Reserva
{
    public class ReservaAdicionar
    {
        public Guid UsuarioId { get; set; }
        public Guid MedicamentoId { get; set; }
        public DateTime DataReserva { get; set; }
        public string ImagemReceita { get; set; } = string.Empty;
        public EnumTipoAtendimento EnumTipoAtendimento { get; set; }
    }
}

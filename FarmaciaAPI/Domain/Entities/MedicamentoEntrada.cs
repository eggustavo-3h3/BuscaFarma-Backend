namespace FarmaciaAPI.Domain.Entities
{
    public class MedicamentoEntrada
    {
        public Guid Id { get; set; }
        public Guid MedicamentoId { get; set; }
        public DateTime Data { get; set; }
        public int Quantidade { get; set; }

        public Medicamento Medicamento { get; set; }
    }
}

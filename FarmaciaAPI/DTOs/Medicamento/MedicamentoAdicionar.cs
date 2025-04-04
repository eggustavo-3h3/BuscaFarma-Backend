using FarmaciaAPI.Domain;
using FarmaciaAPI.Enumerators;

namespace FarmaciaAPI.DTOs.Medicamento
{
    public class MedicamentoAdicionar
    {
        public string NomeComercial { get; set; } = string.Empty;
        public string NomeQuimico { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Imagem { get; set; }
        public EnumTipoMedicamento TipoMedicamento { get; set; }
        public EnumUnidadeMedida UnidadeMedida { get; set; }
        public Guid CategoriaId { get; set; }
    }
}

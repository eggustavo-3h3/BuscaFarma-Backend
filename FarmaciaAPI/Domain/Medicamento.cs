using FarmaciaAPI.Enumerators;

namespace FarmaciaAPI.Domain
{
    public class Medicamento
    {
        public Guid Id { get; set; }
        public string NomeComercial { get; set; } = string.Empty;
        public string NomeQuimico { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public EnumTipoMedicamento TipoMedicamento { get; set; }
        public EnumUnidadeMedida UnidadeMedida { get; set; }
        public Guid CategoriaId { get; set; }

        public Categoria Categoria { get; set; }
    }
}

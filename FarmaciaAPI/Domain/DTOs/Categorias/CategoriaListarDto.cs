namespace FarmaciaAPI.Domain.DTOs.Categorias
{
    public class CategoriaListarDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
}
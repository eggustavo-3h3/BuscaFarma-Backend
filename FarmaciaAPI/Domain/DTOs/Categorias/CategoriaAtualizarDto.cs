﻿namespace FarmaciaAPI.Domain.DTOs.Categorias
{
    public class CategoriaAtualizarDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
}

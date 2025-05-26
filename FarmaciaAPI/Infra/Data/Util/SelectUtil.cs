using FarmaciaAPI.Domain.DTOs.Base;
using FarmaciaAPI.Domain.DTOs.Medicamento;
using FarmaciaAPI.Domain.DTOs.Usuario;
using FarmaciaAPI.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FarmaciaAPI.Infra.Data.Util
{
    public static class SelectUtil
    {

        public static MedicamentoObterDto? ObterMedicamento(FarmaciaContext context, Guid id)
        {
            var medicamento = context.MedicamentoSet.Include(M => M.Categoria).FirstOrDefault(m => m.Id == id);

            if (medicamento == null)
                return null;

            return new MedicamentoObterDto
            {
                Id = medicamento.Id,
                Descricao = medicamento.Descricao,
                NomeComercial = medicamento.NomeComercial,
                NomeQuimico = medicamento.NomeQuimico,
                TipoMedicamento = medicamento.TipoMedicamento,
                Quantidade = medicamento.Quantidade,
                UnidadeMedida = medicamento.UnidadeMedida,
                Imagem = medicamento.Imagem,
                CategoriaId = medicamento.CategoriaId,
                CategoriaDescricao = medicamento.Categoria.Descricao
            };
        }

        public static UsuarioObterDto? ObterUsuario(FarmaciaContext context, Guid id)
        {
            var usuario = context.UsuarioSet.FirstOrDefault(m => m.Id == id);

            if (usuario == null)
            {
                return null;
            }

            return new UsuarioObterDto
            {
                CPF = usuario.CPF,
                Id = usuario.Id,
                Nome = usuario.Nome,
                Telefone = usuario.Telefone
            };
        }

    }
}

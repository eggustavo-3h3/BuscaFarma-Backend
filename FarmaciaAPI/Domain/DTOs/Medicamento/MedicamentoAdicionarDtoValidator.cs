using FluentValidation;
using FarmaciaAPI.Domain.DTOs.Medicamento;

namespace FarmaciaAPI.Domain.Validators.Medicamento
{
    public class MedicamentoAdicionarDtoValidator : AbstractValidator<MedicamentoAdicionarDto>
    {
        public MedicamentoAdicionarDtoValidator()
        {
            RuleFor(m => m.NomeComercial)
                .NotEmpty().WithMessage("Nome comercial é obrigatório.")
                .MaximumLength(100).WithMessage("Nome comercial deve ter no máximo 150 caracteres.");

            RuleFor(m => m.NomeQuimico)
                .NotEmpty().WithMessage("Nome químico é obrigatório.")
                .MaximumLength(100).WithMessage("Nome químico deve ter no máximo 150 caracteres.");

            RuleFor(m => m.Descricao)
                .NotEmpty().WithMessage("Descrição é obrigatória.")
                .MaximumLength(500).WithMessage("Descrição deve ter no máximo 500 caracteres.");

            RuleFor(m => m.Imagem)
                .NotEmpty().WithMessage("Imagem é obrigatória.");

            RuleFor(m => m.TipoMedicamento)
                .IsInEnum().WithMessage("Tipo de medicamento inválido.");

            RuleFor(m => m.Quantidade)
                .NotEmpty().WithMessage("Quantidade é obrigatória.")
                .Matches(@"^\d+(\.\d+)?$").WithMessage("Quantidade deve ser um número válido.");

            RuleFor(m => m.UnidadeMedida)
                .IsInEnum().WithMessage("Unidade de medida inválida.");

            RuleFor(m => m.CategoriaId)
                .NotEmpty().WithMessage("Categoria é obrigatória.");
        }
    }
}

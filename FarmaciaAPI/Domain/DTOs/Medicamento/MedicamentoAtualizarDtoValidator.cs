using FluentValidation;
using FarmaciaAPI.Domain.DTOs.Medicamento;

namespace FarmaciaAPI.Domain.Validators.Medicamento
{
    public class MedicamentoAtualizarDtoValidator : AbstractValidator<MedicamentoAtualizarDto>
    {
        public MedicamentoAtualizarDtoValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("ID é obrigatório.");

            RuleFor(m => m.NomeComercial)
                .NotEmpty().WithMessage("Nome comercial é obrigatório.")
                .MaximumLength(150).WithMessage("Nome comercial deve ter no máximo 150 caracteres.");

            RuleFor(m => m.NomeQuimico)
                .NotEmpty().WithMessage("Nome químico é obrigatório.")
                .MaximumLength(150).WithMessage("Nome químico deve ter no máximo 150 caracteres.");

            RuleFor(m => m.Descricao)
                .NotEmpty().WithMessage("Descrição é obrigatória.")
                .MaximumLength(500).WithMessage("Descrição deve ter no máximo 500 caracteres.");

            RuleFor(m => m.Imagem)
                .NotEmpty().WithMessage("Imagem é obrigatória.");

            RuleFor(m => m.TipoMedicamento)
                .IsInEnum().WithMessage("Tipo de medicamento inválido.");

            RuleFor(m => m.Quantidade)
                .NotEmpty().WithMessage("Quantidade é obrigatória.");

            RuleFor(m => m.UnidadeMedida)
                .IsInEnum().WithMessage("Unidade de medida inválida.");

            RuleFor(m => m.CategoriaId)
                .NotEmpty().WithMessage("Categoria é obrigatória.");
        }
    }
}

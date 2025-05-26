using FluentValidation;

namespace FarmaciaAPI.Domain.DTOs.Categorias
{
    public class CategoriaAtualizarDtoValidator : AbstractValidator<CategoriaAtualizarDto>
    {
        public CategoriaAtualizarDtoValidator()
        {
            RuleFor(c => c.Id)
               .NotEmpty().WithMessage("ID é obrigatório.");

            RuleFor(c => c.Descricao)
                .NotEmpty().WithMessage("Descrição deve ser de prenchimento obrigatório.")
                .MaximumLength(100).WithMessage("Descrição deve ter no máximo 100 caracteres");
        }
    }
}

using FluentValidation;

namespace FarmaciaAPI.Domain.DTOs.Categorias
{
    public class CategoriaAdicionarDtoValidator : AbstractValidator<CategoriaAdicionarDto>
    {
        public CategoriaAdicionarDtoValidator()
        {
            RuleFor(p => p.Descricao)
                .NotEmpty().WithMessage("Descrição deve ser de prenchimento obrigatório.")
                .MaximumLength(100).WithMessage("Descrição deve ter no máximo 100 caracteres");
        }
    }
}

using FluentValidation;
using FarmaciaAPI.Domain.DTOs.Usuario;

namespace FarmaciaAPI.Domain.Validators.Usuario
{
    public class UsuarioAtualizarDtoValidator : AbstractValidator<UsuarioAtualizarDto>
    {
        public UsuarioAtualizarDtoValidator()
        {
            RuleFor(u => u.Id)
                .NotEmpty().WithMessage("ID do usuário é obrigatório.");

            RuleFor(u => u.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .MaximumLength(150).WithMessage("Nome deve ter no máximo 150 caracteres.");

            RuleFor(u => u.CPF)
                .NotEmpty().WithMessage("CPF é obrigatório.")
                .Length(11).WithMessage("CPF deve conter 11 dígitos.")
                .Matches(@"^\d{11}$").WithMessage("CPF deve conter apenas números.");

            RuleFor(u => u.Telefone)
                .NotEmpty().WithMessage("Telefone é obrigatório.")
                .Matches(@"^\d{10,11}$").WithMessage("Telefone deve conter entre 10 e 11 dígitos numéricos.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email é obrigatório.")
                .EmailAddress().WithMessage("Formato do email inválido.");

            RuleFor(u => u.Tipo)
                .IsInEnum().WithMessage("Tipo de usuário inválido.");
        }
    }
}

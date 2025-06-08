using FarmaciaAPI.Domain.DTOs.ResetSenha;
using FluentValidation;

public class ResetSenhaDtoValidator : AbstractValidator<ResetSenhaDto>
{
    public ResetSenhaDtoValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("E-mail inválido.");

        RuleFor(r => r.Codigo)
            .NotEmpty().WithMessage("O código é obrigatório.")
            .Length(6).WithMessage("O código deve ter 6 dígitos.")
            .Matches("^[0-9]{6}$").WithMessage("O código deve conter apenas números.");

        RuleFor(r => r.NovaSenha)
            .NotEmpty().WithMessage("A nova senha é obrigatória.")
            .MinimumLength(6).WithMessage("A nova senha deve ter pelo menos 6 caracteres.");

        RuleFor(r => r.ConfirmarNovaSenha)
            .Equal(r => r.NovaSenha).WithMessage("A confirmação da senha não confere.");
    }
}

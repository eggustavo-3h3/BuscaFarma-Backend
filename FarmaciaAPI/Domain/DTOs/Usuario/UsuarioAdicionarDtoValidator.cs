﻿using FluentValidation;
using FarmaciaAPI.Domain.DTOs.Usuario;

namespace FarmaciaAPI.Domain.Validators.Usuario
{
    public class UsuarioAdicionarDtoValidator : AbstractValidator<UsuarioAdicionarDto>
    {
        public UsuarioAdicionarDtoValidator()
        {
            RuleFor(u => u.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .MaximumLength(100).WithMessage("Nome deve ter no máximo 150 caracteres.");

            RuleFor(u => u.CPF)
                .NotEmpty().WithMessage("CPF é obrigatório.")
                .Length(11).WithMessage("CPF deve conter 11 dígitos.")
                .Matches(@"^\d{11}$").WithMessage("CPF deve conter apenas números.");

            RuleFor(u => u.Telefone)
                .NotEmpty().WithMessage("Telefone é obrigatório.")
                .Matches(@"^\d{10,11}$").WithMessage("Telefone deve conter entre 10 e 11 dígitos numéricos.");

            RuleFor(u => u.Senha)
                .NotEmpty().WithMessage("Senha é obrigatória.")
                .MinimumLength(8).WithMessage("A senha deve ter no mínimo 8 caracteres.")
                .Matches("[A-Z]").WithMessage("A senha deve conter pelo menos uma letra maiúscula.")
                .Matches("[a-z]").WithMessage("A senha deve conter pelo menos uma letra minúscula.")
                .Matches("[0-9]").WithMessage("A senha deve conter pelo menos um número.");

            RuleFor(u => u.ConfirmarSenha)
                .Equal(u => u.Senha).WithMessage("As senhas não coincidem.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email é obrigatório.")
                .EmailAddress().WithMessage("Formato do email inválido.");

            RuleFor(u => u.Tipo)
                .IsInEnum().WithMessage("Tipo de usuário inválido.");
        }
    }
}

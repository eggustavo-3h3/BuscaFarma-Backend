using FluentValidation;
using FarmaciaAPI.Domain.DTOs.Reserva;

namespace FarmaciaAPI.Domain.Validators.Reserva
{
    public class ReservaAdicionarDtoValidator : AbstractValidator<ReservaAdicionarDto>
    {
        public ReservaAdicionarDtoValidator()
        {
            RuleFor(r => r.UsuarioId)
                .NotEmpty().WithMessage("Usuário é obrigatório.");

            RuleFor(r => r.MedicamentoId)
                .NotEmpty().WithMessage("Medicamento é obrigatório.");

            RuleFor(r => r.DataReserva)
                .NotEmpty().WithMessage("Data da reserva é obrigatória.")
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("A data da reserva não pode ser no passado.");

            RuleFor(r => r.ImagemReceita)
                .NotEmpty().WithMessage("Imagem da receita é obrigatória.");

            RuleFor(r => r.EnumTipoAtendimento)
                .IsInEnum().WithMessage("Tipo de atendimento inválido.");

            RuleFor(r => r.Status)
                .IsInEnum().WithMessage("Status da reserva inválido.");
        }
    }
}

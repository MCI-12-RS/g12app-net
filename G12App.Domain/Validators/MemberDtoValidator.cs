using FluentValidation;
using G12App.Domain.DTOs;

namespace G12App.Domain.Validators
{
    public class MemberDtoValidator : AbstractValidator<MemberDto>
    {
        public MemberDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(2)
                .WithMessage("Insira um nome válido, mínimo 2 carácteres");

            RuleFor(x => x.MemberType)
                .IsInEnum()
                .WithMessage("Campo obrigatório");

            RuleFor(x => x.Email)
                .EmailAddress().Unless(x => string.IsNullOrEmpty(x.Email))
                .WithMessage("E-mail inválido");

            RuleFor(x => x.BirthDate)
                .NotEmpty()
                .WithMessage("Campo obrigatório");

            RuleFor(x => x.Gender)
               .IsInEnum()
               .WithMessage("Campo obrigatório");
        }
    }
} 

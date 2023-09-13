using FluentValidation;
using G12App.Domain.DTOs;

namespace G12App.Domain.Validators
{
    public class MemberDtoValidator : AbstractValidator<MemberDto>
    {
        public const string Name_ErrorMessage = "Insira um nome válido, mínimo 2 caracteres";
        public const string Surname_ErrorMessage = "Insira um nome válido, mínimo 2 caracteres";
        public const string MemberType_ErrorMessage = "Campo obrigatório";
        public const string Email_ErrorMessage = "E-mail inválido";
        public const string BirthDate_ErrorMessage = "Campo obrigatório";
        public const string Gender_ErrorMessage = "Campo obrigatório";

        public MemberDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(2)
                .WithMessage(Name_ErrorMessage);

            RuleFor(x => x.Surname)
                .NotEmpty()
                .MinimumLength(2)
                .WithMessage(Surname_ErrorMessage);

            RuleFor(x => x.MemberType)
                .IsInEnum()
                .WithMessage(MemberType_ErrorMessage);

            RuleFor(x => x.Email)
                .EmailAddress().Unless(x => string.IsNullOrEmpty(x.Email))
                .WithMessage(Email_ErrorMessage);

            RuleFor(x => x.BirthDate)
                .NotNull()
                .WithMessage(BirthDate_ErrorMessage);

            RuleFor(x => x.Gender)
               .IsInEnum()
               .WithMessage(Gender_ErrorMessage);
        }
    }
} 

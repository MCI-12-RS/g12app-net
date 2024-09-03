using FluentValidation;
using G12App.Domain.DTOs;
using G12App.Domain.Interfaces.Services;
using G12App.Domain.Querying;
using G12App.Domain.Querying.Filters;
using System;
using System.Linq;

namespace G12App.Domain.Validators
{
    public class MemberDtoValidator : AbstractValidator<MemberDto>
    {
        public const string Name_ErrorMessage = "Insira um nome válido, mínimo 2 caracteres";
        public const string Email_ErrorMessage = "E-mail inválido";
        public const string BirthDate_ErrorMessage = "Campo obrigatório";
        public const string Gender_ErrorMessage = "Campo obrigatório";

        public MemberDtoValidator(IMemberService memberService)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(2)
                .WithMessage(Name_ErrorMessage);

            RuleFor(x => x.Email)
                .EmailAddress().Unless(x => string.IsNullOrEmpty(x.Email))
                .WithMessage(Email_ErrorMessage);

            RuleFor(x => x.BirthDate)
                .GreaterThan(
                    DateTime.Now.Subtract(TimeSpan.FromDays(365 * 100)))
                .WithMessage(BirthDate_ErrorMessage);

            RuleFor(x => x.Gender)
               .IsInEnum()
               .WithMessage(Gender_ErrorMessage);

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    if (string.IsNullOrWhiteSpace(value))
                        return;

                    var members = memberService.GetMembers(new Query<MemberFilter>()
                    {
                        Filter = new MemberFilter()
                        {
                            UserName = value,
                            NotInId = [context.InstanceToValidate.Id.GetValueOrDefault()]
                        }
                    }).ConfigureAwait(false).GetAwaiter().GetResult();

                    if (!(members.Data?.Any()).GetValueOrDefault())
                        return;

                    context.AddFailure("Este e-mail já está sendo usado por outro membro.");
                });

            RuleFor(x => x.MobileNumber)
                .Custom((value, context) =>
                {
                    if (string.IsNullOrWhiteSpace(value))
                        return;

                    var members = memberService.GetMembers(new Query<MemberFilter>()
                    {
                        Filter = new MemberFilter()
                        {
                            UserName = value,
                            NotInId = [context.InstanceToValidate.Id.GetValueOrDefault()]
                        }
                    }).ConfigureAwait(false).GetAwaiter().GetResult();

                    if (!(members.Data?.Any()).GetValueOrDefault())
                        return;

                    context.AddFailure("Este número de celular já está sendo usado por outro membro.");
                });

        }
    }
}

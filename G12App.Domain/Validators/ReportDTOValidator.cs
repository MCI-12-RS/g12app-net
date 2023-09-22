using FluentValidation;
using G12App.Domain.DTOs;

namespace G12App.Domain.Validators
{
    public class ReportDTOValidator : AbstractValidator<ReportDTO>
    {
        public const string Subject_ErrorMessage = "Insira um tema válido";
        public const string Date_ErrorMessage = "Insira uma data válida";

        public ReportDTOValidator()
        {
            RuleFor(x => x.Subject)
                .NotEmpty()
                .WithMessage(Subject_ErrorMessage);

            RuleFor(x => x.Date)
                .NotEmpty()
                .WithMessage(Date_ErrorMessage);
        }
    }

}

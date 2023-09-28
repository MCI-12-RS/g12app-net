using FluentValidation;
using G12App.Domain.DTOs;
using System;

namespace G12App.Domain.Validators
{
    public class ReportDTOValidator : AbstractValidator<ReportDTO>
    {
        public const string Subject_ErrorMessage = "Insira um tema válido";
        public const string Date_ErrorMessage = "Insira uma data válida";
        public const string Frequenters_ErrorMessage = "Campo obrigatório";
        public const string Id_ErrorMessage = "Insira um ID válido";

        public ReportDTOValidator()
        {
            RuleFor(x => x.Subject)
                .NotEmpty()
                .WithMessage(Subject_ErrorMessage);

            RuleFor(x => x.Date)
                .NotEmpty()
                .GreaterThan(DateTime.MinValue)
                .WithMessage(Date_ErrorMessage);

            RuleFor(x => x.CellId)
                .GreaterThan(0)
                .WithMessage(Id_ErrorMessage);

            RuleFor(x => x.LeadershipId)
                .GreaterThan(0)
                .WithMessage(Id_ErrorMessage);

            RuleFor(x => x.HostId)
                .GreaterThan(0)
                .WithMessage(Id_ErrorMessage);

        }
    }

}

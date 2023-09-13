using FluentValidation;
using G12App.Domain.DTOs;

namespace G12App.Domain.Validators
{
    public class CellDtoValidator : AbstractValidator<CellDTO>
    {
        public const string LeadID_ErrorMessage = "Insira um ID válido";
        public const string HostID_ErrorMessage = "Insira um ID válido";
        public const string TimothyID_ErrorMessage = "Insira um ID válido";
        public const string WeekDay_ErrorMessage = "Campo obrigatório";
        public const string PlaceID_ErrorMessage = "Insira um ID válido";

        public CellDtoValidator()
        {
            RuleFor(x => x.LeadId)
                .GreaterThan(0)
                .WithMessage(LeadID_ErrorMessage);

            RuleFor(x => x.HostId)
                .GreaterThan(0)
                .WithMessage(HostID_ErrorMessage);
                
            RuleFor(x => x.TimothyId)
                .GreaterThan(0)
                .WithMessage(TimothyID_ErrorMessage); 
            
            RuleFor(x => x.WeekDay)
                .NotNull()
                .WithMessage(WeekDay_ErrorMessage);
            
            RuleFor(x => x.PlaceId)
                .GreaterThan(0)
                .WithMessage(PlaceID_ErrorMessage);
        }
    }
}

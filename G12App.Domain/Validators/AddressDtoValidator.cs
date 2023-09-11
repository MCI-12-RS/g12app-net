using FluentValidation;
using G12App.Domain.DTOs;

namespace G12App.Domain.Validators
{
    public class AddressDtoValidator : AbstractValidator<AddressDto>
    {
        public const string Street_ErrorMessage = "Campo obrigatório";
        public const string Number_ErrorMessage = "Campo obrigatório";
        public const string Neighborhood_ErrorMessage = "Campo obrigatório";
        public const string City_ErrorMessage = "Campo obrigatório";
        public const string State_ErrorMessage = "Campo obrigatório";
        public const string PostalCode_ErrorMessage = "Campo obrigatório";

        public AddressDtoValidator()
        {
            RuleFor(x => x.Street)
                .NotEmpty()
                .WithMessage(Street_ErrorMessage);

            RuleFor(x => x.Number)
                .NotEmpty()
                .WithMessage(Number_ErrorMessage);

            RuleFor(x => x.Neighborhood)
                .NotEmpty()
                .WithMessage(Neighborhood_ErrorMessage);
            
            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage(City_ErrorMessage);

            RuleFor(x => x.State)
                .NotEmpty()
                .WithMessage(State_ErrorMessage);

            RuleFor(x => x.PostalCode)
                .NotEmpty()
                .WithMessage(PostalCode_ErrorMessage);
        }
    }
}

using FluentValidation;
using AuthenticationService.Application.Commands.CreateCompany;

namespace AuthenticationService.Application.Validators
{
    public class CreateCompanyValidator : AbstractValidator<CreateCompanyCommand>
    {
        public CreateCompanyValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Company name is required.")
                .MaximumLength(100).WithMessage("Company name cannot exceed 100 characters.");

            RuleFor(x => x.OwnerEmail)
                .NotEmpty().WithMessage("Owner email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.OwnerPassword)
                .NotEmpty().WithMessage("Owner password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");

            RuleFor(x => x.OwnerFirstName)
                .NotEmpty().WithMessage("Owner first name is required.")
                .MaximumLength(50).WithMessage("Owner first name cannot exceed 50 characters.");

            RuleFor(x => x.OwnerLastName)
                .NotEmpty().WithMessage("Owner last name is required.")
                .MaximumLength(50).WithMessage("Owner last name cannot exceed 50 characters.");

            RuleFor(x => x.Address)
                .NotNull().WithMessage("Address is required.");

            RuleFor(x => x.Address.Street)
                .NotEmpty().WithMessage("Street is required.");

            RuleFor(x => x.Address.City)
                .NotEmpty().WithMessage("City is required.");

            RuleFor(x => x.Address.State)
                .NotEmpty().WithMessage("State is required.");

            RuleFor(x => x.Address.Country)
                .NotEmpty().WithMessage("Country is required.");

            RuleFor(x => x.Address.PostalCode)
                .NotEmpty().WithMessage("Postal code is required.");
        }
    }
}

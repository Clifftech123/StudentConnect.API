using FluentValidation;
using StudentConnect.API.Models.DTO;

namespace StudentConnect.API.Validations
{
    public class RegistrationModelValidator : AbstractValidator<RegistrationModel>
    {
        public RegistrationModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(1, 50).WithMessage("Name must be between 1 and 50 characters.");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .Length(1, 50).WithMessage("Username must be between 1 and 50 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email is not valid.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Length(6, 100).WithMessage("Password must be between 6 and 100 characters.");

            RuleFor(x => x.School)
                .NotEmpty().WithMessage("School is required.")
                .Length(1, 100).WithMessage("School must be between 1 and 100 characters.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.")
                .Length(1, 50).WithMessage("Role must be between 1 and 50 characters.");
        }

    }
    }

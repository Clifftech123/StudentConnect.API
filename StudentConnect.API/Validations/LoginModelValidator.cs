using FluentValidation;
using StudentConnect.API.Models.DTO;

namespace StudentConnect.API.Validations
{
    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {

            RuleFor(x => x.Username)
             .NotEmpty().WithMessage("Username is required.")
             .Length(1, 50).WithMessage("Username must be between 1 and 50 characters.");

            RuleFor(x => x.Password)
           .NotEmpty().WithMessage("Password is required.")
           .Length(6, 100).WithMessage("Password must be between 6 and 100 characters.");

        }
    }
}

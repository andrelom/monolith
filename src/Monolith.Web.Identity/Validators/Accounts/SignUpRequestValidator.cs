using FluentValidation;
using Monolith.Web.Identity.Requests.Accounts;

namespace Monolith.Web.Identity.Validators.Accounts
{
    public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
    {
        public SignUpRequestValidator()
        {
            RuleFor(model => model.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(model => model.Username)
                .NotEmpty();

            RuleFor(model => model.Password)
                .NotEmpty()
                .MinimumLength(8)
                .Matches(@"(?=.*[a-z])").WithMessage("'{PropertyName}' must contain at least one lowercase letter.")
                .Matches(@"(?=.*[A-Z])").WithMessage("'{PropertyName}' must contain at least one upper case letter.")
                .Matches(@"(?=.*\d)").WithMessage("'{PropertyName}' must contain at least one number.")
                .Matches(@"(?=.*[^a-zA-Z\d])").WithMessage("'{PropertyName}' must contain at least one special character.");
        }
    }
}

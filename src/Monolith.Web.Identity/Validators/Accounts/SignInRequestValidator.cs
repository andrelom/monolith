using FluentValidation;
using Monolith.Web.Identity.Requests.Accounts;

namespace Monolith.Web.Identity.Validators.Accounts
{
    public class SignInRequestValidator : AbstractValidator<SignInRequest>
    {
        public SignInRequestValidator()
        {
            RuleFor(model => model.Username)
                .NotEmpty();

            RuleFor(model => model.Password)
                .NotEmpty();
        }
    }
}

using FluentValidation;
using Monolith.Web.Identity.Requests.Accounts;

namespace Monolith.Web.Identity.Validators.Accounts
{
    public class ConfirmSignUpRequestValidator : AbstractValidator<ConfirmSignUpRequest>
    {
        public ConfirmSignUpRequestValidator()
        {
            RuleFor(model => model.Token)
                .NotEmpty();

            RuleFor(model => model.Username)
                .NotEmpty();
        }
    }
}

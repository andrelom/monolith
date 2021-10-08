using FluentValidation;
using Monolith.Web.Identity.Requests.Accounts;

namespace Monolith.Web.Identity.Validators.Accounts
{
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(model => model.Token)
                .NotEmpty();

            RuleFor(model => model.Username)
                .NotEmpty();

            RuleFor(model => model.Password)
                .NotEmpty();
        }
    }
}

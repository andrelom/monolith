using FluentValidation;
using Monolith.Web.Identity.Requests.Accounts;

namespace Monolith.Web.Identity.Validators.Accounts
{
    public class SendResetPasswordTokenRequestValidator : AbstractValidator<SendResetPasswordTokenRequest>
    {
        public SendResetPasswordTokenRequestValidator()
        {
            RuleFor(model => model.Username)
                .NotEmpty();
        }
    }
}

using System.Threading.Tasks;
using Monolith.Core;
using Monolith.Web.Identity.Models;
using Monolith.Web.Identity.Requests.Accounts;

namespace Monolith.Web.Identity.Services
{
    public interface IAccountService
    {
        Task<Result<SignIn>> SignInAsync(SignInRequest req);

        Task<Result> SignUpAsync(SignUpRequest req);

        Task<Result> SendSignUpConfirmationAsync(SendSignUpConfirmationRequest req);

        Task<Result> ConfirmSignUpAsync(ConfirmSignUpRequest req);

        Task<Result> SendResetPasswordTokenAsync(SendResetPasswordTokenRequest req);

        Task<Result> ResetPasswordAsync(ResetPasswordRequest req);
    }
}

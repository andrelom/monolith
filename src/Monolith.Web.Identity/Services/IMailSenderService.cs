using System.Threading.Tasks;
using Monolith.Core;
using Monolith.Web.Identity.Data.Entities;

namespace Monolith.Web.Identity.Services
{
    public interface IMailSenderService
    {
        Task<Result> SendSignUpConfirmationTokenAsync(User user, string token);

        Task<Result> SendResetPasswordTokenAsync(User user, string token);
    }
}

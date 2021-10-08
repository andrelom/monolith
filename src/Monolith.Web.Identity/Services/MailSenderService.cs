using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Monolith.Core;
using Monolith.Core.Extensions;
using Monolith.Foundation.Mail.Framework;
using Monolith.Web.Identity.Data.Entities;
using Monolith.Web.Identity.Options;

namespace Monolith.Web.Identity.Services
{
    public class MailSenderService : IMailSenderService
    {
        private readonly IConfiguration _configuration;

        private readonly IMailQueue _mailQueue;

        public MailSenderService(
            IConfiguration configuration,
            IMailQueue mailQueue)
        {
            _configuration = configuration;
            _mailQueue = mailQueue;
        }

        public async Task<Result> SendSignUpConfirmationTokenAsync(User user, string token)
        {
            var options = _configuration.Load<MailOptions>("Identity:Mail:ConfirmSignUp");
            var data = CreateBasicUserData(user);

            data.Add("token", token);

            await _mailQueue.EnqueueAsync(
                from: options.From,
                to: user.Email,
                view: options.View,
                data: data).ConfigureAwait(false);

            return Result.Success();
        }

        public async Task<Result> SendResetPasswordTokenAsync(User user, string token)
        {
            var options = _configuration.Load<MailOptions>("Identity:Mail:ResetPassword");
            var data = CreateBasicUserData(user);

            data.Add("token", token);

            await _mailQueue.EnqueueAsync(
                from: options.From,
                to: user.Email,
                view: options.View,
                data: data).ConfigureAwait(false);

            return Result.Success();
        }

        #region Private Methods

        private IDictionary<string, string> CreateBasicUserData(User user)
        {
            return new Dictionary<string, string>
            {
                { "username", user.UserName },
                { "email", user.Email }
            };
        }

        #endregion
    }
}

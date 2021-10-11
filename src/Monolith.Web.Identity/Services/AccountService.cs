using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Monolith.Core;
using Monolith.Web.Identity.Extensions;
using Monolith.Web.Identity.Models;
using Monolith.Web.Identity.Providers;
using Monolith.Web.Identity.Requests.Accounts;
using User = Monolith.Web.Identity.Data.Entities.User;

namespace Monolith.Web.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;

        private readonly ISecurityTokenProvider _securityTokenProvider;

        private readonly IMailSenderService _mailSenderService;

        public AccountService(
            UserManager<User> userManager,
            ISecurityTokenProvider securityTokenProvider,
            IMailSenderService mailSenderService)
        {
            _userManager = userManager;
            _securityTokenProvider = securityTokenProvider;
            _mailSenderService = mailSenderService;
        }

        public async Task<Result<SignIn>> SignInAsync(SignInRequest req)
        {
            if (await AuthorizeUserAsync(req.Username, req.Password).ConfigureAwait(false) is not { } user)
            {
                return Result<SignIn>.Failure(Errors.InvalidCredentials);
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return Result<SignIn>.Failure(Errors.EmailNotConfirmed);
            }

            var token = await _securityTokenProvider.GenerateAsync(user).ConfigureAwait(false);

            return Result<SignIn>.Success(new SignIn
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expires = token.ValidTo
            });
        }

        public async Task<Result> SignUpAsync(SignUpRequest req)
        {
            if (await _userManager.FindByEmailAsync(req.Email).ConfigureAwait(false) != null)
            {
                return Result.Failure(Errors.EmailAlreadyInUse);
            }

            if (await _userManager.FindByNameAsync(req.Username).ConfigureAwait(false) != null)
            {
                return Result.Failure(Errors.UsernameAlreadyInUse);
            }

            var user = CreateUser(req);

            if (await _userManager.CreateAsync(user, req.Password).ConfigureAwait(false) is { Succeeded: false } result)
            {
                return Result.Failure(Errors.Validation, result.Errors.ToArray());
            }

            return Result.Success();
        }

        public async Task<Result> SendSignUpConfirmationAsync(SendSignUpConfirmationRequest req)
        {
            var user = await _userManager.FindByNameAsync(req.Username).ConfigureAwait(false);

            if (user == null)
            {
                return Result.Failure(Errors.UserNotFound);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var res = await _mailSenderService.SendSignUpConfirmationTokenAsync(user, token).ConfigureAwait(false);

            return res.Succeeded
                ? Result.Success()
                : Result.Failure(res.Error, res.Validations?.ToArray());
        }

        public async Task<Result> ConfirmSignUpAsync(ConfirmSignUpRequest req)
        {
            var user = await _userManager.FindByNameAsync(req.Username).ConfigureAwait(false);

            if (user == null)
            {
                return Result.Failure(Errors.UserNotFound);
            }

            var res = await _userManager.ConfirmEmailAsync(user, req.Token);

            return res.Succeeded
                ? Result.Success()
                : Result.Failure(Errors.TokenExpired);
        }

        public async Task<Result> SendResetPasswordTokenAsync(SendResetPasswordTokenRequest req)
        {
            var user = await _userManager.FindByNameAsync(req.Username).ConfigureAwait(false);

            if (user == null)
            {
                return Result.Failure(Errors.UserNotFound);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
            var res = await _mailSenderService.SendResetPasswordTokenAsync(user, token).ConfigureAwait(false);

            return res.Succeeded
                ? Result.Success()
                : Result.Failure(res.Error, res.Validations?.ToArray());
        }

        public async Task<Result> ResetPasswordAsync(ResetPasswordRequest req)
        {
            var user = await _userManager.FindByNameAsync(req.Username).ConfigureAwait(false);

            if (user == null)
            {
                return Result.Failure(Errors.UserNotFound);
            }

            var res = await _userManager.ResetPasswordAsync(user, req.Token, req.Password).ConfigureAwait(false);

            return res.Succeeded
                ? Result.Success()
                : Result.Failure(Errors.TokenExpired);
        }

        #region Private Methods

        private async Task<User> AuthorizeUserAsync(string username, string password)
        {
            if (await _userManager.FindByNameAsync(username).ConfigureAwait(false) is not { } user)
            {
                return default;
            }

            if (!await _userManager.CheckPasswordAsync(user, password).ConfigureAwait(false))
            {
                return default;
            }

            return user;
        }

        private User CreateUser(SignUpRequest req)
        {
            return new()
            {
                Email = req.Email,
                UserName = req.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };
        }

        #endregion
    }
}

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monolith.Core.Mvc;
using Monolith.Web.Identity.Models;
using Monolith.Web.Identity.Requests.Accounts;
using Monolith.Web.Identity.Services;

namespace Monolith.Web.Identity.Controllers
{
    [AllowAnonymous]
    [ApiVersion("1")]
    public class AccountsController : ControllerApi
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [Route("signin")]
        [ProducesResponseType(typeof(SignIn), StatusCodes.Status200OK)]
        public async Task<IActionResult> SignInAsync([FromBody] SignInRequest req)
        {
            var res = await _accountService.SignInAsync(req).ConfigureAwait(false);

            return res.Succeeded
                ? Ok(res.Data)
                : StatusCode(StatusCodes.Status401Unauthorized, res);
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> SignUpAsync([FromBody] SignUpRequest reqSignUp)
        {
            var resSignUp = await _accountService.SignUpAsync(reqSignUp).ConfigureAwait(false);

            if (!resSignUp.Succeeded)
            {
                return StatusCode(StatusCodes.Status400BadRequest, resSignUp);
            }

            var reqSendSignUpConfirmation = new SendSignUpConfirmationRequest() { Username = reqSignUp.Username };
            var resSendSignUpConfirmation = await _accountService.SendSignUpConfirmationAsync(
                reqSendSignUpConfirmation).ConfigureAwait(false);

            // TODO: What to do with the account created earlier, if an error occurs?
            return resSendSignUpConfirmation.Succeeded
                ? Ok()
                : StatusCode(StatusCodes.Status400BadRequest, resSendSignUpConfirmation);
        }

        [HttpPost]
        [Route("confirmsignup")]
        public async Task<IActionResult> ConfirmSignUpAsync([FromBody] ConfirmSignUpRequest req)
        {
            var res = await _accountService.ConfirmSignUpAsync(req).ConfigureAwait(false);

            if (res.Succeeded)
            {
                return Ok();
            }

            return res.Error.Equals(Errors.TokenExpired)
                ? StatusCode(StatusCodes.Status403Forbidden, res)
                : StatusCode(StatusCodes.Status400BadRequest, res);
        }

        [HttpPost]
        [Route("password")]
        public async Task<IActionResult> SendResetPasswordTokenAsync([FromBody] SendResetPasswordTokenRequest req)
        {
            var res = await _accountService.SendResetPasswordTokenAsync(req).ConfigureAwait(false);

            return res.Succeeded
                ? Ok()
                : StatusCode(StatusCodes.Status400BadRequest, res);
        }

        [HttpPost]
        [Route("resetpassword")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequest req)
        {
            var res = await _accountService.ResetPasswordAsync(req).ConfigureAwait(false);

            if (res.Succeeded)
            {
                return Ok();
            }

            return res.Error.Equals(Errors.TokenExpired)
                ? StatusCode(StatusCodes.Status403Forbidden, res)
                : StatusCode(StatusCodes.Status400BadRequest, res);
        }
    }
}

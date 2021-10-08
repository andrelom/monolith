using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monolith.Core.Mvc;
using Monolith.Web.Identity.Models;
using Monolith.Web.Identity.Requests.Users;
using Monolith.Web.Identity.Services;

namespace Monolith.Web.Identity.Controllers
{
    [ApiVersion("1")]
    [Authorize(Roles = Roles.Administrator)]
    public class UsersController : ControllerApi
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        public IActionResult GetAll([FromQuery] GetAllRequest req)
        {
            var res = _userService.GetAll(req);

            return res.Succeeded
                ? Ok(res.Data)
                : StatusCode(StatusCodes.Status400BadRequest, res);
        }

        [HttpGet]
        [Route("{username}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByNameAsync([FromRoute] GetByNameRequest req)
        {
            var res = await _userService.GetByNameAsync(req).ConfigureAwait(false);

            return res.Succeeded
                ? Ok(res.Data)
                : StatusCode(StatusCodes.Status404NotFound, res);
        }
    }
}

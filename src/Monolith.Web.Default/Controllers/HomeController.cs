using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monolith.Core.Mvc;
using Monolith.Foundation.Identity;

namespace Monolith.Web.Default.Controllers
{
    [ApiVersion("1")]
    [Authorize(Roles = Roles.Administrator)]
    public class HomeController : ControllerApi
    {
        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}

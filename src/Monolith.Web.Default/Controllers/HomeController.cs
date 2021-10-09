using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monolith.Core.Mvc;

namespace Monolith.Web.Default.Controllers
{
    [ApiVersion("1")]
    [Authorize(Roles = "Administrator")]
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

using Microsoft.AspNetCore.Mvc;

namespace Monolith.Core.Mvc
{
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public abstract class ControllerApi : ControllerBase
    {
        public new OkObjectResult Ok()
        {
            return base.Ok(new { });
        }
    }
}

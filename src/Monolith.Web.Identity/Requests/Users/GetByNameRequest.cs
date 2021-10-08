using Microsoft.AspNetCore.Mvc;

namespace Monolith.Web.Identity.Requests.Users
{
    public class GetByNameRequest
    {
        [FromRoute(Name = "username")]
        public string Username { get; set; }
    }
}

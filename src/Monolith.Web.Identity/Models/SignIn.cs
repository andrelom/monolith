using System;

namespace Monolith.Web.Identity.Models
{
    public class SignIn
    {
        public string Token { get; set; }

        public DateTime? Expires { get; set; }
    }
}

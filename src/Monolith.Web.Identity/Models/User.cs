using System;

namespace Monolith.Web.Identity.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
    }
}

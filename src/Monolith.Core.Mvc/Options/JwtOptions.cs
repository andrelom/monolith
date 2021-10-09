using System.ComponentModel.DataAnnotations;
using Monolith.Core.Attributes;

namespace Monolith.Core.Mvc.Options
{
    [Option("Core:Mvc:JWT")]
    public class JwtOptions
    {
        [Required]
        public int ExpiresIn { get; set; }

        [Required]
        public string Audience { get; set; }

        [Required]
        public string Issuer { get; set; }

        [Required]
        [Environment("JWT_ISSUER_KEY")]
        public string IssuerKey { get; set; }
    }
}
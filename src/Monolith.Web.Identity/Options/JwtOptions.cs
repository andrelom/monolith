using System.ComponentModel.DataAnnotations;
using Monolith.Core.Attributes;

namespace Monolith.Web.Identity.Options
{
    [Option("Identity:JWT")]
    public class JwtOptions
    {
        [Required]
        public int ExpiresIn { get; set; }

        [Required]
        public string Audience { get; set; }

        [Required]
        public string Issuer { get; set; }

        [Required]
        [Environment("ISSUER_KEY")]
        public string IssuerKey { get; set; }
    }
}

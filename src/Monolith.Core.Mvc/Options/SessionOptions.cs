using System.ComponentModel.DataAnnotations;
using Monolith.Core.Attributes;

namespace Monolith.Core.Mvc.Options
{
    [Option("Core:Mvc:Session")]
    public class SessionOptions
    {
        [Required]
        public int Timeout { get; set; }

        [Required]
        public string Cookie { get; set; }
    }
}

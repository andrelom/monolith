using System.ComponentModel.DataAnnotations;
using Monolith.Core.Attributes;

namespace Monolith.Core.Mvc.Options
{
    [Option("Core:Mvc:Redis")]
    public class RedisOptions
    {
        [Required]
        public string Instance { get; set; }

        [Required]
        public string Hostname { get; set; }
    }
}

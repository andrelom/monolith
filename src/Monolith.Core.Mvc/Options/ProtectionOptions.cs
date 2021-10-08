using System.ComponentModel.DataAnnotations;
using Monolith.Core.Attributes;

namespace Monolith.Core.Mvc.Options
{
    [Option("Core:Mvc:Protection")]
    public class ProtectionOptions
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Environment("PROTECTION_KEYS_PATH")]
        public string Path { get; set; }
    }
}

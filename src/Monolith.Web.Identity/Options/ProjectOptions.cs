using System.ComponentModel.DataAnnotations;
using Monolith.Core.Attributes;

namespace Monolith.Web.Identity.Options
{
    [Option("Project")]
    public class ProjectOptions
    {
        [Required]
        public string ApplicationName { get; set; }

        [Required]
        [Environment("KEYS_PATH")]
        public string KeysPath { get; set; }
    }
}

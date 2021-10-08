using System.ComponentModel.DataAnnotations;
using Monolith.Core.Attributes;

namespace Monolith.Foundation.Mail.Options
{
    [Option("Foundation:Mail")]
    public class MailOptions
    {
        [Required]
        public string Path { get; set; }
    }
}

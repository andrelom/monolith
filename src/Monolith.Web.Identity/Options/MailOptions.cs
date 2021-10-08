using System.ComponentModel.DataAnnotations;

namespace Monolith.Web.Identity.Options
{
    public class MailOptions
    {
        [Required]
        public string From { get; set; }

        [Required]
        public string View { get; set; }
    }
}

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Monolith.Web.Identity.Extensions
{
    public static class IdentityErrorExtensions
    {
        public static string[] ToArray(this IEnumerable<IdentityError> errors)
        {
            return errors?.Select(error => error.Description).ToArray();
        }
    }
}

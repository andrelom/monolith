using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Monolith.Core.Extensions
{
    public static class StringExtensions
    {
        public static string Template(this string input, IDictionary<string, string> data)
        {
            const string pattern = @"\{{2}(\w+)\}{2}";
            var regex = new Regex(pattern, RegexOptions.Compiled);

            return regex.Replace(input, match =>
            {
                var group = match.Groups.FirstOrDefault<Group>();
                var key = Regex.Replace(group?.Value ?? string.Empty, pattern, "$1");

                return data.TryGetValue(key, out var value) ? value : string.Empty;
            });
        }
    }
}

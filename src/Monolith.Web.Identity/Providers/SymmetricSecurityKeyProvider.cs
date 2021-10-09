using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Monolith.Core.Extensions;
using Monolith.Core.Mvc.Options;

namespace Monolith.Web.Identity.Providers
{
    public class SymmetricSecurityKeyProvider : ISecurityKeyProvider
    {
        private readonly JwtOptions _jwtOptions;

        public SymmetricSecurityKeyProvider(IConfiguration configuration)
        {
            _jwtOptions = configuration.Load<JwtOptions>();
        }

        public SecurityKey Generate()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.IssuerKey));
        }
    }
}

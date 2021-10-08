using Microsoft.IdentityModel.Tokens;

namespace Monolith.Web.Identity.Providers
{
    public interface ISecurityKeyProvider
    {
        SecurityKey Generate();
    }
}

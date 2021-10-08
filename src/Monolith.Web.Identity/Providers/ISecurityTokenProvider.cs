using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Monolith.Web.Identity.Data.Entities;

namespace Monolith.Web.Identity.Providers
{
    public interface ISecurityTokenProvider
    {
        Task<SecurityToken> GenerateAsync(User user);
    }
}

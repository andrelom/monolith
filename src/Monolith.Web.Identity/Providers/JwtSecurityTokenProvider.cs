using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Monolith.Core.Extensions;
using Monolith.Web.Identity.Data.Entities;
using Monolith.Web.Identity.Options;

namespace Monolith.Web.Identity.Providers
{
    public class JwtSecurityTokenProvider : ISecurityTokenProvider
    {
        private readonly JwtOptions _jwtOptions;

        private readonly UserManager<User> _userManager;

        private readonly ISecurityKeyProvider _securityKeyProvider;

        public JwtSecurityTokenProvider(
            IConfiguration configuration,
            UserManager<User> userManager,
            ISecurityKeyProvider securityKeyProvider)
        {
            _jwtOptions = configuration.Load<JwtOptions>();
            _userManager = userManager;
            _securityKeyProvider = securityKeyProvider;
        }

        public async Task<SecurityToken> GenerateAsync(User user)
        {
            var key = _securityKeyProvider.Generate();
            var claims = await CreateJwtUserClaims(user).ConfigureAwait(false);

            return new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                expires: DateTime.UtcNow.AddHours(_jwtOptions.ExpiresIn),
                claims: claims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha512));
        }

        #region Private Methods

        private async Task<IEnumerable<Claim>> CreateJwtUserClaims(User user)
        {
            var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            return claims;
        }

        #endregion
    }
}

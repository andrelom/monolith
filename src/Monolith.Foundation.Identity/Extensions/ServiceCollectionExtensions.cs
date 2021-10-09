using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Monolith.Core.Extensions;
using Monolith.Foundation.Identity.Options;

namespace Monolith.Foundation.Identity.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFoundationIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.Load<JwtOptions>();

            var service = services.AddAuthentication(authentication =>
            {
                authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                authentication.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            service.AddJwtBearer(jwt =>
            {
                // In distributed applications, it should be false.
                jwt.SaveToken = false;

                // Parameters used to validate the identity token.
                jwt.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidIssuer = options.Issuer,
                    ValidateAudience = true,
                    ValidAudience = options.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.IssuerKey))
                };
            });

            return services;
        }
    }
}

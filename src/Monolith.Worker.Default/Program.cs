using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Monolith.Core;
using Monolith.Foundation.Mail.Extensions;
using Monolith.Foundation.Messaging.Extensions;

namespace Monolith.Worker.Default
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            Dependencies.Initialize();

            return Host.CreateDefaultBuilder(args).ConfigureServices(ConfigureServices);
        }

        #region Private Methods

        private static void ConfigureServices(HostBuilderContext builder, IServiceCollection services)
        {
            //
            // Foundation

            services
                .AddFoundationMessaging()
                .AddFoundationMail()
                .AddFoundationMailHostedServices();
        }

        #endregion
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Monolith.Core;

namespace Monolith.Web.Identity
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

            return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(ConfigureWebHostDefaults);
        }

        #region Private Methods

        private static void ConfigureWebHostDefaults(IWebHostBuilder builder)
        {
            builder.UseStartup<Startup>();
        }

        #endregion
    }
}

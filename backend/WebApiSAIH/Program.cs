using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiSAIH.Configuration;
using WebApiSAIH.Models;

namespace WebApiSAIH
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    // Set only in Azure (App Service setting). Absent locally/Docker, so this is a no-op there.
                    var keyVaultName = config.Build()["KeyVaultName"];
                    if (!string.IsNullOrEmpty(keyVaultName))
                    {
                        config.AddAzureKeyVault(
                            new Uri($"https://{keyVaultName}.vault.azure.net/"),
                            new DefaultAzureCredential(),
                            new SiteOpsKeyVaultSecretManager());
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

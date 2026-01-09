using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace GTracker.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            var settings = new Dictionary<string, string?>
            {
                // Change key name to match YOUR appsettings key
                ["ConnectionStrings:DefaultConnection"] =
                    "Server=TOBYY\\SQLEXPRESS01;Database=dbi559954_gametracketest;Trusted_Connection=True;TrustServerCertificate=True;"
            };

            config.AddInMemoryCollection(settings);
        });
    }
}

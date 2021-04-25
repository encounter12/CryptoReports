using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace CryptoReports.WebReportDesigner
{
    public class ConfigurationService
    {
        public ConfigurationService(IWebHostEnvironment environment)
        {
            this.Environment = environment;

            var configFileName = System.IO.Path.Combine(environment.ContentRootPath, "appsettings.json");
            var config = new ConfigurationBuilder()
                .AddJsonFile(configFileName, true)
                .Build();

            this.Configuration = config;
        }

        public IConfiguration Configuration { get; private set; }

        public IWebHostEnvironment Environment { get; private set; }
    }
}
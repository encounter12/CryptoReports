using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Telerik.Reporting.Cache.File;
using Telerik.Reporting.Services;
using Telerik.WebReportDesigner.Services;

namespace CryptoReports.WebReportDesigner
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllersWithViews()
                .AddNewtonsoftJson();
            
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            
            // Configure dependencies for ReportServiceConfiguration.
            services.TryAddSingleton<ConfigurationService>(sp => new ConfigurationService(sp.GetService<IWebHostEnvironment>()));
            services.TryAddScoped<IReportSourceResolver>(sp =>
                new TypeReportSourceResolver().AddFallbackResolver(new UriReportSourceResolver(
                    Path.Combine(sp.GetRequiredService<ConfigurationService>().Environment.WebRootPath, "Reports"))));
            services.TryAddScoped<IReportServiceConfiguration>(sp =>
                new ReportServiceConfiguration
                {
                    ReportingEngineConfiguration = sp.GetRequiredService<ConfigurationService>().Configuration,
                    HostAppId = "Html5DemoAppCore",
                    Storage = new FileStorage(),
                    ReportSourceResolver = sp.GetRequiredService<IReportSourceResolver>()
                });

            // Configure dependencies for ReportDesignerServiceConfiguration.  
            services.TryAddScoped<IDefinitionStorage>(sp => new FileDefinitionStorage(Path.Combine(sp.GetRequiredService<ConfigurationService>().Environment.WebRootPath, "Reports")));

            var gosho = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            
            services.TryAddScoped<IReportDesignerServiceConfiguration>(sp => new ReportDesignerServiceConfiguration
            {
                DefinitionStorage = sp.GetRequiredService<IDefinitionStorage>(),
                SettingsStorage = new FileSettingsStorage(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Telerik Reporting"))
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
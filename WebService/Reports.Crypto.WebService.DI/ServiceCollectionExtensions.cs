using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Reports.Crypto.WebService.DAL.Context;
using Reports.Crypto.WebService.DAL.Repositories;
using Reports.Crypto.WebService.DAL.Repositories.Contracts;
using Reports.Crypto.WebService.Infrastructure.Helpers.Http;
using Reports.Crypto.WebService.Services;
using Reports.Crypto.WebService.Services.Contracts;

namespace Reports.Crypto.WebService.DI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencyInjectionBindings(
            this IServiceCollection services, string connectionString)
        {
            BindServices(services);
            BindInfrastructureHelpers(services);
            BindDbContexts(services, connectionString);
            BindRepositories(services);

            return services;
        }
        
        private static void BindServices(IServiceCollection services)
        {
            services.AddScoped<ICryptocurrencyService, CryptocurrencyService>();
        }
        
        private static void BindInfrastructureHelpers(IServiceCollection services)
        {
            services.AddScoped<IHttpHelpers, HttpHelpers>();
        }

        private static void BindRepositories(IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ICryptocurrencyRepository, CryptocurrencyRepository>();
        }

        private static void BindDbContexts(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<CryptocurrenciesDbContext>(options =>
                options
                    .UseLazyLoadingProxies()
                    .UseSqlite(connectionString));
        }
    }
}
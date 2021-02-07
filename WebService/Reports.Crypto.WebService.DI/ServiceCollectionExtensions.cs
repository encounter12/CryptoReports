using System;
using System.Net;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Reports.Crypto.WebService.DAL.Context;
using Reports.Crypto.WebService.DAL.Repositories;
using Reports.Crypto.WebService.DAL.Repositories.Contracts;
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
            BindDbContexts(services, connectionString);
            BindRepositories(services);
            BindHttpClient(services);

            return services;
        }
        
        private static void BindServices(IServiceCollection services)
        {
            services.AddTransient<ICryptocurrencyDataService, CryptocurrencyDataService>();
        }

        private static void BindRepositories(IServiceCollection services)
        {
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<ICryptocurrencyDataRepository, CryptocurrencyDataRepository>();
        }

        private static void BindDbContexts(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<CryptocurrenciesDbContext>(options =>
                options
                    .UseLazyLoadingProxies()
                    .UseSqlite(connectionString),
                ServiceLifetime.Transient);
        }

        private static void BindHttpClient(IServiceCollection services)
        {
            services.AddHttpClient<ICryptocurrencyDataService, CryptocurrencyDataService>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());
        }
        
        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            Random jitterer = new Random();
            
            var retryWithJitterPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6,    // exponential back-off plus some jitter
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                                    + TimeSpan.FromMilliseconds(jitterer.Next(0, 100))
                );

            return retryWithJitterPolicy;
        }
        
        static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }
    }
}
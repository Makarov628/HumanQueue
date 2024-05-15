using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using HQ.Application.Common.Interfaces;
using HQ.Application.Persistence;
using HQ.Application.Printer;

using HQ.Infrastructure.Authentication;
using HQ.Infrastructure.ExternalPrinters;
using HQ.Infrastructure.Persistence;
using HQ.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using HQ.Infrastructure.Persistence.Interceptors;

namespace HQ.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            ConfigurationManager config)
        {
            
            services.Configure<JwtSettings>(config.GetSection(JwtSettings.sectionName));
            services.AddSingleton<IJwtTokenGeneration, JwtTokenGeneration>();

            services.AddDbContext<HQDbContext>(options =>
            {
                options.UseNpgsql(config.GetConnectionString("PostgreSQL"));
            });

            services.AddScoped<IQueueRepository, QueueRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<ITerminalRepository, TerminalRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWindowRepository, WindowRepository>();
            services.AddScoped<PublishDomainEventsInterceptor>();

            services.Configure<List<NetworkPrinterSettings>>(config.GetSection("NetworkPrinters"));
            services.AddSingleton<IExternalPrinterProvider, NetworkPrinterProvider>();

            return services;
        }
    }

}
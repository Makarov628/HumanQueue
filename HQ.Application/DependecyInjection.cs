using HQ.Application.Services.CurrentUser;

using Microsoft.Extensions.DependencyInjection;

namespace HQ.Application
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<CurrentUserService>();

            return services;
        }
    }
}
using HQ.UseCases.Auth.Queries.Login;
using HQ.UseCases.Common.Behaviors;

using ErrorOr;

using FluentValidation;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HQ.UseCases
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            // services.AddMediatR(typeof(DependencyInjection).Assembly);
            // services.AddScoped(
            //     typeof(IPipelineBehavior<,>),
            //     typeof(ValidationBehavior<,>));

            // services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}

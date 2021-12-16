using FooBarServiceTracker.Api.BusinessLogic;
using FooBarServiceTracker.Api.BusinessLogic.Interfaces;
using FooBarServiceTracker.Api.Infrastructure.Filters;
using FooBarServiceTracker.Api.Infrastructure.Mapping;

namespace FooBarServiceTracker.Api.Infrastructure.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddAutoMapper(x => x.AddProfile(typeof(ServiceMapProfile)));
            services.AddScoped<IServiceMaintenanceService, ServiceMaintenanceService>();
            services.AddScoped<ServiceValidationFilter>();
        }
    }
}

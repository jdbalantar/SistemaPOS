using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace ApplicationLayer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        }
    }
}

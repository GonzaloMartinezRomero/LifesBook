using LifesBook.Backend.Infraestructure.Service.Persistence.Abstract;
using LifesBook.Backend.Infraestructure.Service.Persistence.Implementation;
using LifesBook.Backend.Infraestructure.Service.Security.Abstract;
using LifesBook.Backend.Infraestructure.Service.Security.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace LifesBook.Backend.Infraestructure
{
    public static class Startup
    {
        public static IServiceCollection AddHistorySecurity(this IServiceCollection services)
        {
            services.AddScoped<IHistorySecurity, HistorySecurity>();
            return services;
        }

        public static IServiceCollection AddHistoryPersistence(this IServiceCollection services)
        {
            services.AddScoped<IHistoryPersistence, HistoryFilePersistence>();
            return services;
        }
    }
}
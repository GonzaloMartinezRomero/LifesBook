using LifesBook.Backend.Application.Service.HistoryManager.Abstract;
using LifesBook.Backend.Application.Service.HistoryManager.Implementation;
using LifesBook.Backend.Infraestructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LifesBook.Backend.Application
{
    public static class Startup
    {
        public static IServiceCollection AddHistoryManagerApplication(this IServiceCollection service)
        {
            service.AddHistoryPersistence();
            service.AddHistorySecurity();
            service.AddScoped<IHistoryManager, HistoryManager>();

            return service;
        }
    }
}
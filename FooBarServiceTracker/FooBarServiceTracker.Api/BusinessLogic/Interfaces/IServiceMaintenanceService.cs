using FooBarServiceTracker.Api.Infrastructure.Entities;

namespace FooBarServiceTracker.Api.BusinessLogic.Interfaces
{
    public interface IServiceMaintenanceService
    {
        Task<IEnumerable<Service>> GetAll();
        Task<Service?> GetByName(string serviceName);
        Task<IEnumerable<Service>> GetByLabels(KeyValuePair<string, string> label);
        Task<Service?> CreateService(Service service);
        Task<Service?> UpdateService(Service serviceToUpdate);
        Task<bool> DeleteService(string serviceName);
    }
}

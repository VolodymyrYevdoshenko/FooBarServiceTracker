using FooBarServiceTracker.Api.BusinessLogic.Interfaces;
using FooBarServiceTracker.Api.DataAccess;
using FooBarServiceTracker.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace FooBarServiceTracker.Api.BusinessLogic
{
    public class ServiceMaintenanceService : IServiceMaintenanceService
    {
        private readonly ServiceDbContext _context;

        public ServiceMaintenanceService(ServiceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Service>> GetAll()
        {
            return await _context.Services.ToListAsync();
        }

        public async Task<Service?> GetByName(string serviceName)
        {
            return await _context.Services.FirstOrDefaultAsync(s => s.Name.Equals(serviceName));
        }

        public async Task<IEnumerable<Service>> GetByLabels(KeyValuePair<string, string> label)
        {
            var services = await _context.Services.ToListAsync();

            return services.Where(s => s.Labels.Contains(label)).Distinct().ToList();
        }

        public async Task<Service?> CreateService(Service service)
        {
            if (_context.Services.Any(s=>s.Name==service.Name))
            {
                return null;
            }
            var result = _context.Services.Add(service);

            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Service?> UpdateService(Service serviceToUpdate)
        {
            var service = await _context.Services.FirstOrDefaultAsync(s => s.Name.Equals(serviceToUpdate.Name));

            if (service is null)
            {
                return service;
            }

            if (serviceToUpdate.Port.HasValue)
            {
                service.Port = serviceToUpdate.Port;
            }
            if (serviceToUpdate.Maintainer!=null)
            {
                service.Maintainer = serviceToUpdate.Maintainer;
            }
            if (serviceToUpdate.Labels!=null)
            {
                service.Labels = serviceToUpdate.Labels;
            }

            await _context.SaveChangesAsync();

            return service;
        }

        public async Task<bool> DeleteService(string serviceName)
        {
            var serviceToDelete = await _context.Services.FirstOrDefaultAsync(s => s.Name.Equals(serviceName));
            if (serviceToDelete is null)
            {
                return false;
            }

            _context.Services.Remove(serviceToDelete);
           await _context.SaveChangesAsync();

           return true;
        }
    }
}

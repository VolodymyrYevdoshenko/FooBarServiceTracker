using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FooBarServiceTracker.Api.BusinessLogic;
using FooBarServiceTracker.Api.DataAccess;
using FooBarServiceTracker.Api.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FooBarServiceTracker.Tests
{
    [TestClass]
    public class ServiceMaintenanceServiceTests
    {
        private readonly ServiceDbContext _context;

        public ServiceMaintenanceServiceTests()
        {
            _context = GetContext();
        }

        [TestInitialize]
        public void TestInit()
        {
            _context.Services.RemoveRange(_context.Services);
        }

        [TestMethod]
        public async Task GetAll_ShouldReturnCorrectResults()
        {
            var services = GetServices();
            await _context.Services.AddRangeAsync(services);
            await _context.SaveChangesAsync();

            var service = new ServiceMaintenanceService(_context);

            var result = await service.GetAll();

            Assert.AreEqual(services.Count, result.Count());
        }

        [TestMethod]
        public async Task GetByName_ShouldReturnCorrectResult()
        {
            var services = GetServices();
            var firstService = services.First();

            await _context.Services.AddRangeAsync(services);
            await _context.SaveChangesAsync();

            var service = new ServiceMaintenanceService(_context);

            var result = await service.GetByName(firstService.Name);

            Assert.IsNotNull(result);
            Assert.AreEqual(firstService.Name, result.Name);
        }

        [TestMethod]
        public async Task GetByName_ShouldReturnNull()
        {
            var services = GetServices();
            await _context.Services.AddRangeAsync(services);
            await _context.SaveChangesAsync();

            var service = new ServiceMaintenanceService(_context);

            var result = await service.GetByName("Wrong name");

            Assert.IsNull(result);
        }

        [TestMethod]
        [DataRow("Label1", "Value1")]
        [DataRow("Label2", "Value2")]
        [DataRow("Label3", "Value3")]
        public async Task GetByLabel_ShouldReturnCorrectResults(string label, string value)
        {
            var services = GetServices();
            
            await _context.Services.AddRangeAsync(services);
            await _context.SaveChangesAsync();

            var service = new ServiceMaintenanceService(_context);

            var result = await service.GetByLabels(new KeyValuePair<string, string>(label, value));

            Assert.AreEqual(
                services.Count(s => s.Labels != null && s.Labels.ContainsKey(label) && s.Labels?[label] == value),
                result.Count());
        }

        [TestMethod]
        public async Task Create_ShouldCreateCorrectItem()
        {
            var firstService = GetServices().First();

            var context = GetContext("Create");
            var service = new ServiceMaintenanceService(context);

            await service.CreateService(firstService);

            var createdService = context.Services.FirstOrDefault();
            Assert.AreEqual(firstService.Name, createdService?.Name);
            Assert.AreEqual(firstService.Port, createdService?.Port);
            Assert.AreEqual(firstService.Maintainer, createdService?.Maintainer);
        }

        [TestMethod]
        public async Task Update_ShouldUpdateCorrectly()
        {
            var firstService = GetServices().First();

            var context = GetContext("Update");
            await context.Services.AddAsync(firstService);
            await context.SaveChangesAsync();

            var service = new ServiceMaintenanceService(context);

            var serviceToUpdate = new Service
            {
                Name = firstService.Name,
                Port = 98643
            };

            await service.UpdateService(serviceToUpdate);

            var updatedService = context.Services.FirstOrDefault(s=>s.Name==firstService.Name);
            Assert.AreEqual(firstService.Name, updatedService?.Name);
            Assert.AreEqual(serviceToUpdate.Port, updatedService?.Port);
        }

        [TestMethod]
        public async Task Delete_ShouldDeleteCorrectly()
        {
            var firstService = GetServices().First();

            var context = GetContext("Delete");
            await context.Services.AddAsync(firstService);
            await context.SaveChangesAsync();

            var service = new ServiceMaintenanceService(context);

            await service.DeleteService(firstService.Name);

            var deletedService = context.Services.FirstOrDefault(s => s.Name == firstService.Name);
            Assert.IsNull(deletedService);
        }

        private static List<Service> GetServices()
        {
            return new List<Service>
            {
                new()
                {
                    Name = "Service1", Port = 25657, Maintainer = "john@wred.cyt",
                    Labels = new Dictionary<string, string>() { { "Label1", "Value1" }, { "Label2", "Value2" } }
                },
                new()
                {
                    Name = "Service2", Port = 54642, Maintainer = "eregf@ppe.rry",
                    Labels = new Dictionary<string, string>() { { "Label1", "Value1" }, { "Label3", "Value3" } }
                },
                new()
                {
                    Name = "Service3", Port = 124577, Maintainer = "rees@wred.cyt",
                    Labels = new Dictionary<string, string>() { { "Label2", "Value2" } }
                }
            };
        }

        private static ServiceDbContext GetContext(string? dbName = null)
        {
            var options = new DbContextOptionsBuilder<ServiceDbContext>()
                .UseInMemoryDatabase(databaseName: dbName ?? "FooBarDb")
                .Options;
            return new ServiceDbContext(options);
        }
    }
}
using FooBarServiceTracker.Api.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FooBarServiceTracker.Api.DataAccess
{
    public class ServiceDbContext : DbContext
    {
        public DbSet<Service> Services { get; set; }

        public ServiceDbContext()
        {
            
        }
        public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // modelBuilder.Entity<Service>().ToTable("Services");

            modelBuilder.Entity<Service>()
                .Property(s => s.Labels)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<Dictionary<string, string>>(v));
        }
    }
}

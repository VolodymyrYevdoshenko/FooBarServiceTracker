using System.ComponentModel.DataAnnotations;

namespace FooBarServiceTracker.Api.Infrastructure.Entities
{
    public class Service : EntityBase
    {
        [MaxLength(30)] [MinLength(4)] 
        public string Name { get; set; } = string.Empty;   
        public int? Port { get; set; }
        public string? Maintainer { get; set; }
        public Dictionary<string, string>? Labels { get; set; } 
    }
}

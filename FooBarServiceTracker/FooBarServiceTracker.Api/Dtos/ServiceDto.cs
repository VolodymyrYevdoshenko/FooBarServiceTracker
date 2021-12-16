namespace FooBarServiceTracker.Api.Dtos
{
    public class ServiceDto
    {
        public string Name { get; init; } = string.Empty;
        public int? Port { get; init; }
        public string? Maintainer { get; init; }
        public Dictionary<string, string>? Labels { get; init; } 
    }
}

using AutoMapper;
using FooBarServiceTracker.Api.Dtos;
using FooBarServiceTracker.Api.Entities;

namespace FooBarServiceTracker.Api.Infrastructure.Mapping
{
    public class ServiceMapProfile : Profile
    {
        public ServiceMapProfile()
        {
            CreateMap<ServiceDto, Service>().ForMember(s => s.Id, opt => opt.Ignore());
            CreateMap<Service, ServiceDto>();
        }
    }
}

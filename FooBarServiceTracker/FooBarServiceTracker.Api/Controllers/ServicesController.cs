using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FooBarServiceTracker.Api.BusinessLogic.Interfaces;
using FooBarServiceTracker.Api.Dtos;
using FooBarServiceTracker.Api.Entities;
using FooBarServiceTracker.Api.Infrastructure.Filters;
using Microsoft.AspNetCore.Mvc;

namespace FooBarServiceTracker.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceMaintenanceService _service;
        private readonly IMapper _mapper;

        public ServicesController(IServiceMaintenanceService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all services from DB or with provided label
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ServiceDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ServiceDto>> GetAll([FromQuery] string? label)
        {
            var services = string.IsNullOrEmpty(label) ? await _service.GetAll() : await _service.GetByLabels(ConvertLabel());

            return Ok(_mapper.Map<IEnumerable<Service>, IEnumerable<ServiceDto>>(services));

            KeyValuePair<string, string> ConvertLabel()
            {
                var parts = label.Split(':'); 
                return new KeyValuePair<string, string>(parts[0].Trim(), parts[1].Trim());
            }
        }

        /// <summary>
        /// Returns service by name
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        [HttpGet("{serviceName}")]
        [ProducesResponseType(typeof(ServiceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ServiceDto>> GetByName([FromRoute] string serviceName)
        {
            var service = await _service.GetByName(serviceName);

            if (service is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Service, ServiceDto>(service));
        }

        /// <summary>
        /// Creates a new service
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ServiceDto), StatusCodes.Status200OK)]
        [ServiceFilter(typeof(ServiceValidationFilter))]
        public async Task<ActionResult<ServiceDto>> Create([FromBody] ServiceDto input)
        {
            var serviceToCreate = _mapper.Map<ServiceDto, Service>(input);

            var createdService = await _service.CreateService(serviceToCreate);

            if (createdService is null)
            {
                return BadRequest("Service with this name already exists.");
            }

            return Ok(_mapper.Map<Service, ServiceDto>(createdService));
        }

        /// <summary>
        /// Updates provided fields of service.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ServiceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(ServiceValidationFilter))]
        public async Task<ActionResult<ServiceDto>> Update([FromBody] ServiceDto input)
        {
            var serviceToUpdate = _mapper.Map<ServiceDto, Service>(input);

            var updatedService = await _service.UpdateService(serviceToUpdate);

            if (updatedService is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Service, ServiceDto>(updatedService));
        }

        /// <summary>
        /// Remove service by name
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        [HttpDelete("{serviceName}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] string serviceName)
        {
            var result = await _service.DeleteService(serviceName);

            return result ? Accepted() : NotFound();
        }
    }
}
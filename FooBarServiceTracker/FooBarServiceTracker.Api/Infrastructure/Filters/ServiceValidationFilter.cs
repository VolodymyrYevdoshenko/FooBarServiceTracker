using System.Net.Mail;
using System.Text.RegularExpressions;
using FooBarServiceTracker.Api.Infrastructure.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FooBarServiceTracker.Api.Infrastructure.Filters
{
    public class ServiceValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var input = context.ActionArguments.SingleOrDefault(a => a.Value is ServiceDto);

            if (input.Value is null)
            {
                context.Result = new BadRequestResult();
                return;
            }

            var service = input.Value as ServiceDto;

            if (service?.Name is null || service.Name.Length is < 4 or > 30)
            {
                context.Result = new BadRequestObjectResult("Service name is not valid.");
                return;
            }

            if (service.Port is < 0 or > 65535)
            {
                context.Result = new BadRequestObjectResult("Service port is not valid.");
                return;
            }

            if (service.Maintainer is not null && !IsEmailValid(service.Maintainer))
            {
                context.Result = new BadRequestObjectResult("Service maintainer is not valid.");
                return;
            }

            await next();
        }

        private bool IsEmailValid(string email)
        {
            var regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                RegexOptions.CultureInvariant | RegexOptions.Singleline);
            return regex.IsMatch(email);
        }
    }
}

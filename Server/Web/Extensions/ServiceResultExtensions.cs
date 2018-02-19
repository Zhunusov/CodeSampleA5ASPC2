using Domain.Core;
using Microsoft.AspNetCore.Mvc;

namespace Web.Extensions
{
    static class ServiceResultExtensions
    {
        public static IActionResult ToActionResult(this ServiceResult servicesResult)
        {
            if (servicesResult.Succeeded)
            {
                return new OkResult();
            }

            if (servicesResult.Errors.Count > 0)
                return new BadRequestObjectResult(servicesResult.Errors);
            return new BadRequestResult();
        }
    }
}

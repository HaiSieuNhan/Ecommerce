using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared
{
    public class AuthorizationAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                context.Result = new JsonResult("Forbidden")
                {
                    StatusCode = (int)System.Net.HttpStatusCode.Forbidden,
                    Value = new
                    {
                        Status = "Error",
                        Message = "Access denied"
                    },
                };
            }
        }
    }
}

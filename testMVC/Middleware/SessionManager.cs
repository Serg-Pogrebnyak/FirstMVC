using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace testMVC.Middleware
{
    public class SessionManager
    {
        private readonly RequestDelegate _next;

        public SessionManager(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                await _next.Invoke(context);
            }
            else
            {
                var coockie = context.Request.Cookies["BasketId"];
                if (coockie == null)
                {
                    var response = context.Response;
                    response.Cookies.Append("BasketId", "12345");
                }
                await _next.Invoke(context);
            }
        }
    }
}

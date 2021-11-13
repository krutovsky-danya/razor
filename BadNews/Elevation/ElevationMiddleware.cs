using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BadNews.Elevation
{
    public class ElevationMiddleware
    {
        private readonly RequestDelegate next;
    
        public ElevationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        private readonly CookieOptions cookieOptions = new()
        {
            HttpOnly = true
        };
    
        public async Task InvokeAsync(HttpContext context)
        {
            await next(context);

            var path = context.Request.Path;
            if (path.Value == null) return;
            if (!path.Value.EndsWith("/elevation")) return;

            var cookies = context.Response.Cookies;
            if (context.Request.Query.ContainsKey("up"))
                cookies.Append(ElevationConstants.CookieName, ElevationConstants.CookieValue, cookieOptions);
            else
                cookies.Delete(ElevationConstants.CookieName);

            context.Response.Redirect("/");
        }
    }
}

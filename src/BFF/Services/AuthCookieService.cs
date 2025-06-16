using BFF.Abstractions;
using Yarp.ReverseProxy.Transforms;

namespace BFF.Services
{
    internal class AuthCookieService : IAuthCookieService
    {
        private const string CookieName = "AuthCookie";
        public string? ReadFromRequest(RequestTransformContext context)
        {
            context.HttpContext.Request.Cookies.TryGetValue(CookieName, out var cookieValue);
            return cookieValue;
        }

        public void RemoveAuthenticationCookie(RequestTransformContext ctx)
        {
            var cookieFullValue = GetAuthCookieHeaderValue(ctx.HttpContext);
            if (string.IsNullOrEmpty(cookieFullValue))
                return;

            var allCookies = RequestTransform.TakeHeader(ctx, "Cookie");
            var cookiesToAppend = allCookies.Where(x => x != cookieFullValue);
            if (cookiesToAppend.Any())
                ctx.ProxyRequest.Headers.Add("Cookie", cookiesToAppend);
        }

        public void AddAuthCookieToResponse(ResponseTransformContext context, string cookieValue)
        {
            var builder = new CookieBuilder();
            builder.Name = CookieName;
            builder.IsEssential = true;
            builder.MaxAge = TimeSpan.FromDays(30);
            builder.Expiration = TimeSpan.FromDays(30);
            builder.HttpOnly = true;
            builder.SecurePolicy = CookieSecurePolicy.Always;
            builder.SameSite = SameSiteMode.None;
            var cookieOptions = builder.Build(context.HttpContext);
            var cookieHeader = cookieOptions.CreateCookieHeader(CookieName, cookieValue);

            ResponseTransform.SetHeader(context, "Set-Cookie", cookieHeader.ToString());
        }

        private string? GetAuthCookieHeaderValue(HttpContext ctx)
        {
            if (ctx.Request.Cookies.TryGetValue(CookieName, out var result))
            {
                return $"{CookieName}={result}";
            }
            else
                return null;
        }
    }
}

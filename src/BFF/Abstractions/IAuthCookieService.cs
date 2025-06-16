using Yarp.ReverseProxy.Transforms;

namespace BFF.Abstractions
{
    public interface IAuthCookieService
    {
        string? ReadFromRequest(RequestTransformContext context);
        void RemoveAuthenticationCookie(RequestTransformContext ctx);
        void AddAuthCookieToResponse(ResponseTransformContext context, string cookieValue);
    }
}

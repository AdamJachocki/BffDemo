using BFF.Abstractions;
using BFF.Extensions;
using Yarp.ReverseProxy.Transforms;

namespace BFF.Transforms
{
    internal class AccessTokenResponseTransform(IAccessTokenEncoder _tokenEncoder,
    IAuthCookieService _authCookieService) : ResponseTransform
    {
        public override async ValueTask ApplyAsync(ResponseTransformContext context)
        {
            var tokenModel = await ReadTokenModelFromBody(context) ?? context.HttpContext.GetTokens();
            if (tokenModel == null)
                return;

            var cookieValue = _tokenEncoder.EncodeTokens(tokenModel);
            _authCookieService.AddAuthCookieToResponse(context, cookieValue);
        }


        private async Task<AuthTokenModel?> ReadTokenModelFromBody(ResponseTransformContext context)
        {
            if (context.ProxyResponse == null)
                return null;

            if (!context.ProxyResponse.Headers.HasAuthToken())
                return null;

            var result = await context.ProxyResponse.Content.ReadFromJsonAsync<AuthTokenModel>();
            context.SuppressResponseBody = true;
            return result;
        }
    }

}

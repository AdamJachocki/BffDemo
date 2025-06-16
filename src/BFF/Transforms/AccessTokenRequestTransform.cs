using BFF.Abstractions;
using Yarp.ReverseProxy.Transforms;

namespace BFF.Transforms
{
    internal class AccessTokenRequestTransform(IAuthCookieService _authCookieService, 
        IAccessTokenEncoder _tokenEncoder) : RequestTransform
    {
        public override async ValueTask ApplyAsync(RequestTransformContext context)
        {
            var cookieValue = _authCookieService.ReadFromRequest(context);
            if (string.IsNullOrEmpty(cookieValue))
                return;

            var tokens = _tokenEncoder.DecodeTokens(cookieValue!);
            if (tokens != null)
                tokens = await RefreshTokensIfNeeded(context, tokens);

            if (tokens != null)
                AddAuthHeader(context, tokens.AccessToken);

            _authCookieService.RemoveAuthenticationCookie(context);
        }

        private void AddAuthHeader(RequestTransformContext context, string accessToken)
        {
            context.ProxyRequest.Headers.Add("Authorization", $"Bearer {accessToken}");
        }

        private Task<AuthTokenModel> RefreshTokensIfNeeded(RequestTransformContext context, AuthTokenModel tokens)
        {
            //Potraktuj tę metodę jako DUŻY skrót myślowy. Generalnie chodzi o to, żeby sprawdzić, czy token jest ważny i ewentualnie go odświeżyć w ODPOWIEDNI SPOSÓB.

            if (tokens.ValidTo > DateTimeOffset.UtcNow.AddMinutes(5))
                return Task.FromResult(tokens);

            //UWAGA! To tylko przykład, a nie prawdziwe odświeżanie tokenów.
            tokens.ValidTo = tokens.ValidTo.AddMinutes(15);
            return Task.FromResult(tokens);
        }
    }
}
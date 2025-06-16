namespace BFF.Extensions
{
    internal static class HttpContextExtensions
    {
        private const string AuthTokenKey = "Token";
        public static void SaveTokens(this HttpContext ctx, AuthTokenModel data)
        {
            ctx.Items[AuthTokenKey] = data;
        }

        public static AuthTokenModel? GetTokens(this HttpContext ctx)
        {
            if (ctx.Items.TryGetValue(AuthTokenKey, out var tokens))
                return tokens as AuthTokenModel;

            return null;
        }
    }
}

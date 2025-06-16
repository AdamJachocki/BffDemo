using System.Net.Http.Headers;

namespace BFF.Extensions
{
    public static class HeaderExtensions
    {
        public static bool HasAuthToken(this HttpResponseHeaders headers)
        {
            return headers.Contains("MyApp-Has-Token");
        }
    }
}

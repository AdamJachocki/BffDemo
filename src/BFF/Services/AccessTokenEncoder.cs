using BFF.Abstractions;
using Microsoft.AspNetCore.DataProtection;
using System.Text.Json;

namespace BFF.Services
{
    internal class AccessTokenEncoder(IDataProtectionProvider dataProtectionProvider) : IAccessTokenEncoder
    {
        private const string Purpose = "AccessToken";

        public string EncodeTokens(AuthTokenModel data)
        {
            var protector = GetProtector();

            var strData = JsonSerializer.Serialize(data);

            return protector.Protect(strData);
        }

        public AuthTokenModel? DecodeTokens(string data)
        {
            var protector = GetProtector();
            var strData = protector.Unprotect(data);
            return JsonSerializer.Deserialize<AuthTokenModel>(strData);
        }

        private IDataProtector GetProtector()
        {
            return dataProtectionProvider.CreateProtector(Purpose);
        }
    }
}

namespace BFF.Abstractions
{
    public interface IAccessTokenEncoder
    {
        AuthTokenModel? DecodeTokens(string data);
        string EncodeTokens(AuthTokenModel data);
    }
}

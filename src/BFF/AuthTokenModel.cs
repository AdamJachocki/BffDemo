namespace BFF
{
    public class AuthTokenModel
    {
        public required string AccessToken { get; set; }
        public required DateTimeOffset ValidTo { get; set; }
    }
}

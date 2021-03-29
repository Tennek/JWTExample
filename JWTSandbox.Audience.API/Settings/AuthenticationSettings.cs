namespace JWTSandbox.Audience.API.Settings
{
    public class AuthenticationSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string AudienceSecret { get; set; }
        public string AudienceName { get; set; }
    }
}

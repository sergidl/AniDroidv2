using AniDroidv2.AniList.Interfaces;

namespace AniDroidv2.Utils.Integration
{
    public class AniDroidv2AniListAuthConfig : IAniListAuthConfig
    {
        public AniDroidv2AniListAuthConfig(string clientId, string clientSecret, string redirectUri, string authTokenUri)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            RedirectUri = redirectUri;
            AuthTokenUri = authTokenUri;
        }

        public string ClientId { get; }
        public string ClientSecret { get; }
        public string RedirectUri { get; }
        public string AuthTokenUri { get; }
    }
}
using AniDroidv2.AniList.Interfaces;

namespace AniDroidv2.Utils.Integration
{
    internal class AniDroidv2AniListServiceConfig : IAniListServiceConfig
    {
        public AniDroidv2AniListServiceConfig(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        public string BaseUrl { get; }
    }
}
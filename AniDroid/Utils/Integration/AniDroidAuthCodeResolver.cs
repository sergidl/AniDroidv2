using AniDroidv2.AniList.Interfaces;
using AniDroidv2.Utils.Interfaces;

namespace AniDroidv2.Utils.Integration
{
    internal class AniDroidv2AuthCodeResolver : IAuthCodeResolver
    {
        private readonly IAniDroidv2Settings _AniDroidv2Settings;

        public AniDroidv2AuthCodeResolver(IAniDroidv2Settings settings)
        {
            _AniDroidv2Settings = settings;
        }

        public string AuthCode => _AniDroidv2Settings.UserAccessCode;
        public bool IsAuthorized => _AniDroidv2Settings.IsUserAuthenticated;
        public void Invalidate() => _AniDroidv2Settings.ClearUserAuthentication();
    }
}
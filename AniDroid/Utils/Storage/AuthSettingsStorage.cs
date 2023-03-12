using Android.Content;

namespace AniDroidv2.Utils.Storage
{
    internal class AuthSettingsStorage : AniDroidv2Storage
    {
        protected override string Group => "AUTH_SETTINGS";

        public AuthSettingsStorage(Context c) : base(c)
        {
        }
    }
}
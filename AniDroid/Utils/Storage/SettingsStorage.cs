using Android.Content;

namespace AniDroidv2.Utils.Storage
{
    internal class SettingsStorage : AniDroidv2Storage
    {
        protected override string Group => "SETTINGS";

        public SettingsStorage(Context c) : base(c)
        {
        }
    }
}
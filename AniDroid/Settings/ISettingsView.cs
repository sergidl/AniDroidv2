using AniDroidv2.Adapters.Base;
using AniDroidv2.Base;
using AniDroidv2.Main;

namespace AniDroidv2.Settings
{
    public interface ISettingsView : IAniDroidv2View
    {
        void CreateCardTypeSettingItem(BaseRecyclerAdapter.RecyclerCardType cardType);
        void CreateAniDroidv2ThemeSettingItem(BaseAniDroidv2Activity.AniDroidv2Theme theme);
        void CreateDisplayBannersSettingItem(bool displayBanners);
        void CreateDisplayUpcomingEpisodeTimeAsCountdownItem(bool displayUpcomingEpisodeTimeAsCountdown);
        void CreateUseSwipeToRefreshHomeScreen(bool useSwipeToRefreshHomeScreen);
        void CreateWhatsNewSettingItem();
        void CreatePrivacyPolicyLinkItem();
        void CreateAboutSettingItem();

        // Auth settings
        void CreateMediaListSettingsItem();
        void CreateEnableNotificationServiceItem(bool enableNotificationService);
        void CreateDefaultTabItem(MainActivity.DefaultTab defaultTab);
    }
}
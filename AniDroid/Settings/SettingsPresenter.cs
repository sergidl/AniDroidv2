using System.Collections.Generic;
using System.Threading.Tasks;
using AniDroidv2.Adapters.Base;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.Base;
using AniDroidv2.Main;
using AniDroidv2.Utils.Interfaces;
using AniDroidv2.Utils.Logging;

namespace AniDroidv2.Settings
{
    public class SettingsPresenter : BaseAniDroidv2Presenter<ISettingsView>
    {
        public SettingsPresenter(IAniListService service, IAniDroidv2Settings settings,
            IAniDroidv2Logger logger) : base(service, settings, logger)
        {
        }

        public override Task Init()
        {
            View.CreateCardTypeSettingItem(AniDroidv2Settings.CardType);
            View.CreateAniDroidv2ThemeSettingItem(AniDroidv2Settings.Theme);
            View.CreateDisplayBannersSettingItem(AniDroidv2Settings.DisplayBanners);
            View.CreateDisplayUpcomingEpisodeTimeAsCountdownItem(AniDroidv2Settings
                .DisplayUpcomingEpisodeTimeAsCountdown);
            View.CreateUseSwipeToRefreshHomeScreen(AniDroidv2Settings.UseSwipeToRefreshHomeScreen);

            if (AniDroidv2Settings.IsUserAuthenticated)
            {
                View.CreateMediaListSettingsItem();
                View.CreateEnableNotificationServiceItem(AniDroidv2Settings.EnableNotificationService);
                View.CreateDefaultTabItem(AniDroidv2Settings.DefaultTab);
            }

            View.CreatePrivacyPolicyLinkItem();
            View.CreateWhatsNewSettingItem();
            View.CreateAboutSettingItem();

            return Task.CompletedTask;
        }

        public override async Task RestoreState(IList<string> savedState)
        {
            await Init();
        }

        public void SetCardType(BaseRecyclerAdapter.RecyclerCardType cardType)
        {
            AniDroidv2Settings.CardType = cardType;
        }

        public void SetTheme(BaseAniDroidv2Activity.AniDroidv2Theme theme)
        {
            AniDroidv2Settings.Theme = theme;
        }

        public void SetDisplayBanners(bool displayBanners)
        {
            AniDroidv2Settings.DisplayBanners = displayBanners;
        }

        public void SetDisplayUpcomingEpisodeTimeAsCountdown(bool displayUpcomingEpisodeTimeAsCountdown)
        {
            AniDroidv2Settings.DisplayUpcomingEpisodeTimeAsCountdown = displayUpcomingEpisodeTimeAsCountdown;
        }

        public void SetUseSwipeToRefreshHomeScreen(bool useSwipeToRefreshHomeScreen)
        {
            AniDroidv2Settings.UseSwipeToRefreshHomeScreen = useSwipeToRefreshHomeScreen;
        }

        // Auth Settings

        public void SetEnableNotificationService(bool enableNotificationService)
        {
            AniDroidv2Settings.EnableNotificationService = enableNotificationService;
        }

        public void SetDefaultTab(MainActivity.DefaultTab defaultTab)
        {
            AniDroidv2Settings.DefaultTab = defaultTab;
        }
    }
}
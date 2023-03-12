using AniDroidv2.Base;

namespace AniDroidv2.Main
{
    public interface IMainView : IAniDroidv2View
    {
        int GetVersionCode();
        void DisplayWhatsNewDialog();
        void SetAuthenticatedNavigationVisibility(bool isAuthenticated);
        void OnMainViewSetup();
        void SetNotificationCount(int count);
        void LogoutUser();
    }
}
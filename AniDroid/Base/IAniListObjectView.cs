namespace AniDroidv2.Base
{
    public interface IAniListObjectView : IAniDroidv2View
    {
        void Share();
        void SetLoadingShown();
        void SetContentShown(bool hasBanner);
        void SetErrorShown(string title, string message);
        void SetIsFavorite(bool isFavorite, bool showNotification = false);
        void SetShareText(string title, string uri);
        void SetupToolbar(string text, string bannerUri = null);
        void SetStandaloneActivity();
    }
}
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.ActivityModels;
using AniDroidv2.Base;
using AniDroidv2.Utils.Interfaces;
using AniDroidv2.Utils.Logging;
using Google.Android.Material.Snackbar;
using OneOf;

namespace AniDroidv2.Main
{
    public class MainPresenter : BaseAniDroidv2Presenter<IMainView>
    {
        public MainPresenter(IAniListService service, IAniDroidv2Settings settings,
            IAniDroidv2Logger logger) : base(service, settings, logger)
        {
        }

        public override async Task Init()
        {
            // TODO: potentially update notifications here, or trigger update at least

            View.SetAuthenticatedNavigationVisibility(AniDroidv2Settings.IsUserAuthenticated);
            View.OnMainViewSetup();

            if (View.GetVersionCode() > AniDroidv2Settings.HighestVersionUsed)
            {
                View.DisplayWhatsNewDialog();
                AniDroidv2Settings.HighestVersionUsed = View.GetVersionCode();
            }

            if ((AniDroidv2Settings.GenreCache?.Count ?? 0) == 0)
            {
                var genreResult = await AniListService.GetGenreCollectionAsync(default);

                genreResult.Switch(genres => AniDroidv2Settings.GenreCache = genres)
                    .Switch(error =>
                        View.DisplaySnackbarMessage("Error occurred while caching genres", Snackbar.LengthLong));
            }

            if ((AniDroidv2Settings.MediaTagCache?.Count ?? 0) == 0)
            {
                var genreResult = await AniListService.GetMediaTagCollectionAsync(default);

                genreResult.Switch(tags => AniDroidv2Settings.MediaTagCache = tags)
                    .Switch(error =>
                        View.DisplaySnackbarMessage("Error occurred while caching tags", Snackbar.LengthLong));
            }
        }

        public IAsyncEnumerable<OneOf<IPagedData<AniListNotification>, IAniListError>> GetNotificationsEnumerable()
        {
            return AniListService.GetAniListNotifications(true, 20);
        }

        public async Task GetUserNotificationCount()
        {
            var countResp = await AniListService.GetAniListNotificationCount(default);

            countResp.Switch(error => {
                    // we're going to force a log out if there was an unauthenticated error on this call
                    if (error.StatusCode == (int)HttpStatusCode.Unauthorized)
                    {
                        View.LogoutUser();
                    }

                })
                .Switch(user => View.SetNotificationCount(user.UnreadNotificationCount));
        }

        public bool GetIsUserAuthenticated()
        {
            return AniDroidv2Settings.IsUserAuthenticated;
        }

        public MainActivity.DefaultTab GetDefaultTab()
        {
            return AniDroidv2Settings.DefaultTab;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AniDroidv2.AniList.Dto;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.MediaModels;
using AniDroidv2.AniList.Models.UserModels;
using AniDroidv2.AniListObject.Media;
using AniDroidv2.Base;
using AniDroidv2.Utils.Interfaces;
using AniDroidv2.Utils.Logging;
using Google.Android.Material.Snackbar;

namespace AniDroidv2.Browse
{
    public class BrowsePresenter : BaseAniDroidv2Presenter<IBrowseView>, IAniListMediaListEditPresenter, IBrowsePresenter
    {
        private BrowseMediaDto _browseDto;

        public BrowsePresenter(IAniListService service, IAniDroidv2Settings settings,
            IAniDroidv2Logger logger) : base(service, settings, logger)
        {
        }

        public void BrowseAniListMedia(BrowseMediaDto browseDto)
        {
            _browseDto = browseDto;
            View.ShowMediaSearchResults(AniListService.BrowseMedia(browseDto, 20));
        }

        public override Task Init()
        {
            // TODO: does this need anything?
            return Task.CompletedTask;
        }

        public bool GetIsUserAuthenticated()
        {
            return AniDroidv2Settings.IsUserAuthenticated;
        }

        public User GetAuthenticatedUser()
        {
            return AniDroidv2Settings.LoggedInUser;
        }

        public async Task SaveMediaListEntry(MediaListEditDto editDto, Action onSuccess, Action onError)
        {
            var mediaUpdateResp = await AniListService.UpdateMediaListEntry(editDto, default);

            mediaUpdateResp.Switch(mediaList =>
            {
                onSuccess();
                View.DisplaySnackbarMessage("Saved", Snackbar.LengthShort);
                View.UpdateMediaListItem(mediaList);
            }).Switch(error => onError());
        }

        public async Task DeleteMediaListEntry(int mediaListId, Action onSuccess, Action onError)
        {
            var mediaDeleteResp = await AniListService.DeleteMediaListEntry(mediaListId, default);

            mediaDeleteResp.Switch((bool success) =>
            {
                onSuccess();
                View.DisplaySnackbarMessage("Deleted", Snackbar.LengthShort);
                View.RemoveMediaListItem(mediaListId);
            }).Switch(error =>
                onError());
        }

        public BrowseMediaDto GetBrowseDto()
        {
            return _browseDto;
        }

        public IList<MediaTag> GetMediaTags()
        {
            return AniDroidv2Settings.MediaTagCache;
        }

        public IList<string> GetGenres()
        {
            return AniDroidv2Settings.GenreCache;
        }
    }
}
using System;
using System.Collections.Generic;
using AniDroidv2.AniList.Dto;
using AniDroidv2.AniList.Enums.MediaEnums;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.UserModels;
using AniDroidv2.AniListObject.Media;
using AniDroidv2.Base;
using AniDroidv2.Utils.Interfaces;
using AniDroidv2.Utils.Logging;
using Google.Android.Material.Snackbar;
using Task = System.Threading.Tasks.Task;

namespace AniDroidv2.Discover
{
    public class DiscoverPresenter : BaseAniDroidv2Presenter<IDiscoverView>, IAniListMediaListEditPresenter
    {
        public DiscoverPresenter(IAniListService service, IAniDroidv2Settings settings,
            IAniDroidv2Logger logger) : base(service, settings, logger)
        {
        }

        public override Task Init()
        {
            return Task.CompletedTask;
        }

        public void GetDiscoverLists()
        {
            View.ShowCurrentlyAiringResults(AniListService.BrowseMedia(new BrowseMediaDto
            {
                Season = MediaSeason.GetFromDate(DateTime.Now),
                SeasonYear = DateTime.Now.Year,
                Type = MediaType.Anime,
                Sort = new List<MediaSort> { MediaSort.PopularityDesc }
            }, 5));
            View.ShowTrendingAnimeResults(AniListService.BrowseMedia(new BrowseMediaDto
            {
                Type = MediaType.Anime,
                Sort = new List<MediaSort> {MediaSort.TrendingDesc}
            }, 5));
            View.ShowTrendingMangaResults(AniListService.BrowseMedia(new BrowseMediaDto
            {
                Type = MediaType.Manga,
                Sort = new List<MediaSort> { MediaSort.TrendingDesc }
            }, 5));
            View.ShowNewAnimeResults(AniListService.BrowseMedia(
                new BrowseMediaDto
                {
                    Type = MediaType.Anime,
                    Sort = new List<MediaSort> { MediaSort.IdDesc }
                }, 5));
            View.ShowNewMangaResults(AniListService.BrowseMedia(
                new BrowseMediaDto
                {
                    Type = MediaType.Manga,
                    Sort = new List<MediaSort> { MediaSort.IdDesc }
                }, 5));
        }

        public bool GetIsUserAuthenticated()
        {
            return AniDroidv2Settings.IsUserAuthenticated;
        }

        public User GetLoggedInUser()
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
    }
}
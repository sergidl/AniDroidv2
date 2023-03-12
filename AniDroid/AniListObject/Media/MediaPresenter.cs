using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AniDroidv2.AniList.Dto;
using AniDroidv2.AniList.Enums.MediaEnums;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models;
using AniDroidv2.AniList.Models.CharacterModels;
using AniDroidv2.AniList.Models.ForumModels;
using AniDroidv2.AniList.Models.MediaModels;
using AniDroidv2.AniList.Models.RecommendationModels;
using AniDroidv2.AniList.Models.ReviewModels;
using AniDroidv2.AniList.Models.StaffModels;
using AniDroidv2.Base;
using AniDroidv2.Utils.Interfaces;
using AniDroidv2.Utils.Logging;
using Google.Android.Material.Snackbar;
using OneOf;

namespace AniDroidv2.AniListObject.Media
{
    public class MediaPresenter : BaseAniDroidv2Presenter<IMediaView>, IAniListMediaListEditPresenter
    {
        public MediaPresenter(IAniListService service, IAniDroidv2Settings settings,
            IAniDroidv2Logger logger) : base(service, settings, logger)
        {
        }

        public override async Task Init()
        {
            View.SetLoadingShown();
            var mediaId = View.GetMediaId();
            var mediaResp = AniListService.GetMediaById(mediaId, default);
            var userResp = AniListService.GetCurrentUser(default);

            if (AniDroidv2Settings.IsUserAuthenticated)
            {
                View.SetCanEditListItem();
            }

            await Task.WhenAll(mediaResp, userResp);

            userResp.Result.Switch(user => View.SetCurrentUserMediaListOptions(user.MediaListOptions))
                .Switch(error => {
                    if (AniDroidv2Settings.IsUserAuthenticated)
                    {
                        View.DisplaySnackbarMessage("Error occurred while getting user settings", Snackbar.LengthLong);
                    }
                });

            mediaResp.Result.Switch(media =>
                {
                    FixMediaData(media);

                    View.SetIsFavorite(media.IsFavourite);
                    View.SetShareText(media.Title?.UserPreferred, media.SiteUrl);
                    View.SetContentShown(!string.IsNullOrWhiteSpace(media.BannerImage));
                    View.SetupToolbar(media.Title?.UserPreferred, media.BannerImage);
                    View.SetupMediaView(media);
                })
                .Switch(error => View.OnError(error));
        }

        public IAsyncEnumerable<OneOf<IPagedData<CharacterEdge>, IAniListError>> GetMediaCharactersEnumerable(int mediaId, int perPage)
        {
            return AniListService.GetMediaCharacters(mediaId, perPage);
        }

        public IAsyncEnumerable<OneOf<IPagedData<StaffEdge>, IAniListError>> GetMediaStaffEnumerable(int mediaId, int perPage)
        {
            return AniListService.GetMediaStaff(mediaId, perPage);
        }

        public IAsyncEnumerable<OneOf<IPagedData<AniList.Models.MediaModels.MediaList>, IAniListError>>
            GetMediaFollowingUsersMediaListsEnumerable(int mediaId, int perPage)
        {
            return AniListService.GetMediaFollowingUsersMediaLists(mediaId, perPage);
        }

        public IAsyncEnumerable<OneOf<IPagedData<Review>, IAniListError>> GetMediaReviewsEnumerable(int mediaId,
            int perPage)
        {
            return AniListService.GetMediaReviews(mediaId, perPage);
        }

        public IAsyncEnumerable<OneOf<IPagedData<ForumThread>, IAniListError>> GetMediaForumThreadsEnumerable(int mediaId,
            int perPage)
        {
            return AniListService.GetMediaForumThreads(mediaId, perPage);
        }

        public IAsyncEnumerable<OneOf<IPagedData<ConnectionEdge<Recommendation>>, IAniListError>>
            GetMediaRecommendationsEnumerable(int mediaId, int perPage)
        {
            return AniListService.GetMediaRecommendations(mediaId, perPage);
        }

        public IAsyncEnumerable<OneOf<IPagedData<ConnectionEdge<MediaTrend>>, IAniListError>> GetMediaTrendsEnumerable(
            int mediaId)
        {
            return AniListService.GetMediaTrends(mediaId, true, new[]
            {
                MediaTrendSort.DateDesc
            }, 25);
        }

        public async Task ToggleFavorite()
        {
            var mediaId = View.GetMediaId();
            var mediaType = View.GetMediaType();

            var favDto = new FavoriteDto();

            if (mediaType == MediaType.Anime)
            {
                favDto.AnimeId = mediaId;
            }
            else
            {
                favDto.MangaId = mediaId;
            }

            var favResp = await AniListService.ToggleFavorite(favDto,
                default(CancellationToken));

            favResp.Switch(error => View.OnError(error))
                .Switch(favorites =>
                    View.SetIsFavorite(
                        (mediaType == MediaType.Anime
                            ? favorites.Anime?.Nodes?.Any(x => x.Id == mediaId)
                            : favorites.Manga?.Nodes?.Any(x => x.Id == mediaId)) == true, true));
        }

        public async Task SaveMediaListEntry(MediaListEditDto editDto, Action onSuccess, Action onError)
        {
            var mediaUpdateResp = await AniListService.UpdateMediaListEntry(editDto, default(CancellationToken));

            mediaUpdateResp.Switch(mediaList =>
                {
                    onSuccess();
                    View.DisplaySnackbarMessage("Saved", Snackbar.LengthShort);
                    View.UpdateMediaListItem(mediaList);
                })
                .Switch(error =>
                {
                    onError();
                });
        }

        public async Task DeleteMediaListEntry(int mediaListId, Action onSuccess, Action onError)
        {
            var mediaDeleteResp = await AniListService.DeleteMediaListEntry(mediaListId, default(CancellationToken));

            mediaDeleteResp.Switch((bool success) =>
            {
                onSuccess();
                View.DisplaySnackbarMessage("Deleted", Snackbar.LengthShort);
                View.RemoveMediaListItem();
            }).Switch(error => onError());
        }

        private void FixMediaData(AniList.Models.MediaModels.Media media)
        {
            // small fixes and sorts for what media data we can fix

            if (media.Tags?.Any() == true)
            {
                media.Tags = media.Tags.OrderByDescending(x => x.Rank).ToList();
            }
        }
    }
}
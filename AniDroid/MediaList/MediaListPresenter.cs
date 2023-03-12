using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AniDroidv2.Adapters.Base;
using AniDroidv2.Adapters.MediaAdapters;
using AniDroidv2.AniList.Dto;
using AniDroidv2.AniList.Enums.MediaEnums;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.MediaModels;
using AniDroidv2.AniListObject.Media;
using AniDroidv2.Base;
using AniDroidv2.Utils.Comparers;
using AniDroidv2.Utils.Interfaces;
using AniDroidv2.Utils.Logging;
using Google.Android.Material.Snackbar;

namespace AniDroidv2.MediaList
{
    public class MediaListPresenter : BaseAniDroidv2Presenter<IMediaListView>, IAniListMediaListEditPresenter
    {
        public MediaListPresenter(IAniListService service, IAniDroidv2Settings settings,
            IAniDroidv2Logger logger) : base(service, settings, logger)
        {
        }

        public override Task Init()
        {
            return Task.CompletedTask;
        }

        public async Task GetMediaLists(int userId)
        {
            var mediaListResp = await AniListService.GetUserMediaList(userId,
                View.GetMediaType(), AniDroidv2Settings.GroupCompletedLists, default);


            mediaListResp.Switch(error => View.OnError(error))
                .Switch(mediaLists =>
                {
                    if (userId == AniDroidv2Settings.LoggedInUser?.Id)
                    {
                        AniDroidv2Settings.UpdateLoggedInUser(mediaLists.User);
                    }

                    View.SetCollection(mediaLists);
                });
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

        public async Task IncreaseMediaProgress(AniList.Models.MediaModels.MediaList mediaListToUpdate)
        {
            var editDto = new MediaListEditDto
            {
                MediaId = mediaListToUpdate.Media.Id,
                Progress = (mediaListToUpdate.Progress ?? 0) + 1
            };

            var mediaUpdateResp = await AniListService.UpdateMediaListEntry(editDto, default);

            mediaUpdateResp.Switch(mediaList =>
                {
                    View.DisplaySnackbarMessage($"Updated progress for {mediaList.Media.Title.UserPreferred}", Snackbar.LengthShort);
                    View.UpdateMediaListItem(mediaList);
                })
                .Switch(error =>
                {
                    View.DisplaySnackbarMessage("Error occurred while saving list entry", Snackbar.LengthLong);
                    View.ResetMediaListItem(mediaListToUpdate.Media.Id);
                });
        }

        public async Task CompleteMedia(AniList.Models.MediaModels.MediaList mediaListToComplete)
        {
            var editDto = new MediaListEditDto
            {
                MediaId = mediaListToComplete.Media.Id,
                Progress = mediaListToComplete.Media.Episodes,
                Status = MediaListStatus.Completed
            };

            var mediaUpdateResp = await AniListService.UpdateMediaListEntry(editDto, default);

            mediaUpdateResp.Switch(mediaList =>
                {
                    View.DisplaySnackbarMessage($"Completed {mediaList.Media.Title.UserPreferred}", Snackbar.LengthShort);
                    View.UpdateMediaListItem(mediaList);
                })
                .Switch(error =>
                {
                    View.DisplaySnackbarMessage("Error occurred while saving list entry", Snackbar.LengthLong);
                    View.ResetMediaListItem(mediaListToComplete.Media.Id);
                });
        }

        public BaseRecyclerAdapter.RecyclerCardType GetCardType()
        {
            return AniDroidv2Settings.CardType;
        }

        public MediaListRecyclerAdapter.MediaListItemViewType GetMediaListItemViewType()
        {
            return AniDroidv2Settings.MediaViewType;
        }

        public bool GetHighlightPriorityItems()
        {
            return AniDroidv2Settings.HighlightPriorityMediaListItems;
        }

        public MediaListRecyclerAdapter.MediaListProgressDisplayType GetProgressDisplayType()
        {
            return AniDroidv2Settings.MediaListProgressDisplay;
        }

        public bool GetUseLongClickForEpisodeAdd()
        {
            return AniDroidv2Settings.UseLongClickForEpisodeAdd;
        }

        public bool GetDisplayTimeUntilAiringAsCountdown()
        {
            return AniDroidv2Settings.DisplayUpcomingEpisodeTimeAsCountdown;
        }

        public bool GetUseSwipeToRefreshOnMediaLists()
        {
            return AniDroidv2Settings.UseSwipeToRefreshOnMediaLists;
        }

        public bool GetShowEpisodeAddButtonForRepeatingMedia()
        {
            return AniDroidv2Settings.ShowEpisodeAddButtonForRepeatingMedia;
        }

        public MediaListSortComparer.SortDirection GetMediaListSortDirection(MediaType mediaType)
        {
            if (MediaType.Anime.Equals(mediaType))
            {
                return AniDroidv2Settings.AnimeListSortDirection;
            }
            else if (MediaType.Manga.Equals(mediaType))
            {
                return AniDroidv2Settings.MangaListSortDirection;
            }

            return MediaListSortComparer.SortDirection.Ascending;
        }

        public MediaListSortComparer.MediaListSortType GetMediaListSortType(MediaType mediaType)
        {
            if (MediaType.Anime.Equals(mediaType))
            {
                return AniDroidv2Settings.AnimeListSortType;
            }

            if (MediaType.Manga.Equals(mediaType))
            {
                return AniDroidv2Settings.MangaListSortType;
            }

            return MediaListSortComparer.MediaListSortType.NoSort;
        }

        public void SetMediaListSortSettings(MediaType mediaType, MediaListSortComparer.MediaListSortType sort,
            MediaListSortComparer.SortDirection direction)
        {
            if (MediaType.Anime.Equals(mediaType))
            {
                AniDroidv2Settings.AnimeListSortType = sort;
                AniDroidv2Settings.AnimeListSortDirection = direction;
            }
            else if (MediaType.Manga.Equals(mediaType))
            {
                AniDroidv2Settings.MangaListSortType = sort;
                AniDroidv2Settings.MangaListSortDirection = direction;
            }
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
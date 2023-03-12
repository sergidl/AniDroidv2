using System;
using System.Threading;
using System.Threading.Tasks;
using AniDroidv2.AniList.Dto;
using AniDroidv2.AniList.Enums.MediaEnums;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniListObject.Media;
using AniDroidv2.Base;
using AniDroidv2.Utils.Interfaces;
using AniDroidv2.Utils.Logging;
using Google.Android.Material.Snackbar;

namespace AniDroidv2.SearchResults
{
    public class SearchResultsPresenter : BaseAniDroidv2Presenter<ISearchResultsView>, IAniListMediaListEditPresenter
    {
        private const int PageSize = 20;

        public SearchResultsPresenter(IAniListService service, IAniDroidv2Settings settings,
            IAniDroidv2Logger logger) : base(service, settings, logger)
        {
        }

        public void SearchAniList(string searchType, string searchTerm)
        {
            switch (searchType)
            {
                case SearchResultsActivity.AniListSearchTypes.Anime:
                    View.ShowMediaSearchResults(AniListService.SearchMedia(searchTerm, MediaType.Anime,
                        PageSize));
                    break;
                case SearchResultsActivity.AniListSearchTypes.Manga:
                    View.ShowMediaSearchResults(AniListService.SearchMedia(searchTerm, MediaType.Manga,
                        PageSize));
                    break;
                case SearchResultsActivity.AniListSearchTypes.Characters:
                    View.ShowCharacterSearchResults(AniListService.SearchCharacters(searchTerm, PageSize));
                    break;
                case SearchResultsActivity.AniListSearchTypes.Staff:
                    View.ShowStaffSearchResults(AniListService.SearchStaff(searchTerm, PageSize));
                    break;
                case SearchResultsActivity.AniListSearchTypes.Studios:
                    View.ShowStudioSearchResults(AniListService.SearchStudios(searchTerm, PageSize));
                    break;
                case SearchResultsActivity.AniListSearchTypes.Users:
                    View.ShowUserSearchResults(AniListService.SearchUsers(searchTerm, PageSize));
                    break;
                case SearchResultsActivity.AniListSearchTypes.Forum:
                    View.ShowForumThreadSearchResults(AniListService.SearchForumThreads(searchTerm, PageSize));
                    break;
            }
        }

        public override Task Init()
        {
            // TODO: determine if these are needed for this presenter
            return Task.CompletedTask;
        }

        public async Task SaveMediaListEntry(MediaListEditDto editDto, Action onSuccess, Action onError)
        {
            var mediaUpdateResp = await AniListService.UpdateMediaListEntry(editDto, default(CancellationToken));

            mediaUpdateResp.Switch(mediaList =>
            {
                onSuccess();
                View.DisplaySnackbarMessage("Saved", Snackbar.LengthShort);
                View.UpdateMediaListItem(mediaList);
            }).Switch(error => onError());
        }

        public async Task DeleteMediaListEntry(int mediaListId, Action onSuccess, Action onError)
        {
            var mediaDeleteResp = await AniListService.DeleteMediaListEntry(mediaListId, default(CancellationToken));

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
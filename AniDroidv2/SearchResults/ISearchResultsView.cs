using System.Collections.Generic;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.CharacterModels;
using AniDroidv2.AniList.Models.ForumModels;
using AniDroidv2.AniList.Models.MediaModels;
using AniDroidv2.AniList.Models.StaffModels;
using AniDroidv2.AniList.Models.StudioModels;
using AniDroidv2.AniList.Models.UserModels;
using AniDroidv2.Base;
using OneOf;

namespace AniDroidv2.SearchResults
{
    public interface ISearchResultsView : IAniDroidv2View
    {
        void ShowMediaSearchResults(IAsyncEnumerable<OneOf<IPagedData<Media>, IAniListError>> mediaEnumerable);
        void ShowCharacterSearchResults(IAsyncEnumerable<OneOf<IPagedData<Character>, IAniListError>> characterEnumerable);
        void ShowStaffSearchResults(IAsyncEnumerable<OneOf<IPagedData<Staff>, IAniListError>> staffEnumerable);
        void ShowUserSearchResults(IAsyncEnumerable<OneOf<IPagedData<User>, IAniListError>> userEnumerable);
        void ShowForumThreadSearchResults(IAsyncEnumerable<OneOf<IPagedData<ForumThread>, IAniListError>> forumThreadEnumerable);
        void ShowStudioSearchResults(IAsyncEnumerable<OneOf<IPagedData<Studio>, IAniListError>> studioEnumerable);
        void UpdateMediaListItem(AniList.Models.MediaModels.MediaList mediaList);
        void RemoveMediaListItem(int mediaListId);
    }
}
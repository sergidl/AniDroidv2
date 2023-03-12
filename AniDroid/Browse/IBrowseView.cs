using System.Collections.Generic;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.MediaModels;
using AniDroidv2.Base;
using OneOf;

namespace AniDroidv2.Browse
{
    public interface IBrowseView : IAniDroidv2View
    {
        void ShowMediaSearchResults(IAsyncEnumerable<OneOf<IPagedData<Media>, IAniListError>> mediaEnumerable);
        void UpdateMediaListItem(AniList.Models.MediaModels.MediaList mediaList);
        void RemoveMediaListItem(int mediaListId);
    }
}
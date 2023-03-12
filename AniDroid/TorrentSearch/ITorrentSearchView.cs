using System.Collections.Generic;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.Base;
using AniDroidv2.Torrent.NyaaSi;
using OneOf;

namespace AniDroidv2.TorrentSearch
{
    public interface ITorrentSearchView : IAniDroidv2View
    {
        void ShowNyaaSiSearchResults(
            IAsyncEnumerable<OneOf<IPagedData<NyaaSiSearchResult>, IAniListError>> searchEnumerable);
    }
}
using System.Collections.Generic;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.MediaModels;
using AniDroidv2.Base;
using OneOf;

namespace AniDroidv2.Discover
{
    public interface IDiscoverView : IAniDroidv2View
    {
        void ShowCurrentlyAiringResults(IAsyncEnumerable<OneOf<IPagedData<Media>, IAniListError>> mediaEnumerable);
        void ShowTrendingAnimeResults(IAsyncEnumerable<OneOf<IPagedData<Media>, IAniListError>> mediaEnumerable);
        void ShowTrendingMangaResults(IAsyncEnumerable<OneOf<IPagedData<Media>, IAniListError>> mediaEnumerable);
        void ShowNewAnimeResults(IAsyncEnumerable<OneOf<IPagedData<Media>, IAniListError>> mediaEnumerable);
        void ShowNewMangaResults(IAsyncEnumerable<OneOf<IPagedData<Media>, IAniListError>> mediaEnumerable);
        void UpdateMediaListItem(AniList.Models.MediaModels.MediaList mediaList);
        void RemoveMediaListItem(int mediaListId);
    }
}
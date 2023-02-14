using System.Collections.Generic;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.MediaModels;
using AniDroidv2.Base;
using OneOf;

namespace AniDroidv2.CurrentSeason
{
    public interface ICurrentSeasonView : IAniDroidv2View
    {
        void ShowCurrentTv(IAsyncEnumerable<OneOf<IPagedData<Media>, IAniListError>> mediaEnumerable);
        void ShowCurrentMovies(IAsyncEnumerable<OneOf<IPagedData<Media>, IAniListError>> mediaEnumerable);
        void ShowCurrentOvaOna(IAsyncEnumerable<OneOf<IPagedData<Media>, IAniListError>> mediaEnumerable);
        void ShowCurrentLeftovers(IAsyncEnumerable<OneOf<IPagedData<Media>, IAniListError>> mediaEnumerable);
    }
}
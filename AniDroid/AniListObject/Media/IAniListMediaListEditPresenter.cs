using System;
using System.Threading.Tasks;
using AniDroidv2.AniList.Dto;

namespace AniDroidv2.AniListObject.Media
{
    public interface IAniListMediaListEditPresenter
    {
        Task SaveMediaListEntry(MediaListEditDto editDto, Action onSuccess, Action onError);
        Task DeleteMediaListEntry(int mediaListId, Action onSuccess, Action onError);
    }
}
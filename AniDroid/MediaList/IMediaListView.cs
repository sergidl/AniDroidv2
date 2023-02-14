using AniDroidv2.AniList.Enums.MediaEnums;
using AniDroidv2.AniList.Models.MediaModels;
using AniDroidv2.Base;

namespace AniDroidv2.MediaList
{
    public interface IMediaListView : IAniDroidv2View
    {
        MediaType GetMediaType();
        void SetCollection(MediaListCollection collection);
        void UpdateMediaListItem(AniList.Models.MediaModels.MediaList mediaList);
        void ResetMediaListItem(int mediaId);
        void RemoveMediaListItem(int mediaListId);
        MediaListFilterModel GetMediaListFilter();
        void SetMediaListFilter(MediaListFilterModel filterModel);
    }
}
using AniDroidv2.AniList.Enums.MediaEnums;
using AniDroidv2.AniList.Models.UserModels;
using AniDroidv2.Base;

namespace AniDroidv2.AniListObject.Media
{
    public interface IMediaView : IAniListObjectView
    {
        int GetMediaId();
        MediaType GetMediaType();
        void SetCanEditListItem();
        void SetupMediaView(AniList.Models.MediaModels.Media media);
        void SetCurrentUserMediaListOptions(UserMediaListOptions mediaListOptions);
        void ShowMediaListEditDialog(AniList.Models.MediaModels.MediaList mediaList);
        void UpdateMediaListItem(AniList.Models.MediaModels.MediaList mediaList);
        void RemoveMediaListItem();
    }
}
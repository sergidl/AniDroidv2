using AniDroidv2.Base;

namespace AniDroidv2.AniListObject.Studio
{
    public interface IStudioView : IAniListObjectView
    {
        int GetStudioId();
        void SetupStudioView(AniList.Models.StudioModels.Studio studio);
    }
}
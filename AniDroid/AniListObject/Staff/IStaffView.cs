using AniDroidv2.Base;

namespace AniDroidv2.AniListObject.Staff
{
    public interface IStaffView : IAniListObjectView
    {
        int GetStaffId();
        void SetupStaffView(AniList.Models.StaffModels.Staff staff);
    }
}
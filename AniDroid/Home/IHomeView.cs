using System.Collections.Generic;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.ActivityModels;
using AniDroidv2.Base;
using OneOf;

namespace AniDroidv2.Home
{
    public interface IHomeView : IAniDroidv2View
    {
        void ShowUserActivity(IAsyncEnumerable<OneOf<IPagedData<AniListActivity>, IAniListError>> activityEnumerable, int userId);
        void ShowAllActivity(IAsyncEnumerable<OneOf<IPagedData<AniListActivity>, IAniListError>> activityEnumerable, int userId);
        void UpdateActivity(int activityPosition, AniListActivity activity);
        void RemoveActivity(int activityPosition);
        void RefreshActivity();
    }
}
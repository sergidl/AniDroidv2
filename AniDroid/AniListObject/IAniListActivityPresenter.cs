﻿using System.Threading.Tasks;
using AniDroidv2.AniList.Models.ActivityModels;

namespace AniDroidv2.AniListObject
{
    public interface IAniListActivityPresenter
    {
        Task ToggleActivityLikeAsync(AniListActivity activity, int activityPosition);
        Task PostActivityReplyAsync(AniListActivity activity, int activityPosition, string activityText);
        Task EditStatusActivityAsync(AniListActivity activity, int activityPosition, string updateText);
        Task DeleteActivityAsync(int activityId, int activityPosition);
        Task ToggleActivityReplyLikeAsync(ActivityReply activityReply, int activityPosition);
        Task EditActivityReplyAsync(ActivityReply activityReply, int activityPosition,
            string updateText);
        Task<bool> DeleteActivityReplyAsync(ActivityReply activityReply, int activityPosition);
        Task UpdateActivityAsync(AniListActivity activity, int activityPosition);
    }
}
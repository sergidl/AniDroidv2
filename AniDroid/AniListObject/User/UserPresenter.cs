﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AniDroidv2.AniList.Dto;
using AniDroidv2.AniList.Enums;
using AniDroidv2.AniList.Enums.UserEnums;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.ActivityModels;
using AniDroidv2.AniList.Models.ReviewModels;
using AniDroidv2.Base;
using AniDroidv2.Utils.Interfaces;
using AniDroidv2.Utils.Logging;
using Google.Android.Material.Snackbar;
using OneOf;

namespace AniDroidv2.AniListObject.User
{
    public class UserPresenter : BaseAniDroidv2Presenter<IUserView>, IAniListActivityPresenter
    {
        public UserPresenter(IAniListService service, IAniDroidv2Settings settings,
            IAniDroidv2Logger logger) : base(service, settings, logger)
        {
        }

        public override async Task Init()
        {
            View.SetLoadingShown();
            var userId = View.GetUserId();
            var userName = View.GetUserName();

            Logger.Debug("UserPresenter", $"Init started (userId: {userId}, userName: {userName})");

            var userResp = await AniListService.GetUser(userName, userId, default(CancellationToken));

            userResp.Switch(user =>
                {
                    if (AniDroidv2Settings.IsUserAuthenticated && user.Id != AniDroidv2Settings.LoggedInUser.Id)
                    {
                        View.SetCanFollow();
                        View.SetCanMessage();
                        View.SetIsFollowing(user.IsFollowing, false);
                    }

                    View.SetShareText(user.Name, user.SiteUrl);
                    View.SetContentShown(!string.IsNullOrWhiteSpace(user.BannerImage));
                    View.SetupToolbar(user.Name, user.BannerImage);
                    View.SetupUserView(user);
                })
                .Switch(error => View.OnError(error));
        }

        public int? GetCurrentUserId()
        {
            return AniDroidv2Settings.LoggedInUser?.Id;
        }

        public IAsyncEnumerable<OneOf<IPagedData<AniListActivity>, IAniListError>> GetUserActivityEnumerable(int userId, int count)
        {
            return AniListService.GetAniListActivity(new AniListActivityDto {UserId = userId}, count);
        }

        public IAsyncEnumerable<OneOf<IPagedData<AniList.Models.UserModels.User>, IAniListError>> GetUserFollowersEnumerable(int userId, int count)
        {
            return AniListService.GetUserFollowers(userId, UserSort.Username, count);
        }

        public IAsyncEnumerable<OneOf<IPagedData<AniList.Models.UserModels.User>, IAniListError>> GetUserFollowingEnumerable(int userId, int count)
        {
            return AniListService.GetUserFollowing(userId, UserSort.Username, count);
        }

        public IAsyncEnumerable<OneOf<IPagedData<Review>, IAniListError>> GetUserReviewsEnumerable(int userId,
            int perPage)
        {
            return AniListService.GetUserReviews(userId, perPage);
        }

        public async Task ToggleFollowUser(int userId)
        {
            var toggleResp = await AniListService.ToggleFollowUser(userId, default(CancellationToken));

            toggleResp.Switch((IAniListError error) =>
                    View.DisplaySnackbarMessage("Error occurred while trying to toggle following status",
                        Snackbar.LengthLong))
                .Switch(user => View.SetIsFollowing(user.IsFollowing, true));
        }

        public async Task PostUserMessage(int userId, string message)
        {
            var postResp = await AniListService.PostUserMessage(userId, message, default(CancellationToken));

            postResp.Switch((IAniListError error) =>
                    View.DisplaySnackbarMessage("Error occurred while posting message", Snackbar.LengthLong))
                .Switch(activity =>
                {
                    View.RefreshUserActivity();
                    View.DisplaySnackbarMessage("Message posted successfully", Snackbar.LengthShort);
                });
        }

        public async Task ToggleActivityLikeAsync(AniListActivity activity, int activityPosition)
        {
            var toggleResp = await AniListService.ToggleLike(activity.Id,
                LikeableType.Activity, default(CancellationToken));

            toggleResp.Switch((IAniListError error) =>
                {
                    View.UpdateActivity(activityPosition, activity);
                    View.DisplaySnackbarMessage("Error occurred while toggling like", Snackbar.LengthLong);
                })
                .Switch(userLikes =>
                {
                    activity.Likes = userLikes;
                    View.UpdateActivity(activityPosition, activity);
                });
        }

        public async Task PostActivityReplyAsync(AniListActivity activity, int activityPosition, string text)
        {
            var postResp = await AniListService.PostActivityReply(activity.Id, text, default(CancellationToken));

            postResp.Switch((IAniListError error) =>
                {
                    View.UpdateActivity(activityPosition, activity);
                    View.DisplaySnackbarMessage("Error occurred while posting reply", Snackbar.LengthLong);
                })
                .Switch(async reply =>
                {
                    var refreshResp =
                        await AniListService.GetAniListActivityById(activity.Id, default(CancellationToken));

                    refreshResp.Switch((IAniListError error) =>
                        {
                            View.UpdateActivity(activityPosition, activity);
                            View.DisplaySnackbarMessage("Error occurred while refreshing activity",
                                Snackbar.LengthLong);
                        })
                        .Switch(activityResp =>
                        {
                            View.UpdateActivity(activityPosition, activityResp);
                            View.DisplaySnackbarMessage("Reply posted successfully", Snackbar.LengthShort);
                        });
                });
        }

        public async Task EditStatusActivityAsync(AniListActivity activity, int activityPosition, string updateText)
        {
            var postResp = await AniListService.SaveTextActivity(updateText, activity.Id, default);

            postResp.Switch((IAniListError error) => View.DisplaySnackbarMessage("Error occurred while saving status", Snackbar.LengthLong))
                .Switch(updatedAct => View.UpdateActivity(activityPosition, updatedAct));
        }

        public async Task DeleteActivityAsync(int activityId, int activityPosition)
        {
            var deleteResp = await AniListService.DeleteActivity(activityId, default);

            deleteResp.Switch((IAniListError error) => View.DisplaySnackbarMessage("Error occurred while deleting activity", Snackbar.LengthLong))
                .Switch(deleted => {
                    if (deleted?.Deleted == true)
                    {
                        View.RemoveActivity(activityPosition);
                    }
                    else
                    {
                        View.DisplaySnackbarMessage("Error occurred while deleting activity", Snackbar.LengthLong);
                    }
                });
        }

        public async Task ToggleActivityReplyLikeAsync(ActivityReply activityReply, int activityPosition)
        {
            var toggleResp = await AniListService.ToggleLike(activityReply.Id, LikeableType.ActivityReply, default);

            toggleResp.Switch((IAniListError error) =>
                {
                    View.DisplaySnackbarMessage("Error occurred while toggling like", Snackbar.LengthLong);
                })
                .Switch(userLikes =>
                {
                    activityReply.Likes = userLikes;
                });
        }

        public async Task EditActivityReplyAsync(ActivityReply activityReply, int activityPosition, string updateText)
        {
            var editResp = await AniListService.SaveActivityReply(activityReply.Id, updateText, default);

            editResp.Switch((IAniListError error) =>
                {
                    View.DisplaySnackbarMessage("Error occurred while updating reply", Snackbar.LengthLong);
                })
                .Switch(retReply =>
                {
                    activityReply.Text = retReply.Text;
                    activityReply.Likes = retReply.Likes;
                });
        }

        public async Task<bool> DeleteActivityReplyAsync(ActivityReply activityReply,
            int activityPosition)
        {
            var editResp = await AniListService.DeleteActivityReply(activityReply.Id, default);

            return editResp.Match((IAniListError error) =>
                {
                    View.DisplaySnackbarMessage("Error occurred while deleting reply", Snackbar.LengthLong);

                    return false;
                })
                .Match(deletedResponse => deletedResponse.Deleted);
        }

        public async Task UpdateActivityAsync(AniListActivity activity, int activityPosition)
        {
            var activityResp = await AniListService.GetAniListActivityById(activity.Id, default);

            activityResp.Switch((IAniListError error) => View.DisplaySnackbarMessage("Error occurred while refreshing activity", Snackbar.LengthLong))
                .Switch(updatedAct => View.UpdateActivity(activityPosition, updatedAct));
        }
    }
}
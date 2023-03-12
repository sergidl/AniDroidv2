using System;
using System.Collections.Generic;
using Android.Views;
using AniDroidv2.Adapters.Base;
using AniDroidv2.Adapters.ViewModels;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.UserModels;
using AniDroidv2.AniListObject.User;
using AniDroidv2.Base;
using OneOf;

namespace AniDroidv2.Adapters.UserAdapters
{
    public class UserRecyclerAdapter : AniDroidv2RecyclerAdapter<UserViewModel, User>
    {
        public UserRecyclerAdapter(BaseAniDroidv2Activity context,
            IAsyncEnumerable<OneOf<IPagedData<User>, IAniListError>> enumerable, RecyclerCardType cardType,
            Func<User, UserViewModel> createViewModelFunc) : base(context, enumerable, cardType, createViewModelFunc)
        {
            SetDefaultClickActions();
        }

        public override CardItem SetupCardItemViewHolder(CardItem item)
        {
            item.Button.Visibility = ViewStates.Gone;
            item.DetailSecondary.Visibility = ViewStates.Gone;
            return item;
        }

        private void SetDefaultClickActions()
        {
            ClickAction =
                (viewModel, position) => UserActivity.StartActivity(Context, viewModel.Model.Id);
        }
    }
}
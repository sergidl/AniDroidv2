using System;
using System.Collections.Generic;
using Android.Views;
using AniDroidv2.Adapters.Base;
using AniDroidv2.Adapters.ViewModels;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.StaffModels;
using AniDroidv2.AniListObject.Staff;
using AniDroidv2.Base;
using OneOf;

namespace AniDroidv2.Adapters.StaffAdapters
{
    public class StaffRecyclerAdapter : AniDroidv2RecyclerAdapter<StaffViewModel, Staff>
    {
        public StaffRecyclerAdapter(BaseAniDroidv2Activity context,
            IAsyncEnumerable<OneOf<IPagedData<Staff>, IAniListError>> enumerable, RecyclerCardType cardType,
            Func<Staff, StaffViewModel> createViewModelFunc) : base(context, enumerable, cardType, createViewModelFunc)
        {
            SetDefaultClickActions();
        }

        public StaffRecyclerAdapter(BaseAniDroidv2Activity context, List<StaffViewModel> items, RecyclerCardType cardType)
            : base(context, items, cardType)
        {
            SetDefaultClickActions();
        }

        public override CardItem SetupCardItemViewHolder(CardItem item)
        {
            item.Button.Visibility = ViewStates.Gone;
            return item;
        }

        private void SetDefaultClickActions()
        {
            ClickAction = (viewModel, position) =>
                StaffActivity.StartActivity(Context, viewModel.Model.Id, BaseAniDroidv2Activity.ObjectBrowseRequestCode);
        }
    }
}
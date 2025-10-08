using System;
using System.Collections.Generic;
using AniDroidv2.Adapters.Base;
using AniDroidv2.Adapters.ViewModels;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.StaffModels;
using AniDroidv2.AniListObject.Staff;
using AniDroidv2.Base;
using OneOf;

namespace AniDroidv2.Adapters.StaffAdapters
{
    public class StaffEdgeRecyclerAdapter : AniDroidv2RecyclerAdapter<StaffEdgeViewModel, StaffEdge>
    {
        public StaffEdgeRecyclerAdapter(BaseAniDroidv2Activity context,
            IAsyncEnumerable<OneOf<IPagedData<StaffEdge>, IAniListError>> enumerable, RecyclerCardType cardType,
            Func<StaffEdge, StaffEdgeViewModel> createViewModelFunc) : base(context, enumerable, cardType,
            createViewModelFunc)
        {
            SetupDefaultClickActions();
        }

        private void SetupDefaultClickActions()
        {
            ClickAction = (viewModel, position) =>
                StaffActivity.StartActivity(Context, viewModel.Model.Node.Id,
                    BaseAniDroidv2Activity.ObjectBrowseRequestCode);
        }
    }
}
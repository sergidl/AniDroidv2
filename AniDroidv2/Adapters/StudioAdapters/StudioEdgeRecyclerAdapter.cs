using System;
using System.Collections.Generic;
using AniDroidv2.Adapters.Base;
using AniDroidv2.Adapters.ViewModels;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.StudioModels;
using AniDroidv2.AniListObject.Studio;
using AniDroidv2.Base;
using OneOf;

namespace AniDroidv2.Adapters.StudioAdapters
{
    public class StudioEdgeRecyclerAdapter : AniDroidv2RecyclerAdapter<StudioEdgeViewModel, StudioEdge>
    {
        public StudioEdgeRecyclerAdapter(BaseAniDroidv2Activity context, List<StudioEdgeViewModel> items) : base(context,
            items, RecyclerCardType.Horizontal)
        {
            ClickAction = (viewModel, position) => StudioActivity.StartActivity(Context, viewModel.Model?.Node?.Id ?? 0,
                BaseAniDroidv2Activity.ObjectBrowseRequestCode);
        }

        public StudioEdgeRecyclerAdapter(BaseAniDroidv2Activity context,
            IAsyncEnumerable<OneOf<IPagedData<StudioEdge>, IAniListError>> enumerable, RecyclerCardType cardType,
            Func<StudioEdge, StudioEdgeViewModel> createViewModelFunc) : base(context, enumerable, cardType,
            createViewModelFunc)
        {
            ClickAction = (viewModel, position) => StudioActivity.StartActivity(Context, viewModel.Model?.Node?.Id ?? 0,
                BaseAniDroidv2Activity.ObjectBrowseRequestCode);
        }

        public override CardItem SetupCardItemViewHolder(CardItem item)
        {
            item.ContainerCard.SetContentPadding(20, 20, 20, 20);

            return item;
        }
    }
}
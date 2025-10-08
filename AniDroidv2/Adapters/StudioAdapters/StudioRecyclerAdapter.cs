using System;
using System.Collections.Generic;
using Android.Views;
using AndroidX.Core.Widget;
using AniDroidv2.Adapters.Base;
using AniDroidv2.Adapters.ViewModels;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.StudioModels;
using AniDroidv2.AniListObject.Studio;
using AniDroidv2.Base;
using OneOf;

namespace AniDroidv2.Adapters.StudioAdapters
{
    public class StudioRecyclerAdapter : AniDroidv2RecyclerAdapter<StudioViewModel, Studio>
    {
        public StudioRecyclerAdapter(BaseAniDroidv2Activity context,
            IAsyncEnumerable<OneOf<IPagedData<Studio>, IAniListError>> enumerable,
            Func<Studio, StudioViewModel> createViewModelFunc) : base(context, enumerable, RecyclerCardType.Horizontal,
            createViewModelFunc)
        {
            ClickAction =
                (viewModel, position) => StudioActivity.StartActivity(Context, viewModel.Model.Id);
        }

        public override void BindCardViewHolder(CardItem holder, int position)
        {
            holder.Image.Visibility = ViewStates.Gone;
        }

        public override CardItem SetupCardItemViewHolder(CardItem item)
        {
            item.ButtonIcon.SetImageResource(Resource.Drawable.ic_favorite_white_24px);
            ImageViewCompat.SetImageTintList(item.ButtonIcon, FavoriteIconColor);
            item.ContainerCard.SetContentPadding(0, 20, 0, 20);

            return item;
        }
    }
}
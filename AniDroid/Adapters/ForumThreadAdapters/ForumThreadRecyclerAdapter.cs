using System;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using AniDroidv2.Adapters.Base;
using AniDroidv2.Adapters.ViewModels;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.ForumModels;
using AniDroidv2.Base;
using OneOf;

namespace AniDroidv2.Adapters.ForumThreadAdapters
{
    public class ForumThreadRecyclerAdapter : AniDroidv2RecyclerAdapter<ForumThreadViewModel, ForumThread>
    {
        public ForumThreadRecyclerAdapter(BaseAniDroidv2Activity context,
            IAsyncEnumerable<OneOf<IPagedData<ForumThread>, IAniListError>> enumerable,
            Func<ForumThread, ForumThreadViewModel> createViewModelFunc) : base(context, enumerable,
            RecyclerCardType.Horizontal, createViewModelFunc)
        {
            ClickAction = (viewModel, position) =>
            {
                var intent = new Intent(Intent.ActionView);
                intent.SetData(Android.Net.Uri.Parse(viewModel.Model.SiteUrl));
                Context.StartActivity(intent);
            };
        }

        public override CardItem SetupCardItemViewHolder(CardItem item)
        {
            item.Button.Visibility = ViewStates.Gone;
            return item;
        }
    }
}
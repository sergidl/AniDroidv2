using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using AniDroidv2.Adapters.Base;
using AniDroidv2.Adapters.ViewModels;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.ReviewModels;
using AniDroidv2.Base;
using OneOf;

namespace AniDroidv2.Adapters.ReviewAdapters
{
    public class ReviewRecyclerAdapter : AniDroidv2RecyclerAdapter<ReviewViewModel, Review>
    {
        public ReviewRecyclerAdapter(BaseAniDroidv2Activity context,
            IAsyncEnumerable<OneOf<IPagedData<Review>, IAniListError>> enumerable, RecyclerCardType cardType,
            Func<Review, ReviewViewModel> createViewModelFunc) : base(context, enumerable, cardType,
            createViewModelFunc)
        {
            ClickAction = (viewModel, position) =>
            {
                Toast.MakeText(Application.Context, "In-app review viewing coming Soon™", ToastLength.Short).Show();
                var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse($"https://anilist.co/review/{viewModel.Model.Id}"));
                Context.StartActivity(intent);
            };
        }

        public override CardItem SetupCardItemViewHolder(CardItem item)
        {
            item.Button.Visibility = ViewStates.Gone;
            item.Name.SetSingleLine(false);
            item.Name.SetMaxLines(2);
            return item;
        }
    }
}
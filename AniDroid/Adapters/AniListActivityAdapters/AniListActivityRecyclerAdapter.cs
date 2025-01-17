﻿using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content.Res;
using Android.Graphics;
using Android.Text;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using AniDroidv2.Adapters.Base;
using AniDroidv2.AniList.Enums.ActivityEnums;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.ActivityModels;
using AniDroidv2.AniListObject;
using AniDroidv2.AniListObject.Media;
using AniDroidv2.AniListObject.User;
using AniDroidv2.Base;
using AniDroidv2.Dialogs;
using AniDroidv2.Utils;
using AniDroidv2.Utils.Formatting.Markdown;
using OneOf;

namespace AniDroidv2.Adapters.AniListActivityAdapters
{
    public class AniListActivityRecyclerAdapter : LazyLoadingRecyclerViewAdapter<AniListActivity>
    {
        private readonly IAniListActivityPresenter _presenter;
        private readonly string _userNameColorHex;
        private readonly string _actionColorHex;
        private readonly int? _userId;
        private readonly Color _defaultIconColor;

        public AniListActivityRecyclerAdapter(BaseAniDroidv2Activity context, IAniListActivityPresenter presenter,
            IAsyncEnumerable<OneOf<IPagedData<AniListActivity>, IAniListError>> enumerable, int? currentUserId) : base(context, enumerable, RecyclerCardType.Custom)
        {
            _presenter = presenter;
            _userId = currentUserId;
            _userNameColorHex = $"#{Context.GetThemedColor(Resource.Attribute.Primary) & 0xffffff:X6}";
            _actionColorHex = $"#{Context.GetThemedColor(Resource.Attribute.Primary_Dark) & 0xffffff:X6}";
            _defaultIconColor = new Color(context.GetThemedColor(Resource.Attribute.Secondary_Dark));
            CustomCardUseItemDecoration = true;
        }

        public AniListActivityRecyclerAdapter(BaseAniDroidv2Activity context, AniListActivityRecyclerAdapter adapter) :
            base(context, adapter)
        {
            _presenter = adapter._presenter;
            _userId = adapter._userId;
            _userNameColorHex = $"#{Context.GetThemedColor(Resource.Attribute.Primary) & 0xffffff:X6}";
            _actionColorHex = $"#{Context.GetThemedColor(Resource.Attribute.Primary_Dark) & 0xffffff:X6}";
            _defaultIconColor = new Color(context.GetThemedColor(Resource.Attribute.Secondary_Dark));
            CustomCardUseItemDecoration = adapter.CustomCardUseItemDecoration;
        }

        public override void BindCustomViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (!(holder is AniListActivityViewHolder viewHolder))
            {
                return;
            }

            var item = Items[position];


            viewHolder.Timestamp.Text = item.GetAgeString(item.CreatedAt);

            if (item.ReplyCount > 0)
            {
                viewHolder.ReplyCountContainer.Visibility = ViewStates.Visible;
                viewHolder.ReplyCount.Text = item.ReplyCount.ToString();
            }
            else
            {
                viewHolder.ReplyCountContainer.Visibility = ViewStates.Gone;
            }

            viewHolder.LikeCount.Text = item.Likes?.Count.ToString();

            if (_userId.HasValue)
            {
                viewHolder.LikeIcon.ImageTintList = ColorStateList.ValueOf(item.Likes?.Any(x => x.Id == _userId) == true
                    ? Color.Crimson
                    : _defaultIconColor);
                viewHolder.ReplyLikeContainer.SetTag(Resource.Id.Object_Position, position);
                viewHolder.ReplyLikeContainer.Click -= ShowReplyDialog;
                viewHolder.ReplyLikeContainer.Click += ShowReplyDialog;
            }

            viewHolder.Image.SetTag(Resource.Id.Object_Position, position);
            viewHolder.Image.Click -= ImageClick;
            viewHolder.Image.Click += ImageClick;

            viewHolder.Container.SetTag(Resource.Id.Object_Position, position);
            viewHolder.Container.Click -= RowClick;
            viewHolder.Container.Click += RowClick;
            viewHolder.Container.LongClick -= RowLongClick;
            viewHolder.Container.LongClick += RowLongClick;

            if (item.Type == ActivityType.Text)
            {
                BindTextActivityViewHolder(viewHolder, item);
            }
            else if (item.Type == ActivityType.Message)
            {
                BindMessageActivityViewHolder(viewHolder, item);
            }
            else if (item.Type == ActivityType.AnimeList ||
                     item.Type == ActivityType.MangaList)
            {
                BindListActivityViewHolder(viewHolder, item);
            }
        }

        public override RecyclerView.ViewHolder CreateCustomViewHolder(ViewGroup parent, int viewType)
        {
            var holder = new AniListActivityViewHolder(
                Context.LayoutInflater.Inflate(Resource.Layout.View_AniListActivityItem, parent, false));

            if (!_userId.HasValue)
            {
                holder.ReplyButton.Visibility = ViewStates.Gone;
            }

            return holder;
        }

        private void BindTextActivityViewHolder(AniListActivityViewHolder viewHolder, AniListActivity item)
        {
            viewHolder.Title.TextFormatted = BaseAniDroidv2Activity.FromHtml($"<b><font color='{_userNameColorHex}'>{item.User?.Name}</font></b>");
            viewHolder.ContentImageContainer.Visibility = ViewStates.Gone;

            var builder = new SpannableStringBuilder(MarkdownTextCleaner.ConvertToSpanned(item.Text));
            //viewHolder.ContentText.MovementMethod = LinkMovementMethod.Instance;
            viewHolder.ContentText.SetText(builder, TextView.BufferType.Spannable);
            MarkdownSpannableFormatter.FormatMarkdownSpannable(Context, viewHolder.ContentText.TextFormatted as SpannableString);
            viewHolder.ContentText.Visibility = ViewStates.Visible;

            ImageLoader.LoadImage(viewHolder.Image, item.User?.Avatar?.Large);
        }

        private void BindMessageActivityViewHolder(AniListActivityViewHolder viewHolder, AniListActivity item)
        {
            viewHolder.Title.TextFormatted = BaseAniDroidv2Activity.FromHtml($"<b><font color='{_userNameColorHex}'>{item.Messenger?.Name}</font></b>");
            viewHolder.ContentImageContainer.Visibility = ViewStates.Gone;

            var builder = new SpannableStringBuilder(MarkdownTextCleaner.ConvertToSpanned(item.Message));
            //viewHolder.ContentText.MovementMethod = LinkMovementMethod.Instance;
            viewHolder.ContentText.SetText(builder, TextView.BufferType.Spannable);
            MarkdownSpannableFormatter.FormatMarkdownSpannable(Context, viewHolder.ContentText.TextFormatted as SpannableString);
            viewHolder.ContentText.Visibility = ViewStates.Visible;

            ImageLoader.LoadImage(viewHolder.Image, item.Messenger?.Avatar?.Large);
        }

        private void BindListActivityViewHolder(AniListActivityViewHolder viewHolder, AniListActivity item)
        {
            viewHolder.Title.TextFormatted = BaseAniDroidv2Activity.FromHtml($"<b><font color='{_userNameColorHex}'>{item.User?.Name}</font></b> {item.Status} {(!string.IsNullOrWhiteSpace(item.Progress) ? $"{item.Progress} of" : "")} <b><font color='{_actionColorHex}'>{item.Media?.Title?.UserPreferred}</font></b>");
            viewHolder.ContentText.Visibility = ViewStates.Gone;
            viewHolder.ContentImageContainer.Visibility = ViewStates.Visible;
            viewHolder.ContentImageContainer.RemoveAllViews();

            ImageLoader.LoadImage(viewHolder.Image, item.Media?.CoverImage?.Large);
        }

        private void ImageClick(object sender, EventArgs e)
        {
            var image = sender as ImageView;
            var position = (int)image.GetTag(Resource.Id.Object_Position);
            var item = Items[position];

            if (item.Type == ActivityType.Text)
            {
                if (item.User?.Id == null)
                {
                    return;
                }

                UserActivity.StartActivity(Context, item.User.Id);
            }
            else if (item.Type == ActivityType.Message)
            {
                if (item.Messenger?.Id == null)
                {
                    return;
                }

                UserActivity.StartActivity(Context, item.Messenger.Id);
            }
            else if (item.Type == ActivityType.AnimeList || item.Type == ActivityType.MangaList)
            {
                if (item.Media?.Id == null)
                {
                    return;
                }

                MediaActivity.StartActivity(Context, item.Media.Id);
            }
        }

        private void RowClick(object sender, EventArgs e)
        {
            var view = sender as View;
            var position = (int)view.GetTag(Resource.Id.Object_Position);
            var item = Items[position];

            if (item.Type == ActivityType.Text || item.Type == ActivityType.AnimeList || item.Type == ActivityType.MangaList)
            {
                UserActivity.StartActivity(Context, item.User.Id);
            }
            else if (item.Type == ActivityType.Message)
            {
                UserActivity.StartActivity(Context, item.Messenger.Id);
            }
        }

        private void RowLongClick(object sender, View.LongClickEventArgs e)
        {
            var view = sender as View;
            var position = (int)view.GetTag(Resource.Id.Object_Position);
            var item = Items[position];

            if (item.Type == ActivityType.Text && item.UserId == _userId)
            {
                AniListActivityCreateDialog.CreateEditActivity(Context, item.Text,
                    text => _presenter.EditStatusActivityAsync(item, position, text),
                    () => _presenter.DeleteActivityAsync(item.Id, position));
            }
        }

        private void ShowReplyDialog(object sender, EventArgs e)
        {
            var senderView = sender as View;
            var activityPosition = (int)senderView.GetTag(Resource.Id.Object_Position);
            var activity = Items[activityPosition];

            if (_userId.HasValue || activity.Likes?.Any() == true || activity.Replies?.Any() == true)
            {
                AniListActivityRepliesDialog.Create(Context, activity, activityPosition, _presenter, _userId, PostReply, ToggleLikeActivity);
            }
        }

        private async void ToggleLikeActivity(int activityId)
        {
            var activityItemPosition = Items.FindIndex(x => x?.Id == activityId);
            var activityItem = Items[activityItemPosition];
            Items[activityItemPosition] = null;
            NotifyItemChanged(activityItemPosition);

            await _presenter.ToggleActivityLikeAsync(activityItem, activityItemPosition);
        }

        private async void PostReply(int activityId, string text)
        {
            var activityItemPosition = Items.FindIndex(x => x?.Id == activityId);
            var activityItem = Items[activityItemPosition];
            Items[activityItemPosition] = null;
            NotifyItemChanged(activityItemPosition);

            await _presenter.PostActivityReplyAsync(activityItem, activityItemPosition, text);
        }

        public class AniListActivityViewHolder : RecyclerView.ViewHolder
        {
            public View Container { get; set; }
            public ImageView Image { get; set; }
            public TextView Title { get; set; }
            public TextView Timestamp { get; set; }
            public TextView ContentText { get; set; }
            public LinearLayout ContentImageContainer { get; set; }
            public View ReplyContainer { get; set; }
            public View ReplyCountContainer { get; set; }
            public TextView ReplyCount { get; set; }
            public View ReplyLikeContainer { get; set; }
            public TextView LikeCount { get; set; }
            public ImageView LikeIcon { get; set; }
            public View ReplyButton { get; set; }

            public AniListActivityViewHolder(View view) : base(view)
            {
                Container = view.FindViewById(Resource.Id.AniListActivity_Container);
                Image = view.FindViewById<ImageView>(Resource.Id.AniListActivity_Image);
                Title = view.FindViewById<TextView>(Resource.Id.AniListActivity_Title);
                Timestamp = view.FindViewById<TextView>(Resource.Id.AniListActivity_Timestamp);
                ContentText = view.FindViewById<TextView>(Resource.Id.AniListActivity_ContentText);
                ContentImageContainer = view.FindViewById<LinearLayout>(Resource.Id.AniListActivity_ContentImageContainer);
                ReplyContainer = view.FindViewById(Resource.Id.AniListActivity_ReplyContainer);
                ReplyCountContainer = view.FindViewById(Resource.Id.AniListActivity_ReplyCountContainer);
                ReplyCount = view.FindViewById<TextView>(Resource.Id.AniListActivity_ReplyCount);
                ReplyLikeContainer = view.FindViewById(Resource.Id.AniListActivity_ReplyLikeContainer);
                LikeCount = view.FindViewById<TextView>(Resource.Id.AniListActivity_LikeCount);
                LikeIcon = view.FindViewById<ImageView>(Resource.Id.AniListActivity_LikeIcon);
                ReplyButton = view.FindViewById(Resource.Id.AniListActivity_ReplyButton);
            }
        }
    }
}
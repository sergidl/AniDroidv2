﻿using System;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using AniDroidv2.Adapters.Base;
using AniDroidv2.Adapters.ViewModels;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.ActivityModels;
using AniDroidv2.Base;
using AniDroidv2.Utils;
using OneOf;

namespace AniDroidv2.Adapters.UserAdapters
{
    public class AniListNotificationRecyclerAdapter : AniDroidv2RecyclerAdapter<AniListNotificationViewModel, AniListNotification>
    {
        private readonly int _unreadCount;

        public AniListNotificationRecyclerAdapter(
            BaseAniDroidv2Activity context,
            IAsyncEnumerable<OneOf<IPagedData<AniListNotification>,IAniListError>> enumerable,
            int unreadCount,
            Func<AniListNotification, AniListNotificationViewModel> createViewModelFunc) : 
            base(context, enumerable, RecyclerCardType.Custom, createViewModelFunc)
        {
            _unreadCount = unreadCount;
            CustomCardUseItemDecoration = true;
            ClickAction = (viewModel, position) => (viewModel as AniListNotificationViewModel)?.ClickAction?.Invoke();
        }

        public override void BindCustomViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = holder as AniListNotificationViewHolder;
            var viewModel = Items[position];

            viewHolder.Text.TextFormatted = viewModel.FormattedTitle;
            viewHolder.Timestamp.Text = viewModel.Timestamp;
            ImageLoader.LoadImage(viewHolder.Image, viewModel.ImageUri);

            viewHolder.ItemView.SetTag(Resource.Id.Object_Position, position);
            viewHolder.ItemView.Click -= RowClick;
            viewHolder.ItemView.Click += RowClick;
        }

        public override RecyclerView.ViewHolder CreateCustomViewHolder(ViewGroup parent, int viewType)
        {
            return new AniListNotificationViewHolder(
                Context.LayoutInflater.Inflate(Resource.Layout.View_AniListNotificationItem, parent, false));
        }

        public class AniListNotificationViewHolder : RecyclerView.ViewHolder
        {
            public ImageView Image { get; set; }
            public TextView Text { get; set; }
            public TextView Timestamp { get; set; }

            public AniListNotificationViewHolder(View itemView) : base(itemView)
            {
                Image = itemView.FindViewById<ImageView>(Resource.Id.AniListNotification_Image);
                Text = itemView.FindViewById<TextView>(Resource.Id.AniListNotification_Text);
                Timestamp = itemView.FindViewById<TextView>(Resource.Id.AniListNotification_Timestamp);
            }
        }
    }
}
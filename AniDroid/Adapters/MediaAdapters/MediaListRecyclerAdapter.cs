﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using AniDroid.Adapters.Base;
using AniDroid.AniList.Models;
using AniDroid.AniListObject.Media;
using AniDroid.Base;
using AniDroid.Dialogs;
using AniDroid.MediaList;

namespace AniDroid.Adapters.MediaAdapters
{
    public class MediaListRecyclerAdapter : BaseRecyclerAdapter<Media.MediaList>
    {
        private readonly MediaListPresenter _presenter;
        private readonly User.UserMediaListOptions _mediaListOptions;
        private readonly bool _isCustomList;
        private readonly string _listName;

        public MediaListRecyclerAdapter(BaseAniDroidActivity context, Media.MediaListGroup mediaListGroup,
            User.UserMediaListOptions mediaListOptions, MediaListPresenter presenter, CardType cardType,
            int verticalCardColumns = 2) : base(context, mediaListGroup.Entries, cardType, verticalCardColumns)
        {
            _presenter = presenter;
            _mediaListOptions = mediaListOptions;
            _isCustomList = mediaListGroup.IsCustomList;
            _listName = mediaListGroup.Name;
        }

        public void UpdateMediaListItem(int mediaId, Media.MediaList updatedMediaList)
        {
            var position = Items.FindIndex(x => x.Media.Id == mediaId);

            if (position >= 0)
            {
                var oldItem = Items[position];

                if (updatedMediaList.HiddenFromStatusLists && !_isCustomList ||
                    _isCustomList && !updatedMediaList.CustomLists.Any(x => x.Enabled && x.Name == _listName))
                {
                    Items.RemoveAt(position);
                    NotifyDataSetChanged();
                }
                else
                {
                    Items[position] = updatedMediaList;
                    NotifyItemChanged(position);
                }

                // TODO: implement rest of list cleaning (status lists, etc.)
            }
            else
            {
                if (_isCustomList && updatedMediaList.CustomLists.Any(x => x.Enabled && x.Name == _listName))
                {
                    Items.Insert(0, updatedMediaList);
                    NotifyDataSetChanged();
                }
            }
        }

        public override void BindCardViewHolder(CardItem holder, int position)
        {
            var item = Items[position];

            holder.Name.Text = item.Media.Title.UserPreferred;
            holder.DetailPrimary.Text = GetDetailOne(item);
            holder.DetailSecondary.Text = GetDetailTwo(item);
            holder.Button.SetTag(Resource.Id.Object_Position, position);
            Context.LoadImage(holder.Image, item.Media.CoverImage.Large);

            holder.ContainerCard.SetTag(Resource.Id.Object_Position, position);
            holder.ContainerCard.Click -= RowClick;
            holder.ContainerCard.Click += RowClick;
            holder.ContainerCard.LongClick -= RowLongClick;
            holder.ContainerCard.LongClick += RowLongClick;

            if (item.Status == Media.MediaListStatus.Current)
            {
                holder.Button.Visibility = ViewStates.Visible;

                holder.ButtonIcon.SetImageResource(item.Progress + 1 >= (item.Media.Episodes ?? item.Media.Chapters)
                    ? Resource.Drawable.svg_check_circle_outline
                    : Resource.Drawable.svg_plus_circle_outline);
            }
            else
            {
                holder.Button.Visibility = ViewStates.Gone;
            }
        }

        public override CardItem SetupCardItemViewHolder(CardItem item)
        {
            item.Button.Click -= ButtonClick;
            item.Button.Click += ButtonClick;

            return item;
        }

        private void RowClick(object sender, EventArgs e)
        {
            var senderView = sender as View;
            var mediaListPos = (int)senderView.GetTag(Resource.Id.Object_Position);
            var mediaList = Items[mediaListPos];

            MediaActivity.StartActivity(Context, mediaList.Media.Id, BaseAniDroidActivity.ObjectBrowseRequestCode);
        }

        private void RowLongClick(object sender, View.LongClickEventArgs longClickEventArgs)
        {
            var senderView = sender as View;
            var mediaListPos = (int)senderView.GetTag(Resource.Id.Object_Position);
            var mediaList = Items[mediaListPos];

            EditMediaListItemDialog.Create(Context, _presenter, mediaList.Media, mediaList, _mediaListOptions);
        }

        private async void ButtonClick(object sender, EventArgs eventArgs)
        {
            var senderView = sender as View;
            var mediaListPos = (int)senderView.GetTag(Resource.Id.Object_Position);
            var mediaList = Items[mediaListPos];

            if (mediaList.Progress + 1 == (mediaList.Media.Episodes ?? mediaList.Media.Chapters))
            {
                await _presenter.CompleteMedia(mediaList);
            }
            else
            {
                await _presenter.IncreaseMediaProgress(mediaList);
            }
        }

        private string GetDetailOne(Media.MediaList mediaList)
        {
            if (mediaList.Status == Media.MediaListStatus.Planning)
            {
                if (mediaList.Media.Type == Media.MediaType.Anime)
                {
                    return $"{mediaList.Media.Episodes?.ToString() ?? "?"} episode(s)";
                }

                if (mediaList.Media.Type == Media.MediaType.Manga)
                {
                    return $"{mediaList.Media.Chapters?.ToString() ?? "?"} chapter(s)";
                }
            }

            if (mediaList.Status == Media.MediaListStatus.Current || mediaList.Status == Media.MediaListStatus.Paused || mediaList.Status == Media.MediaListStatus.Dropped || mediaList.Status == Media.MediaListStatus.Repeating)
            {
                if (mediaList.Progress.HasValue &&
                    mediaList.Progress == (mediaList.Media.Episodes ?? mediaList.Media.Chapters))
                {
                    return "Status needs to be marked as Completed";
                }

                if (mediaList.Media.Type == Media.MediaType.Anime)
                {
                    return $"Watched {mediaList.Progress ?? 0} out of {mediaList.Media.Episodes?.ToString() ?? "?"}";
                }

                if (mediaList.Media.Type == Media.MediaType.Manga)
                {
                    return $"Read {mediaList.Progress ?? 0} out of {mediaList.Media.Chapters?.ToString() ?? "?"}";
                }
            }

            return mediaList.GetScoreString(_mediaListOptions.ScoreFormat);
        }

        private string GetDetailTwo(Media.MediaList mediaList)
        {
            return $"{mediaList.Media.Format?.DisplayValue}{(mediaList.Media.IsAdult ? " (Hentai)" : "")}";
        }
    }
}
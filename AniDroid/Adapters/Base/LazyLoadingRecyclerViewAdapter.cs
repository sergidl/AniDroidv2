using System;
using System.Collections.Generic;
using System.Linq;
using Android.Graphics;
using Android.Views;
using Android.Views.Animations;
using AndroidX.RecyclerView.Widget;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.Base;
using Google.Android.Material.Snackbar;
using OneOf;

namespace AniDroidv2.Adapters.Base
{
    public abstract class LazyLoadingRecyclerViewAdapter<T> : BaseRecyclerAdapter<T> where T : class
    {
        private readonly IAsyncEnumerable<OneOf<IPagedData<T>, IAniListError>> _asyncEnumerable;
        private IAsyncEnumerator<OneOf<IPagedData<T>, IAniListError>> _asyncEnumerator;
        private bool _isLazyLoading;
        private bool _dataLoaded;

        protected int LoadingCardWidth = ViewGroup.LayoutParams.MatchParent;
        protected int LoadingCardHeight = ViewGroup.LayoutParams.WrapContent;

        public Color? LoadingItemBackgroundColor { get; set; }

        protected LazyLoadingRecyclerViewAdapter(BaseAniDroidv2Activity context, IAsyncEnumerable<OneOf<IPagedData<T>, IAniListError>> enumerable, RecyclerCardType cardType) : base(context, new List<T> { null }, cardType)
        {
            _asyncEnumerable = enumerable;
            _asyncEnumerator = enumerable.GetAsyncEnumerator();
        }

        protected LazyLoadingRecyclerViewAdapter(BaseAniDroidv2Activity context,
            LazyLoadingRecyclerViewAdapter<T> adapter) : base(context, adapter.Items, adapter.CardType)
        {
            _asyncEnumerable = adapter._asyncEnumerable;
            _asyncEnumerator = adapter._asyncEnumerator;
        }

        public void ResetAdapter()
        {
            _asyncEnumerator = _asyncEnumerable.GetAsyncEnumerator();
            Items.Clear();
            Items.Add(null);
            NotifyDataSetChanged();
        }

        public sealed override async void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (Items[position] != null)
            {
                base.OnBindViewHolder(holder, position);
                return;
            }

            if (position < ItemCount - 1 || _isLazyLoading)
            {
                return;
            }

            _isLazyLoading = true;

            var moveNextResult = await _asyncEnumerator.MoveNextAsync();

            _asyncEnumerator.Current?.Switch((IAniListError error) =>
                    Context.DisplaySnackbarMessage("Error occurred while getting next page of data", Snackbar.LengthLong))
                .Switch(data =>
                {
                    if (!moveNextResult)
                    {
                        return;
                    }

                    if (!_dataLoaded)
                    {
                        DataLoaded?.Invoke(RecyclerView, data.PageInfo.Total > 0);
                        _dataLoaded = true;
                    }

                    AddItems(data.Data, data.PageInfo.HasNextPage);
                });

            RemoveItem(position);

            _isLazyLoading = false;
        }

        public sealed override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if (viewType != ProgressBarViewType)
            {
                return base.OnCreateViewHolder(parent, viewType);
            }

            var view = Context.LayoutInflater.Inflate(Resource.Layout.View_IndeterminateProgressIndicator, parent, false);
            view.LayoutParameters.Width = LoadingCardWidth;
            view.LayoutParameters.Height = LoadingCardHeight;
            var holder = new ProgressBarViewHolder(view, LoadingItemBackgroundColor);

            return holder;
        }

        public override bool InsertItem(int position, T item, bool notify = true)
        {
            return !_isLazyLoading && base.InsertItem(position, item, notify);
        }

        public override bool ReplaceItem(int position, T item, bool notify = true)
        {
            return !_isLazyLoading && base.ReplaceItem(position, item, notify);
        }

        public void AddItems(IEnumerable<T> itemsToAdd, bool hasNextPage)
        {
            var initialAdd = Items.Count == 1 && Items[0] == null;

            if (hasNextPage)
            {
                itemsToAdd = itemsToAdd.Append(null);
            }

            Items.AddRange(itemsToAdd);

            NotifyDataSetChanged();

            if (initialAdd)
            {
                var controller = AnimationUtils.LoadLayoutAnimation(Context, Resource.Animation.Layout_Animation_Falldown);
                RecyclerView.LayoutAnimation = controller;
                RecyclerView.ScheduleLayoutAnimation();
            }
        }

        public sealed override int GetItemViewType(int position)
        {
            return (Items[position] == null) ? ProgressBarViewType : 0;
        }

        private class ProgressBarViewHolder : RecyclerView.ViewHolder
        {
            public ProgressBarViewHolder(View itemView, Color? backgroundColor = null) : base(itemView)
            {
                if (backgroundColor.HasValue)
                {
                    itemView.SetBackgroundColor(backgroundColor.Value);
                }
            }
        }

        public event EventHandler<bool> DataLoaded;

        #region Constants

        private const int ProgressBarViewType = -1;

        #endregion
    }
}
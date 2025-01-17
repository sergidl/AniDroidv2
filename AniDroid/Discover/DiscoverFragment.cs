﻿using System;
using System.Collections.Generic;
using Android.OS;
using Android.Views;
using Android.Widget;
using AniDroidv2.Adapters.Base;
using AniDroidv2.Adapters.MediaAdapters;
using AniDroidv2.Adapters.ViewModels;
using AniDroidv2.AniList.Enums.MediaEnums;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.MediaModels;
using AniDroidv2.Base;
using AniDroidv2.Dialogs;
using AniDroidv2.MediaList;
using AniDroidv2.Widgets;
using OneOf;

namespace AniDroidv2.Discover
{
    public class DiscoverFragment : BaseMainActivityFragment<DiscoverPresenter>, IDiscoverView
    {
        private LinearLayout _listContainer;
        private MediaRecyclerAdapter _currentlyAiringRecyclerAdapter;
        private MediaRecyclerAdapter _trendingAnimeRecyclerAdapter;
        private MediaRecyclerAdapter _trendingMangaRecyclerAdapter;
        private MediaRecyclerAdapter _newAnimeRecyclerAdapter;
        private MediaRecyclerAdapter _newMangaRecyclerAdapter;

        private List<MediaRecyclerAdapter> AdapterList => new List<MediaRecyclerAdapter>
        {
            _currentlyAiringRecyclerAdapter,
            _trendingAnimeRecyclerAdapter,
            _trendingMangaRecyclerAdapter,
            _newAnimeRecyclerAdapter,
            _newMangaRecyclerAdapter
        };

        private static DiscoverFragment _instance;

        private const int CardWidth = 150;

        public override bool HasMenu => true;
        public override string FragmentName => "DISCOVER_FRAGMENT";

        protected override void SetInstance(BaseMainActivityFragment instance)
        {
            _instance = instance as DiscoverFragment;
        }

        public override void ClearState()
        {
            _instance = null;
        }

        public override View CreateMainActivityFragmentView(ViewGroup container, Bundle savedInstanceState)
        {
            CreatePresenter(savedInstanceState).GetAwaiter().GetResult();

            var view = LayoutInflater.Inflate(Resource.Layout.Fragment_Discover, container, false);
            _listContainer = view.FindViewById<LinearLayout>(Resource.Id.Discover_Container);

            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            Presenter.GetDiscoverLists();
        }

        public override void SetupMenu(IMenu menu)
        {
            menu.Clear();
            var inflater = new MenuInflater(Context);
            inflater.Inflate(Resource.Menu.Discover_ActionBar, menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.Menu_Discover_Refresh)
            {
                _currentlyAiringRecyclerAdapter.ResetAdapter();
                _trendingAnimeRecyclerAdapter.ResetAdapter();
                _trendingMangaRecyclerAdapter.ResetAdapter();
                _newAnimeRecyclerAdapter.ResetAdapter();
                _newMangaRecyclerAdapter.ResetAdapter();
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public override void OnError(IAniListError error)
        {
            throw new NotImplementedException();
        }

        public void ShowCurrentlyAiringResults(
            IAsyncEnumerable<OneOf<IPagedData<Media>, IAniListError>> mediaEnumerable)
        {
            var currentlyAiringView = new SideScrollingList(Activity) {LabelText = "Currently Airing"};

            var adapter = new MediaRecyclerAdapter(Activity, mediaEnumerable,
                BaseRecyclerAdapter.RecyclerCardType.Vertical, MediaViewModel.CreateMediaViewModel)
            {
                LongClickAction = (viewModel, position) =>
                {
                    if (Presenter.GetIsUserAuthenticated())
                    {
                        EditMediaListItemDialog.Create(Activity, Presenter, viewModel.Model,
                            viewModel.Model.MediaListEntry,
                            Presenter.GetLoggedInUser()?.MediaListOptions);
                    }
                },

            };
            adapter.SetHorizontalAdapterCardWidthDip(CardWidth);

            currentlyAiringView.RecyclerAdapter = _currentlyAiringRecyclerAdapter = adapter;

            _listContainer.AddView(currentlyAiringView);
        }

        public void ShowTrendingAnimeResults(IAsyncEnumerable<OneOf<IPagedData<Media>, IAniListError>> mediaEnumerable)
        {
            var trendingView = new SideScrollingList(Activity) { LabelText = "Trending Anime" };

            var adapter = new MediaRecyclerAdapter(Activity, mediaEnumerable,
                BaseRecyclerAdapter.RecyclerCardType.Vertical, MediaViewModel.CreateMediaViewModel)
            {
                LongClickAction = (viewModel, position) =>
                {
                    if (Presenter.GetIsUserAuthenticated())
                    {
                        EditMediaListItemDialog.Create(Activity, Presenter, viewModel.Model,
                            viewModel.Model.MediaListEntry,
                            Presenter.GetLoggedInUser()?.MediaListOptions);
                    }
                },

            };
            adapter.SetHorizontalAdapterCardWidthDip(CardWidth);

            trendingView.RecyclerAdapter = _trendingAnimeRecyclerAdapter = adapter;

            _listContainer.AddView(trendingView);
        }

        public void ShowTrendingMangaResults(IAsyncEnumerable<OneOf<IPagedData<Media>, IAniListError>> mediaEnumerable)
        {
            var trendingView = new SideScrollingList(Activity) { LabelText = "Trending Manga" };

            var adapter = new MediaRecyclerAdapter(Activity, mediaEnumerable,
                BaseRecyclerAdapter.RecyclerCardType.Vertical, MediaViewModel.CreateMediaViewModel)
            {
                LongClickAction = (viewModel, position) =>
                {
                    if (Presenter.GetIsUserAuthenticated())
                    {
                        EditMediaListItemDialog.Create(Activity, Presenter, viewModel.Model,
                            viewModel.Model.MediaListEntry,
                            Presenter.GetLoggedInUser()?.MediaListOptions);
                    }
                },

            };
            adapter.SetHorizontalAdapterCardWidthDip(CardWidth);

            trendingView.RecyclerAdapter = _trendingMangaRecyclerAdapter = adapter;

            _listContainer.AddView(trendingView);
        }

        public void ShowNewAnimeResults(IAsyncEnumerable<OneOf<IPagedData<Media>, IAniListError>> mediaEnumerable)
        {
            var newAnimeView = new SideScrollingList(Activity) {LabelText = "New Anime"};

            var adapter = new MediaRecyclerAdapter(Activity, mediaEnumerable,
                BaseRecyclerAdapter.RecyclerCardType.Vertical, MediaViewModel.CreateMediaViewModel)
            {
                LongClickAction = (viewModel, position) =>
                {
                    if (Presenter.GetIsUserAuthenticated())
                    {
                        EditMediaListItemDialog.Create(Activity, Presenter, viewModel.Model,
                            viewModel.Model.MediaListEntry,
                            Presenter.GetLoggedInUser()?.MediaListOptions);
                    }
                },

            };
            adapter.SetHorizontalAdapterCardWidthDip(CardWidth);

            newAnimeView.RecyclerAdapter = _newAnimeRecyclerAdapter = adapter;

            _listContainer.AddView(newAnimeView);
        }

        public void ShowNewMangaResults(IAsyncEnumerable<OneOf<IPagedData<Media>, IAniListError>> mediaEnumerable)
        {
            var newMangaView = new SideScrollingList(Activity) {LabelText = "New Manga"};

            var adapter = new MediaRecyclerAdapter(Activity, mediaEnumerable,
                BaseRecyclerAdapter.RecyclerCardType.Vertical, MediaViewModel.CreateMediaViewModel)
            {
                LongClickAction = (viewModel, position) =>
                {
                    if (Presenter.GetIsUserAuthenticated())
                    {
                        EditMediaListItemDialog.Create(Activity, Presenter, viewModel.Model,
                            viewModel.Model.MediaListEntry,
                            Presenter.GetLoggedInUser()?.MediaListOptions);
                    }
                },

            };
            adapter.SetHorizontalAdapterCardWidthDip(CardWidth);

            newMangaView.RecyclerAdapter = _newMangaRecyclerAdapter = adapter;

            _listContainer.AddView(newMangaView);
        }

        public void UpdateMediaListItem(AniList.Models.MediaModels.MediaList mediaList)
        {
            if (mediaList.Media?.Type == MediaType.Anime)
            {
                var instance = MediaListFragment.GetInstance(MediaListFragment.AnimeMediaListFragmentName);

                (instance as MediaListFragment)?.UpdateMediaListItem(mediaList);
            }
            else if (mediaList.Media?.Type == MediaType.Manga)
            {
                (MediaListFragment.GetInstance(MediaListFragment.MangaMediaListFragmentName) as MediaListFragment)
                    ?.UpdateMediaListItem(mediaList);
            }

            UpdateMediaListOnAdapters(mediaList);
        }

        public void RemoveMediaListItem(int mediaListId)
        {
            (MediaListFragment.GetInstance(MediaListFragment.AnimeMediaListFragmentName) as MediaListFragment)
                ?.RemoveMediaListItem(mediaListId);
            (MediaListFragment.GetInstance(MediaListFragment.MangaMediaListFragmentName) as MediaListFragment)
                ?.RemoveMediaListItem(mediaListId);

            DeleteMediaListOnAdapters(mediaListId);
        }

        private void UpdateMediaListOnAdapters(AniList.Models.MediaModels.MediaList mediaList)
        {
            AdapterList.ForEach(adapter => {
                var itemPosition =
                    adapter?.Items.FindIndex(x => x?.Model?.Id == mediaList.Media?.Id);

                if (itemPosition >= 0)
                {
                    mediaList.Media.MediaListEntry = mediaList;

                    adapter.ReplaceItem(itemPosition.Value, adapter.CreateViewModelFunc?.Invoke(mediaList.Media));
                }
            });
        }

        private void DeleteMediaListOnAdapters(int mediaListId)
        {
            AdapterList.ForEach(adapter => {
                var itemPosition =
                    adapter?.Items?.FindIndex(x => x?.Model?.MediaListEntry?.Id == mediaListId);

                if (itemPosition >= 0)
                {
                    var item = adapter.Items[itemPosition.Value];
                    item.Model.MediaListEntry = null;

                    adapter.ReplaceItem(itemPosition.Value, adapter.CreateViewModelFunc?.Invoke(item.Model));
                }
            });
            
        }
    }
}
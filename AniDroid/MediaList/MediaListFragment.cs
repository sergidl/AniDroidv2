﻿using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using AndroidX.SwipeRefreshLayout.Widget;
using AndroidX.ViewPager.Widget;
using AniDroidv2.Adapters;
using AniDroidv2.Adapters.MediaAdapters;
using AniDroidv2.Adapters.ViewModels;
using AniDroidv2.AniList;
using AniDroidv2.AniList.Enums.MediaEnums;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.MediaModels;
using AniDroidv2.Base;
using AniDroidv2.Dialogs;
using AniDroidv2.Utils.Comparers;
using Google.Android.Material.Snackbar;
using Google.Android.Material.Tabs;

namespace AniDroidv2.MediaList
{
    public class MediaListFragment : BaseMainActivityFragment<MediaListPresenter>, IMediaListView
    {
        public const string AnimeMediaListFragmentName = "ANIME_MEDIA_LIST_FRAGMENT";
        public const string MangaMediaListFragmentName = "MANGA_MEDIA_LIST_FRAGMENT";
        private const string MediaTypeKey = "MEDIA_TYPE";
        private const string MediaSortKey = "MEDIA_SORT";
        private const string UserIdKey = "USER_ID";
        private const int FilterTextUpdateHandlerMessage = 1;

        private int _userId;
        private MediaType _type;
        private IList<MediaListRecyclerAdapter> _recyclerAdapters;
        private MediaListCollection _collection;
        private MediaListSortComparer.MediaListSortType _currentSort;
        private MediaListSortComparer.SortDirection _currentSortDirection;
        private IMenu _menu;
        private MediaListFilterModel _filterModel;

        private static MediaListFragment _animeListFragmentInstance;
        private static MediaListFragment _mangaListFragmentInstance;
        private Handler _filterTextHandler;

        public override bool HasMenu => true;
        public override string FragmentName {
            get {
                if (_type == MediaType.Anime)
                {
                    return AnimeMediaListFragmentName;
                }

                return _type == MediaType.Manga ? MangaMediaListFragmentName : "";
            }
        }

        public static BaseMainActivityFragment<MediaListPresenter> GetInstance(string fragmentName)
        {
            switch (fragmentName)
            {
                case AnimeMediaListFragmentName:
                    return _animeListFragmentInstance;
                case MangaMediaListFragmentName:
                    return _mangaListFragmentInstance;
                default:
                    return null;
            }
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var typeString = Arguments.GetString(MediaTypeKey);
            _type = AniListEnum.GetEnum<MediaType>(typeString);
            _userId = Arguments.GetInt(UserIdKey);

            if (_type == MediaType.Anime)
            {
                _animeListFragmentInstance = this;
            }
            else if (_type == MediaType.Manga)
            {
                _mangaListFragmentInstance = this;
            }

            _filterTextHandler ??= new Handler(UpdateFilterText);
            _filterModel = new MediaListFilterModel();
        }

        public override void OnResume()
        {
            base.OnResume();

            if (Activity.ToolbarSearch == null)
            {
                return;
            }

            Activity.ToolbarSearch.Text = _filterModel.Title;
            Activity.ToolbarSearch.Visibility = ViewStates.Visible;
            Activity.ToolbarSearch.Hint = "Search lists...";

            Activity.ToolbarSearch.AfterTextChanged -= ToolbarSearchTextChanged;
            Activity.ToolbarSearch.AfterTextChanged += ToolbarSearchTextChanged;
        }

        public override void OnPause()
        {
            base.OnPause();

            // need to manually remove this event since ToolbarSearch exists in the parent context
            Activity.ToolbarSearch.AfterTextChanged -= ToolbarSearchTextChanged;
        }

        private void ToolbarSearchTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            _filterTextHandler.RemoveMessages(FilterTextUpdateHandlerMessage);
            _filterTextHandler.SendEmptyMessageDelayed(FilterTextUpdateHandlerMessage, 500);
        }

        private void UpdateFilterText(Message message)
        {
            if (Activity?.ToolbarSearch?.Text != null && !string.Equals(_filterModel.Title, Activity.ToolbarSearch.Text))
            {
                _filterModel.Title = Activity.ToolbarSearch.Text;
                SetMediaListFilter(_filterModel);
            }
        }

        public static MediaListFragment CreateMediaListFragment(int userId, MediaType type, MediaSort sort = null)
        {
            var frag = new MediaListFragment();
            var bundle = new Bundle(6);
            bundle.PutString(MediaTypeKey, type.Value);
            bundle.PutInt(UserIdKey, userId);
            frag.Arguments = bundle;

            return frag;
        }

        public override void OnError(IAniListError error)
        {
            if (error.StatusCode >= 400 && error.StatusCode <= 403)
            {
                Toast.MakeText(Activity, "Please log in again", ToastLength.Long)?.Show();
                Presenter.AniDroidv2Settings.ClearUserAuthentication();
                Activity.RestartAniDroidv2();
            }
        }

        protected override void SetInstance(BaseMainActivityFragment instance)
        {
        }

        public override void ClearState()
        {
            if (_type == MediaType.Anime)
            {
                _animeListFragmentInstance = null;
            }
            else if (_type == MediaType.Manga)
            {
                _mangaListFragmentInstance = null;
            }
        }

        public override View CreateMainActivityFragmentView(ViewGroup container, Bundle savedInstanceState)
        {
            if (_collection != null)
            {
                return GetMediaListCollectionView();
            }
            
            if (_type == null)
            {
                return LayoutInflater.Inflate(Resource.Layout.View_Error, container, false);
            }

            CreatePresenter(savedInstanceState).GetAwaiter().GetResult();
            Presenter.GetMediaLists(_userId);

            return LayoutInflater.Inflate(Resource.Layout.View_IndeterminateProgressIndicator, container, false);
        }

        public MediaType GetMediaType()
        {
            return _type;
        }

        public void SetCollection(MediaListCollection collection)
        {
            _collection = collection;
            RecreateFragment();
        }

        public void UpdateMediaListItem(AniList.Models.MediaModels.MediaList mediaList)
        {
            foreach (var adapter in _recyclerAdapters)
            {
                if (mediaList?.Media?.Id == null)
                {
                    Activity.Logger.Error("UpdateMediaListItem", "A media list item update was attempted with a null reference.");
                    continue;
                }

                adapter.UpdateMediaListItem(mediaList.Media.Id, mediaList);
            }
        }

        public void ResetMediaListItem(int mediaId)
        {
            foreach (var adapter in _recyclerAdapters)
            {
                adapter.ResetMediaListItem(mediaId);
            }
        }

        public void RemoveMediaListItem(int mediaListId)
        {
            foreach (var adapter in _recyclerAdapters)
            {
                adapter.RemoveMediaListItem(mediaListId);
            }
        }

        public MediaListFilterModel GetMediaListFilter()
        {
            return _filterModel;
        }

        public void SetMediaListFilter(MediaListFilterModel filterModel)
        {
            try
            {
                foreach (var adapter in _recyclerAdapters)
                {
                    adapter.SetFilter(filterModel);
                }

                if (Activity?.ToolbarSearch != null && !string.Equals(_filterModel.Title, Activity.ToolbarSearch.Text))
                {
                    Activity.ToolbarSearch.Text = _filterModel.Title;
                }

                if (_filterModel.IsFilteringActive)
                {
                    if (!_filterModel.FilteringPreviouslyActive)
                    {
                        DisplaySnackbarMessage("List filtering is active", Snackbar.LengthLong);
                    }

                    _filterModel.FilteringPreviouslyActive = true;
                    _menu?.FindItem(Resource.Id.Menu_MediaLists_Filter)?.Icon
                        ?.SetTintList(ColorStateList.ValueOf(Color.LightGreen));
                }
                else
                {
                    if (_filterModel.FilteringPreviouslyActive)
                    {
                        DisplaySnackbarMessage("List filtering is not active", Snackbar.LengthShort);
                    }

                    _filterModel.FilteringPreviouslyActive = false;
                    _menu?.FindItem(Resource.Id.Menu_MediaLists_Filter)?.Icon?.SetTintList(null);
                }
            }
            catch (Exception e)
            {
                Activity?.Logger?.Error("SetMediaListFilter", "Error occurred while setting media list filter", e);
            }
        }

        public void SetMediaListSort(MediaListSortComparer.MediaListSortType sort, MediaListSortComparer.SortDirection direction)
        {
            _currentSort = sort;
            _currentSortDirection = direction;

            Presenter.SetMediaListSortSettings(_type, _currentSort, _currentSortDirection);

            RecreateFragment();
        }

        public override void SetupMenu(IMenu menu)
        {
            menu.Clear();
            var inflater = new MenuInflater(Context);
            inflater.Inflate(Resource.Menu.MediaLists_ActionBar, menu);
            _menu = menu;

            _menu.FindItem(Resource.Id.Menu_MediaLists_Filter)?.Icon
                ?.SetTintList(_filterModel.IsFilteringActive ? ColorStateList.ValueOf(Color.LightGreen) : null);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.Menu_MediaLists_Refresh:
                    _collection = null;
                    RecreateFragment();
                    return true;
                case Resource.Id.Menu_MediaLists_Sort:
                    MediaListSortDialog.Create(Activity, _currentSort, _currentSortDirection, SetMediaListSort);
                    return true;
                case Resource.Id.Menu_MediaLists_Filter:
                    MediaListFilterDialog.Create(Activity, this, _type, Presenter.GetGenres(), Presenter.GetMediaTags());
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private View GetMediaListCollectionView()
        {
            var mediaCollectionView = LayoutInflater.Inflate(Resource.Layout.Fragment_MediaLists, null);
            var pagerAdapter = new FragmentlessViewPagerAdapter();
            _recyclerAdapters = new List<MediaListRecyclerAdapter>();

            var listOrder = GetListOrder();
            var orderedLists = _collection.Lists
                .Where(x => listOrder.FirstOrDefault(y => y.Key == x.Name).Value)
                .OrderBy(x => listOrder.FindIndex(y => y.Key == x.Name)).ToList();

            _currentSort = Presenter.GetMediaListSortType(_type);
            _currentSortDirection = Presenter.GetMediaListSortDirection(_type);

            if (_currentSort != MediaListSortComparer.MediaListSortType.NoSort)
            {
                _collection.Lists.ForEach(list =>
                    list.Entries.Sort(new MediaListSortComparer(_currentSort, _currentSortDirection)));
            }

            foreach (var statusList in orderedLists)
            {
                if (statusList.Entries?.Any() != true)
                {
                    continue;
                }

                var adapter = new MediaListRecyclerAdapter(Activity, statusList, Presenter.GetCardType(),
                    item => MediaListViewModel.CreateViewModel(item, _collection.User.MediaListOptions.ScoreFormat,
                        Presenter.GetDisplayTimeUntilAiringAsCountdown(), Presenter.GetProgressDisplayType(), false,
                        Presenter.GetShowEpisodeAddButtonForRepeatingMedia()),
                    Presenter.GetMediaListItemViewType(), Presenter.GetHighlightPriorityItems(),
                    Presenter.GetUseLongClickForEpisodeAdd(),
                    async (viewModel, callback) =>
                    {
                        if (viewModel.Model.Progress + 1 ==
                            (viewModel.Model.Media.Episodes ?? viewModel.Model.Media.Chapters))
                        {
                            EditMediaListItemDialog.Create(Activity, Presenter, viewModel.Model.Media, viewModel.Model,
                                _collection.User.MediaListOptions, true);
                        }
                        else
                        {
                            await Presenter.IncreaseMediaProgress(viewModel.Model);
                        }

                        callback?.Invoke();
                    })
                {
                    LongClickAction = (viewModel, position) => EditMediaListItemDialog.Create(Activity, Presenter,
                        viewModel.Model.Media, viewModel.Model, _collection.User.MediaListOptions)
                };

                adapter.SetFilter(_filterModel);

                _recyclerAdapters.Add(adapter);
                var listView = LayoutInflater.Inflate(Resource.Layout.View_SwipeRefreshList, null);
                listView.FindViewById<RecyclerView>(Resource.Id.List_RecyclerView).SetAdapter(adapter);
                pagerAdapter.AddView(listView, statusList.Name);

                var swipeRefreshLayout = listView.FindViewById<SwipeRefreshLayout>(Resource.Id.List_SwipeRefreshLayout);

                if (Presenter.GetUseSwipeToRefreshOnMediaLists())
                {
                    swipeRefreshLayout.Enabled = true;
                    swipeRefreshLayout.Refresh += (sender, e) =>
                    {
                        _collection = null;
                        RecreateFragment();
                        if (sender is SwipeRefreshLayout refreshLayout)
                        {
                            refreshLayout.Refreshing = false;
                        }
                    };
                }
                else
                {
                    swipeRefreshLayout.Enabled = false;
                }
            }

            var viewPagerView = mediaCollectionView.FindViewById<ViewPager>(Resource.Id.MediaLists_ViewPager);
            viewPagerView.Adapter = pagerAdapter;
            mediaCollectionView.FindViewById<TabLayout>(Resource.Id.MediaLists_Tabs).SetupWithViewPager(viewPagerView);

            return mediaCollectionView;
        }

        private List<KeyValuePair<string, bool>> GetListOrder()
        {
            var retList = new List<KeyValuePair<string, bool>>();

            if (_type == MediaType.Anime)
            {
                var lists = _collection.User.MediaListOptions?.AnimeList?.SectionOrder?
                                .Union(_collection.User.MediaListOptions.AnimeList.CustomLists ?? new List<string>()) ?? new List<string>();

                if (Presenter.AniDroidv2Settings.AnimeListOrder?.Any() != true)
                {
                    // if we don't have the list order yet, go ahead and store it
                    Presenter.AniDroidv2Settings.AnimeListOrder = lists.Select(x => new KeyValuePair<string, bool>(x, true)).ToList();
                }

                retList = Presenter.AniDroidv2Settings.AnimeListOrder;
            }
            else if (_type == MediaType.Manga)
            {
                var lists = _collection.User.MediaListOptions?.MangaList?.SectionOrder?
                                .Union(_collection.User.MediaListOptions.MangaList.CustomLists ?? new List<string>()) ?? new List<string>();

                if (Presenter.AniDroidv2Settings.MangaListOrder?.Any() != true)
                {
                    Presenter.AniDroidv2Settings.MangaListOrder = lists.Select(x => new KeyValuePair<string, bool>(x, true)).ToList();
                }

                retList = Presenter.AniDroidv2Settings.MangaListOrder;
            }

            return retList;
        }
    }
}
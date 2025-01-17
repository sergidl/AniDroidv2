﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.CoordinatorLayout.Widget;
using AndroidX.RecyclerView.Widget;
using AniDroidv2.Adapters.Base;
using AniDroidv2.Adapters.CharacterAdapters;
using AniDroidv2.Adapters.ForumThreadAdapters;
using AniDroidv2.Adapters.MediaAdapters;
using AniDroidv2.Adapters.StaffAdapters;
using AniDroidv2.Adapters.StudioAdapters;
using AniDroidv2.Adapters.UserAdapters;
using AniDroidv2.Adapters.ViewModels;
using AniDroidv2.AniList.Enums.MediaEnums;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.CharacterModels;
using AniDroidv2.AniList.Models.ForumModels;
using AniDroidv2.AniList.Models.MediaModels;
using AniDroidv2.AniList.Models.StaffModels;
using AniDroidv2.AniList.Models.StudioModels;
using AniDroidv2.AniList.Models.UserModels;
using AniDroidv2.Base;
using AniDroidv2.Dialogs;
using AniDroidv2.MediaList;
using AniDroidv2.Utils;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using OneOf;

namespace AniDroidv2.SearchResults
{
    [Activity(Label = "Search Results", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.KeyboardHidden | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class SearchResultsActivity : BaseAniDroidv2Activity<SearchResultsPresenter>, ISearchResultsView
    {
        private string _searchType;
        private string _searchTerm;
        private BaseRecyclerAdapter.RecyclerCardType _cardType;
        private BaseRecyclerAdapter _adapter;

        [InjectView(Resource.Id.SearchResults_CoordLayout)]
        private CoordinatorLayout _coordLayout;
        [InjectView(Resource.Id.SearchResults_RecyclerView)]
        private RecyclerView _recyclerView;
        [InjectView(Resource.Id.SearchResults_Toolbar)]
        private Toolbar _toolbar;
        [InjectView(Resource.Id.SearchResults_SearchFab)]
        private FloatingActionButton _searchButton;

        public override void OnError(IAniListError error)
        {
            throw new NotImplementedException();
        }

        public void ShowMediaSearchResults(IAsyncEnumerable<OneOf<IPagedData<Media>, IAniListError>> mediaEnumerable)
        {
            _adapter = new MediaRecyclerAdapter(this, mediaEnumerable, _cardType, MediaViewModel.CreateMediaViewModel)
            {
                LongClickAction = (viewModel, position) =>
                {
                    if (Settings?.IsUserAuthenticated == true)
                    {
                        EditMediaListItemDialog.Create(this, Presenter, viewModel.Model,
                            viewModel.Model.MediaListEntry,
                            Settings.LoggedInUser?.MediaListOptions);
                    }
                },

            };

            _recyclerView.SetAdapter(_adapter);
        }

        public void ShowCharacterSearchResults(
            IAsyncEnumerable<OneOf<IPagedData<Character>, IAniListError>> characterEnumerable)
        {
            _recyclerView.SetAdapter(_adapter = new CharacterRecyclerAdapter(this, characterEnumerable, _cardType,
                CharacterViewModel.CreateCharacterViewModel));
        }

        public void ShowStaffSearchResults(IAsyncEnumerable<OneOf<IPagedData<Staff>, IAniListError>> staffEnumerable)
        {
            _recyclerView.SetAdapter(_adapter =
                new StaffRecyclerAdapter(this, staffEnumerable, _cardType, StaffViewModel.CreateStaffViewModel));
        }

        public void ShowUserSearchResults(IAsyncEnumerable<OneOf<IPagedData<User>, IAniListError>> userEnumerable)
        {
            _recyclerView.SetAdapter(
                _adapter = new UserRecyclerAdapter(this, userEnumerable, _cardType, UserViewModel.CreateUserViewModel));
        }

        public void ShowForumThreadSearchResults(
            IAsyncEnumerable<OneOf<IPagedData<ForumThread>, IAniListError>> forumThreadEnumerable)
        {
            _recyclerView.SetAdapter(_adapter = new ForumThreadRecyclerAdapter(this, forumThreadEnumerable,
                ForumThreadViewModel.CreateForumThreadViewModel));
        }

        public void ShowStudioSearchResults(IAsyncEnumerable<OneOf<IPagedData<Studio>, IAniListError>> studioEnumerable)
        {
            _recyclerView.SetAdapter(_adapter =
                new StudioRecyclerAdapter(this, studioEnumerable, StudioViewModel.CreateStudioViewModel));
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

            var mediaAdapter = _adapter as MediaRecyclerAdapter;

            var itemPosition =
                mediaAdapter?.Items.FindIndex(x => x.Model?.Id == mediaList.Media?.Id);

            if (itemPosition == null || mediaList.Media == null)
            {
                return;
            }

            mediaList.Media.MediaListEntry = mediaList;

            mediaAdapter.ReplaceItem(itemPosition.Value, mediaAdapter.CreateViewModelFunc?.Invoke(mediaList.Media));
        }

        public void RemoveMediaListItem(int mediaListId)
        {
            (MediaListFragment.GetInstance(MediaListFragment.AnimeMediaListFragmentName) as MediaListFragment)
                ?.RemoveMediaListItem(mediaListId);
            (MediaListFragment.GetInstance(MediaListFragment.MangaMediaListFragmentName) as MediaListFragment)
                ?.RemoveMediaListItem(mediaListId);

            var mediaAdapter = _adapter as MediaRecyclerAdapter;

            var itemPosition =
                mediaAdapter?.Items.FindIndex(x => x.Model?.MediaListEntry?.Id == mediaListId);

            if (itemPosition == null)
            {
                return;
            }

            var item = mediaAdapter.Items[itemPosition.Value];
            item.Model.MediaListEntry = null;

            mediaAdapter.ReplaceItem(itemPosition.Value, mediaAdapter.CreateViewModelFunc?.Invoke(item.Model));
        }

        public override void DisplaySnackbarMessage(string message, int length)
        {
            Snackbar.Make(_coordLayout, message, length).Show();
        }

        public override async Task OnCreateExtended(Bundle savedInstanceState)
        {
            SetContentView(Resource.Layout.Activity_SearchResults);

            await CreatePresenter(savedInstanceState);

            _cardType = Presenter.AniDroidv2Settings.CardType;

            _searchType = Intent.GetStringExtra(IntentKeys.SearchType);
            _searchTerm = Intent.GetStringExtra(IntentKeys.SearchTerm);

            Presenter.SearchAniList(_searchType, _searchTerm);

            _searchButton.Clickable = true;
            _searchButton.Click -= SearchButtonOnClick;
            _searchButton.Click += SearchButtonOnClick;

            SetupToolbar();
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);

            //_adapter.RefreshAdapter();
        }

        private void SearchButtonOnClick(object sender, EventArgs eventArgs)
        {
            SearchDialog.Create(this, (type, term) =>
            {
                _searchType = type;
                _searchTerm = term;
                Presenter.SearchAniList(type, term);
            }, _searchType, _searchTerm);
        }

        public static void StartActivity(Context context, string type, string term)
        {
            var searchIntent = new Intent(context, typeof(SearchResultsActivity));
            searchIntent.PutExtra(IntentKeys.SearchTerm, term);
            searchIntent.PutExtra(IntentKeys.SearchType, type);
            context.StartActivity(searchIntent);
        }

        #region Toolbar

        private void SetupToolbar()
        {
            SetSupportActionBar(_toolbar);
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_arrow_back_white_24px);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        public override bool MenuItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
                return true;
            }

            return false;
        }

        #endregion

        #region Constants

        public static class IntentKeys
        {
            public const string SearchType = "SEARCH_TYPE";
            public const string SearchTerm = "SEARCH_TERM";
        }

        public static class AniListSearchTypes
        {
            public const string Anime = "Anime";
            public const string Characters = "Characters";
            public const string Staff = "Staff";
            public const string Manga = "Manga";
            public const string Studios = "Studios";
            public const string Users = "Users";
            public const string Forum = "Forum";

            public static string[] AllTypes => new[] { Anime, Manga, Characters, Staff, Studios, Users, Forum };
        }

        #endregion
    }
}
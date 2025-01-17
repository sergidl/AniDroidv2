﻿using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using AniDroidv2.Adapters.Base;
using AniDroidv2.Adapters.MediaAdapters;
using AniDroidv2.Adapters.ViewModels;
using AniDroidv2.AniList.Dto;
using AniDroidv2.AniList.Enums.MediaEnums;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.MediaModels;
using AniDroidv2.Base;
using AniDroidv2.Dialogs;
using AniDroidv2.MediaList;
using OneOf;

namespace AniDroidv2.Browse
{
    public class BrowseFragment : BaseMainActivityFragment<BrowsePresenter>, IBrowseView
    {
        public override string FragmentName => "BROWSE_FRAGMENT";

        private MediaRecyclerAdapter _adapter;
        private BaseRecyclerAdapter.RecyclerCardType _cardType;
        private static BrowseFragment _instance;

        public override bool HasMenu => true;

        public override void OnError(IAniListError error)
        {
            throw new NotImplementedException();
        }

        public void ShowMediaSearchResults(IAsyncEnumerable<OneOf<IPagedData<Media>, IAniListError>> mediaEnumerable)
        {
            var recycler = View.FindViewById<RecyclerView>(Resource.Id.List_RecyclerView);
            recycler.SetAdapter(_adapter = new MediaRecyclerAdapter(Activity, mediaEnumerable, _cardType,
                MediaViewModel.CreateMediaViewModel)
            {
                LongClickAction = (viewModel, position) =>
                {
                    if (Presenter.GetIsUserAuthenticated())
                    {
                        EditMediaListItemDialog.Create(Activity, Presenter, viewModel.Model,
                            viewModel.Model.MediaListEntry,
                            Presenter.GetAuthenticatedUser()?.MediaListOptions);
                    }
                },

            });
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

            var itemPosition =
                _adapter?.Items.FindIndex(x => x.Model?.Id == mediaList.Media?.Id);

            if (itemPosition == null || mediaList.Media == null)
            {
                return;
            }

            mediaList.Media.MediaListEntry = mediaList;

            _adapter.ReplaceItem(itemPosition.Value, _adapter.CreateViewModelFunc?.Invoke(mediaList.Media));
        }

        public void RemoveMediaListItem(int mediaListId)
        {
            (MediaListFragment.GetInstance(MediaListFragment.AnimeMediaListFragmentName) as MediaListFragment)
                ?.RemoveMediaListItem(mediaListId);
            (MediaListFragment.GetInstance(MediaListFragment.MangaMediaListFragmentName) as MediaListFragment)
                ?.RemoveMediaListItem(mediaListId);

            var itemPosition =
                _adapter?.Items.FindIndex(x => x.Model?.MediaListEntry?.Id == mediaListId);

            if (itemPosition == null)
            {
                return;
            }

            var item = _adapter.Items[itemPosition.Value];
            item.Model.MediaListEntry = null;

            _adapter.ReplaceItem(itemPosition.Value, _adapter.CreateViewModelFunc?.Invoke(item.Model));
        }

        protected override void SetInstance(BaseMainActivityFragment instance)
        {
            _instance = instance as BrowseFragment;
        }

        public override void ClearState()
        {
            _instance = null;
        }

        public override View CreateMainActivityFragmentView(ViewGroup container, Bundle savedInstanceState)
        {
            CreatePresenter(savedInstanceState).GetAwaiter().GetResult();
            _cardType = Presenter.AniDroidv2Settings.CardType;

            return LayoutInflater.Inflate(Resource.Layout.View_List, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            var browseModel = new BrowseMediaDto()
            {
                Type = MediaType.Anime,
                Format = MediaFormat.Tv,
                Status = MediaStatus.Releasing,
                Season = MediaSeason.GetFromDate(DateTime.UtcNow),
                Country = MediaCountry.Japan,
                Year = DateTime.Now.Year,
                Sort = new List<MediaSort> { MediaSort.PopularityDesc }
            };
            Presenter.BrowseAniListMedia(browseModel);
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);

            _adapter.RefreshAdapter();
        }

        public override void SetupMenu(IMenu menu)
        {
            menu?.Clear();
            var inflater = new MenuInflater(Context);
            inflater.Inflate(Resource.Menu.BrowseFragment_ActionBar, menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.Menu_Browse_Filter:
                    BrowseFilterDialog.Create(Activity, Presenter);
                    return true;
                case Resource.Id.Menu_Browse_Sort:
                    BrowseSortDialog.Create(Activity, Presenter.GetBrowseDto().Sort.FirstOrDefault(), sort =>
                    {
                        var browseDto = Presenter.GetBrowseDto();
                        browseDto.Sort = new List<MediaSort> {sort};
                        Presenter.BrowseAniListMedia(browseDto);
                    });
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}
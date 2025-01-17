﻿using System;
using System.Collections.Generic;
using Android.OS;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using AniDroidv2.Adapters.TorrentAdapters;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.Base;
using AniDroidv2.Dialogs;
using AniDroidv2.Torrent.NyaaSi;
using OneOf;

namespace AniDroidv2.TorrentSearch
{
    public class TorrentSearchFragment : BaseMainActivityFragment<TorrentSearchPresenter>, ITorrentSearchView
    {
        public override string FragmentName => "TORRENT_SEARCH_FRAGMENT";

        private static TorrentSearchFragment _instance;
        private RecyclerView _recycler;
        private NyaaSiSearchRequest _request;

        public override void OnError(IAniListError error)
        {
            throw new NotImplementedException();
        }

        public void ShowNyaaSiSearchResults(IAsyncEnumerable<OneOf<IPagedData<NyaaSiSearchResult>, IAniListError>> searchEnumerable)
        {
            var adapter = new NyaaSiSearchRecyclerAdapter(Activity, searchEnumerable);
            _recycler?.SetAdapter(adapter);
        }

        protected override void SetInstance(BaseMainActivityFragment instance)
        {
            _instance = instance as TorrentSearchFragment;
        }

        public override void ClearState()
        {
            _instance = null;
        }

        public override View CreateMainActivityFragmentView(ViewGroup container, Bundle savedInstanceState)
        {
            return LayoutInflater.Inflate(Resource.Layout.View_List, container, false);
        }

        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _recycler = view.FindViewById<RecyclerView>(Resource.Id.List_RecyclerView);
            
            await CreatePresenter(savedInstanceState);

            GetSearchFabAction().Invoke();
        }

        public override Action GetSearchFabAction()
        {
            return () => TorrentSearchDialog.Create(Activity, req => { Presenter.SearchNyaaSi(req); _request = req; }, _request);
        }
    }
}
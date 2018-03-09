﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using AniDroid.Adapters.MediaAdapters;
using AniDroid.AniList.Interfaces;
using AniDroid.AniList.Models;
using AniDroid.Base;
using AniDroid.Utils;
using AniDroid.Utils.Interfaces;
using Ninject;

namespace AniDroid.Discover
{
    public class DiscoverFragment : BaseAniDroidFragment<DiscoverPresenter>, IDiscoverView
    {
        public override string FragmentName => "DISCOVER_FRAGMENT";

        protected override IReadOnlyKernel Kernel => new StandardKernel(new ApplicationModule<IDiscoverView, DiscoverFragment>(this));

        private View _view;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = LayoutInflater.Inflate(Resource.Layout.Fragment_Discover, container, false);

            CreatePresenter(savedInstanceState).GetAwaiter().GetResult();

            return _view;
        }

        public override void OnError(IAniListError error)
        {
            throw new NotImplementedException();
        }

        public void ShowTrendingResults(IAsyncEnumerable<IPagedData<Media>> mediaEnumerable)
        {
            var recycler = _view.FindViewById<RecyclerView>(Resource.Id.Discover_TrendingRecyclerView);
            var adapter = new DiscoverMediaRecyclerAdapter(Activity, mediaEnumerable);
            adapter.DataLoaded += (sender, b) => DisplaySnackbarMessage("Trending loaded", Snackbar.LengthShort);
            recycler.SetAdapter(adapter);
        }

        public void ShowNewAnimeResults(IAsyncEnumerable<IPagedData<Media>> mediaEnumerable)
        {
            var recycler = _view.FindViewById<RecyclerView>(Resource.Id.Discover_NewAnimeRecyclerView);
            recycler.SetAdapter(new DiscoverMediaRecyclerAdapter(Activity, mediaEnumerable));
        }

        public void ShowNewMangaResults(IAsyncEnumerable<IPagedData<Media>> mediaEnumerable)
        {
            var recycler = _view.FindViewById<RecyclerView>(Resource.Id.Discover_NewMangaRecyclerView);
            recycler.SetAdapter(new DiscoverMediaRecyclerAdapter(Activity, mediaEnumerable));
        }
    }
}
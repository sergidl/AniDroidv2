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
using OneOf;

namespace AniDroid.Discover
{
    public class DiscoverFragment : BaseAniDroidFragment<DiscoverPresenter>, IDiscoverView
    {
        public override string FragmentName => "DISCOVER_FRAGMENT";

        protected override IReadOnlyKernel Kernel => new StandardKernel(new ApplicationModule<IDiscoverView, DiscoverFragment>(this));

        public override View CreateView(ViewGroup container, Bundle savedInstanceState)
        {
            CreatePresenter(savedInstanceState).GetAwaiter().GetResult();

            return LayoutInflater.Inflate(Resource.Layout.Fragment_Discover, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            Presenter.GetDiscoverLists();
        }

        public override void OnError(IAniListError error)
        {
            throw new NotImplementedException();
        }

        public void ShowTrendingResults(IAsyncEnumerable<OneOf<IPagedData<Media>, IAniListError>> mediaEnumerable)
        {
            var recycler = View.FindViewById<RecyclerView>(Resource.Id.Discover_TrendingRecyclerView);
            recycler.SetAdapter(new DiscoverMediaRecyclerAdapter(Activity, mediaEnumerable));
        }

        public void ShowNewAnimeResults(IAsyncEnumerable<OneOf<IPagedData<Media>, IAniListError>> mediaEnumerable)
        {
            var recycler = View.FindViewById<RecyclerView>(Resource.Id.Discover_NewAnimeRecyclerView);
            recycler.SetAdapter(new DiscoverMediaRecyclerAdapter(Activity, mediaEnumerable));
        }

        public void ShowNewMangaResults(IAsyncEnumerable<OneOf<IPagedData<Media>, IAniListError>> mediaEnumerable)
        {
            var recycler = View.FindViewById<RecyclerView>(Resource.Id.Discover_NewMangaRecyclerView);
            recycler.SetAdapter(new DiscoverMediaRecyclerAdapter(Activity, mediaEnumerable));
        }
    }
}
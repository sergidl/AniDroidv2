﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AniDroid.Adapters.AniListActivityAdapters;
using AniDroid.AniList.Interfaces;
using AniDroid.AniList.Models;
using AniDroid.Base;
using AniDroid.Dialogs;
using AniDroid.Utils;
using AniDroid.Utils.Interfaces;
using Ninject;
using OneOf;

namespace AniDroid.Home
{
    public class HomeFragment : BaseMainActivityFragment<HomePresenter>, IHomeView
    {
        public const string HomeFragmentName = "HOME_FRAGMENT";

        private RecyclerView _recyclerView;
        private SwipeRefreshLayout _swipeRefreshLayout;
        private AniListActivityRecyclerAdapter _recyclerAdapter;
        private bool _isFollowingOnly;
        private bool _isAuthenticated;
        private static HomeFragment _instance;

        public override bool HasMenu => true;
        public override string FragmentName => HomeFragmentName;
        protected override IReadOnlyKernel Kernel => new StandardKernel(new ApplicationModule<IHomeView, HomeFragment>(this));

        public static HomeFragment GetInstance() => _instance;

        protected override void SetInstance(BaseMainActivityFragment instance)
        {
            _instance = instance as HomeFragment;
        }

        public override void ClearState()
        {
            _instance = null;
        }

        public override View CreateMainActivityFragmentView(ViewGroup container, Bundle savedInstanceState)
        {
            var settings = Kernel.Get<IAniDroidSettings>();
            _isAuthenticated = settings.IsUserAuthenticated;
            _isFollowingOnly = _isAuthenticated && !settings.ShowAllAniListActivity;

            CreatePresenter(savedInstanceState).GetAwaiter().GetResult();

            var listView = LayoutInflater.Inflate(Resource.Layout.View_SwipeRefreshList, container, false);
            _recyclerView = listView.FindViewById<RecyclerView>(Resource.Id.List_RecyclerView);
            _swipeRefreshLayout = listView.FindViewById<SwipeRefreshLayout>(Resource.Id.List_SwipeRefreshLayout);

            _recyclerAdapter = _recyclerAdapter != null
                ? new AniListActivityRecyclerAdapter(Activity, _recyclerAdapter)
                : new AniListActivityRecyclerAdapter(Activity, Presenter,
                    Presenter.GetAniListActivity(_isFollowingOnly), Presenter.GetUserId());

            _recyclerView.SetAdapter(_recyclerAdapter);

            if (settings.UseSwipeToRefreshHomeScreen)
            {
                _swipeRefreshLayout.SetEnabled(true);
                _swipeRefreshLayout.Refresh += (sender, e) =>
                {
                    RefreshActivity();
                    _swipeRefreshLayout.Refreshing = false;
                };
            }
            else
            {
                _swipeRefreshLayout.SetEnabled(false);
            }

            return listView;
        }

        public override void SetupMenu(IMenu menu)
        {
            menu.Clear();
            var inflater = new MenuInflater(Context);
            inflater.Inflate(Resource.Menu.Home_ActionBar, menu);

            if (!_isAuthenticated)
            {
                menu.FindItem(Resource.Id.Menu_Home_PostStatus).SetVisible(false);
            }

            SetActivityIcon(menu.FindItem(Resource.Id.Menu_Home_ToggleActivityType));
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.Menu_Home_Refresh:
                    RefreshActivity();
                    return true;
                case Resource.Id.Menu_Home_PostStatus:
                    AniListActivityCreateDialog.CreateNewActivity(Activity, Presenter.CreateStatusActivity);
                    return true;
                case Resource.Id.Menu_Home_ToggleActivityType:
                    _isFollowingOnly = !_isFollowingOnly;
                    _recyclerAdapter = new AniListActivityRecyclerAdapter(Activity, Presenter,
                        Presenter.GetAniListActivity(_isFollowingOnly), Presenter.GetUserId());
                    _recyclerView.SetAdapter(_recyclerAdapter);
                    SetActivityIcon(item);
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public override void OnError(IAniListError error)
        {
            throw new NotImplementedException();
        }

        public void ShowUserActivity(IAsyncEnumerable<OneOf<IPagedData<AniListActivity>, IAniListError>> activityEnumerable, int userId)
        {
            var recycler = View.FindViewById<RecyclerView>(Resource.Id.List_RecyclerView);
            recycler.SetAdapter(_recyclerAdapter = new AniListActivityRecyclerAdapter(Activity, Presenter, activityEnumerable, userId));
        }

        public void ShowAllActivity(IAsyncEnumerable<OneOf<IPagedData<AniListActivity>, IAniListError>> activityEnumerable, int userIUd)
        {
            throw new NotImplementedException();
        }

        public void UpdateActivity(int activityPosition, AniListActivity activity)
        {
            _recyclerAdapter.Items[activityPosition] = activity;
            _recyclerAdapter.NotifyItemChanged(activityPosition);
        }

        public void RemoveActivity(int activityPosition)
        {
            _recyclerAdapter.RemoveItem(activityPosition);
        }

        public void RefreshActivity()
        {
            _recyclerAdapter?.ResetAdapter();
        }

        private void SetActivityIcon(IMenuItem menuItem)
        {
            if (!_isAuthenticated)
            {
                menuItem.SetVisible(false);
            }
            else
            {
                if (_isFollowingOnly)
                {
                    menuItem.SetIcon(Resource.Drawable.svg_person);
                    menuItem.SetTitle("Show Public Activity");
                }
                else
                {
                    menuItem.SetIcon(Resource.Drawable.ic_group_white_24px);
                    menuItem.SetTitle("Show Personal Activity");
                }
            }
        }
    }
}
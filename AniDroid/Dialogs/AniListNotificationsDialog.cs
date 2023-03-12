using System;
using System.Collections.Generic;
using Android.Graphics;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using AniDroidv2.Adapters.UserAdapters;
using AniDroidv2.Adapters.ViewModels;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.ActivityModels;
using AniDroidv2.Base;
using OneOf;

namespace AniDroidv2.Dialogs
{
    public static class AniListNotificationsDialog
    {
        public static void Create(BaseAniDroidv2Activity context, IAsyncEnumerable<OneOf<IPagedData<AniListNotification>, IAniListError>> enumerable, int unreadCount, Action dataLoadedAction = null)
        {
            var dialogView = context.LayoutInflater.Inflate(Resource.Layout.View_List, null);
            dialogView.SetBackgroundColor(Color.Transparent);
            var recycler = dialogView.FindViewById<RecyclerView>(Resource.Id.List_RecyclerView);
            var adapter =
                new AniListNotificationRecyclerAdapter(
                    context,
                    enumerable,
                    unreadCount,
                    viewModel => AniListNotificationViewModel.CreateViewModel(viewModel, context, new Color(context.GetThemedColor(Resource.Attribute.Primary)))
                )
                {
                    LoadingItemBackgroundColor = Color.Transparent
                };
            adapter.DataLoaded += (sender, b) => dataLoadedAction?.Invoke();
            recycler.SetAdapter(adapter);
            var dialog = new AlertDialog.Builder(context, context.GetThemedResourceId(Resource.Attribute.Dialog_Theme))
                .SetView(dialogView)
                .Create();

            dialog.Show();
        }
    }
}
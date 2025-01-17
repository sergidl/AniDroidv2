﻿using System;
using System.Collections.Generic;
using System.Linq;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using AniDroidv2.Adapters.Base;
using AniDroidv2.Adapters.MediaAdapters;
using AniDroidv2.Base;
using AniDroidv2.Utils.Extensions;

namespace AniDroidv2.Dialogs
{
    public class MediaListTabOrderDialog
    {
        public static void Create(BaseAniDroidv2Activity context, List<KeyValuePair<string, bool>> mediaListTabs, Action<List<KeyValuePair<string, bool>>> onDismissAction)
        {
            var view = context.LayoutInflater.Inflate(Resource.Layout.View_List, null);
            var recyclerView = view.FindViewById<RecyclerView>(Resource.Id.List_RecyclerView);
            var adapter = new MediaListTabOrderRecyclerAdapter(context,
                mediaListTabs.Select(x => new BaseRecyclerAdapter.StableIdItem<KeyValuePair<string, bool>>(x))
                    .ToList());

            recyclerView.SetAdapter(adapter);
            recyclerView.SetLayoutManager(new LinearLayoutManager(context));

            var helper = recyclerView.AddDragAndDropSupport();

            var dialog = new AlertDialog.Builder(context, context.GetThemedResourceId(Resource.Attribute.Dialog_Theme)).Create();
            dialog.SetView(view);
            dialog.SetCancelable(true);
            dialog.Show();

            dialog.DismissEvent += (sender, e) => { onDismissAction.Invoke(adapter.Items.Select(x => x.Item).ToList()); };
        }

    }
}
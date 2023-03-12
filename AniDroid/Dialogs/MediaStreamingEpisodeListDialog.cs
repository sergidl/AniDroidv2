using System.Collections.Generic;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using AniDroidv2.Adapters.MediaAdapters;
using AniDroidv2.AniList.Models.MediaModels;
using AniDroidv2.Base;

namespace AniDroidv2.Dialogs
{
    public class MediaStreamingEpisodeListDialog
    {
        public static void Create(BaseAniDroidv2Activity context, List<MediaStreaming> streamingEpisodes)
        {
            var dialogView = context.LayoutInflater.Inflate(Resource.Layout.View_List, null);
            dialogView.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.MatchParent);
            var dialogRecycler = dialogView.FindViewById<RecyclerView>(Resource.Id.List_RecyclerView);
            var recyclerAdapter = new MediaStreamingEpisodesRecyclerAdapter(context, streamingEpisodes);
            dialogRecycler.SetAdapter(recyclerAdapter);

            var dialog = new AlertDialog.Builder(context, context.GetThemedResourceId(Resource.Attribute.Dialog_Theme));
            dialog.SetView(dialogView);
            dialog.Show();
        }
    }
}
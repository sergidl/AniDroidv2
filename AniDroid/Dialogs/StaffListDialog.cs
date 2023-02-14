using System.Collections.Generic;
using System.Linq;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using AniDroidv2.Adapters.Base;
using AniDroidv2.Adapters.StaffAdapters;
using AniDroidv2.Adapters.ViewModels;
using AniDroidv2.AniList.Models.StaffModels;
using AniDroidv2.Base;

namespace AniDroidv2.Dialogs
{
    public class StaffListDialog
    {
        public static void Create(BaseAniDroidv2Activity context, ICollection<Staff> staff)
        {
            var dialogView = context.LayoutInflater.Inflate(Resource.Layout.View_List, null);
            dialogView.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.MatchParent);
            var dialogRecycler = dialogView.FindViewById<RecyclerView>(Resource.Id.List_RecyclerView);
            var recyclerAdapter = new StaffRecyclerAdapter(context,
                staff.Select(StaffViewModel.CreateStaffViewModel).ToList(),
                BaseRecyclerAdapter.RecyclerCardType.FlatHorizontal);
            dialogRecycler.SetAdapter(recyclerAdapter);

            var dialog = new AlertDialog.Builder(context,
                context.GetThemedResourceId(Resource.Attribute.Dialog_Theme));
            dialog.SetView(dialogView);
            dialog.Show();
        }
    }
}
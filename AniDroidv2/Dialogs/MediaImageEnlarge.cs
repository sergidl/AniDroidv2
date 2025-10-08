using System.Collections.Generic;
using System.Linq;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AniDroidv2.AniList.Models.MediaModels;
using AniDroidv2.Base;
using AniDroidv2.Utils;

namespace AniDroidv2.Dialogs
{
    public class MediaImageEnlarge
	{
		public static void Create(BaseAniDroidv2Activity context, string imageUrl)
		{
			var dialogView = context.LayoutInflater.Inflate(Resource.Layout.Dialog_MediaImageEnlarge, null);
			var imageView = dialogView.FindViewById<ImageView>(Resource.Id.MediaImageEnlarge_Image);
			ImageLoader.LoadImage(imageView, imageUrl);
			var dialog = new AlertDialog.Builder(context, context.GetThemedResourceId(Resource.Attribute.Dialog_Theme));
			dialog.SetView(dialogView);
			dialog.Show();
		}
	}

}
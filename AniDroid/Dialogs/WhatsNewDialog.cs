﻿using System;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AniDroidv2.Base;

namespace AniDroidv2.Dialogs
{
    public static class WhatsNewDialog
    {
        public static void Create(BaseAniDroidv2Activity context)
        {
            var textView = new TextView(context) {Text = "", LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent)};
            textView.SetPadding(15, 5, 15, 5);

            var whatsNewVals = context.Resources.GetStringArray(Resource.Array.Application_WhatsNew);
            var whatsNewSplitVals = whatsNewVals
                .Select(x =>
                    new {Date = DateTime.TryParseExact(x.Split('|')[0], "MM-dd-yyyy", null, System.Globalization.DateTimeStyles.None, out var parsedDate) ? parsedDate : DateTime.Now, Val = x.Split('|')[1]})
                .GroupBy(x => x.Date).OrderByDescending(x => x.Key);
            foreach (var date in whatsNewSplitVals)
            {
                var str = $"{date.Key.ToShortDateString()}:\n\n{string.Join("\n", date.Select(x => " - " + x.Val))}";
                str += "\n\n\n";
                textView.Text += str;
            }

            var view = context.LayoutInflater.Inflate(Resource.Layout.View_ScrollLayout, null);
            view.SetBackgroundColor(Color.Transparent);
            view.FindViewById<LinearLayout>(Resource.Id.Scroll_Container).AddView(textView);

            var dialog = new AlertDialog.Builder(context,
                context.GetThemedResourceId(Resource.Attribute.Dialog_Theme));
            dialog.SetCancelable(true);
            dialog.SetView(view);
            dialog.SetTitle("What's New?");
            var shownDialog = dialog.Create();
            shownDialog.SetButton((int)DialogButtonType.Positive, "Close", (send, args) => shownDialog.Dismiss());
            shownDialog.Show();
        }
    }
}
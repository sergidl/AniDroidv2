﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AniDroid.Adapters.Base;
using AniDroid.AniList;
using AniDroid.AniList.Interfaces;
using AniDroid.AniList.Models;
using AniDroid.AniListObject.Media;
using AniDroid.Base;

namespace AniDroid.Adapters.MediaAdapters
{
    public class MediaRelationsRecyclerAdapter : BaseRecyclerAdapter<Media.Edge>
    {
        public MediaRelationsRecyclerAdapter(BaseAniDroidActivity context, List<Media.Edge> items, CardType cardType, int verticalCardColumns = 2) : base(context, items, cardType, verticalCardColumns)
        {
        }

        public override void BindCardViewHolder(CardItem holder, int position)
        {
            var item = Items[position];

            holder.Name.Text = item.Node?.Title?.UserPreferred;
            holder.DetailPrimary.Text = $"{item.Node?.Format?.DisplayValue}{(item.Node?.IsAdult == true ? " (Hentai)" : "")}";
            holder.DetailSecondary.Text = item.RelationType?.DisplayValue;
            Context.LoadImage(holder.Image, item.Node?.CoverImage?.Large ?? "");

            holder.ContainerCard.SetTag(Resource.Id.Object_Position, position);
            holder.ContainerCard.Click -= RowClick;
            holder.ContainerCard.Click += RowClick;
        }

        private void RowClick(object sender, EventArgs e)
        {
            var senderView = sender as View;
            var mediaPos = (int)senderView.GetTag(Resource.Id.Object_Position);
            var mediaEdge = Items[mediaPos];

            MediaActivity.StartActivity(Context, mediaEdge.Node.Id, BaseAniDroidActivity.ObjectBrowseRequestCode);
        }
    }
}
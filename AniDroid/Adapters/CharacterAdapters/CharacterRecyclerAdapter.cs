﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using AniDroid.Adapters.Base;
using AniDroid.Adapters.ViewModels;
using AniDroid.AniList.Interfaces;
using AniDroid.AniList.Models;
using AniDroid.AniListObject.Character;
using AniDroid.Base;
using OneOf;

namespace AniDroid.Adapters.CharacterAdapters
{
    public class CharacterRecyclerAdapter : AniDroidRecyclerAdapter<CharacterViewModel, Character>
    {
        public CharacterRecyclerAdapter(BaseAniDroidActivity context,
            IAsyncEnumerable<OneOf<IPagedData<Character>, IAniListError>> enumerable, RecyclerCardType cardType,
            Func<Character, CharacterViewModel> createViewModelFunc) : base(context, enumerable, cardType,
            createViewModelFunc)
        {
            ClickAction = viewModel =>
                CharacterActivity.StartActivity(Context, viewModel.Model.Id,
                    BaseAniDroidActivity.ObjectBrowseRequestCode);
        }

        public override void BindCardViewHolder(CardItem holder, int position)
        {
            var viewModel = Items[position];

            holder.Name.Text = viewModel.TitleText;
            holder.DetailPrimary.Text = viewModel.DetailPrimaryText;

            holder.Button.Visibility = viewModel.Model.IsFavourite ? ViewStates.Visible : ViewStates.Gone;
            Context.LoadImage(holder.Image, viewModel.ImageUri);
        }

        public override CardItem SetupCardItemViewHolder(CardItem item)
        {
            item.Button.Clickable = false;
            item.ButtonIcon.SetImageResource(Resource.Drawable.ic_favorite_white_24px);
            ImageViewCompat.SetImageTintList(item.ButtonIcon, FavoriteIconColor);

            item.DetailSecondary.Visibility = ViewStates.Gone;

            return item;
        }
    }
}
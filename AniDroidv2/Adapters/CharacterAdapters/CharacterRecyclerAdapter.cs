using System;
using System.Collections.Generic;
using AndroidX.Core.Widget;
using AniDroidv2.Adapters.Base;
using AniDroidv2.Adapters.ViewModels;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.CharacterModels;
using AniDroidv2.AniListObject.Character;
using AniDroidv2.Base;
using OneOf;

namespace AniDroidv2.Adapters.CharacterAdapters
{
    public class CharacterRecyclerAdapter : AniDroidv2RecyclerAdapter<CharacterViewModel, Character>
    {
        public CharacterRecyclerAdapter(BaseAniDroidv2Activity context,
            IAsyncEnumerable<OneOf<IPagedData<Character>, IAniListError>> enumerable, RecyclerCardType cardType,
            Func<Character, CharacterViewModel> createViewModelFunc) : base(context, enumerable, cardType,
            createViewModelFunc)
        {
            ClickAction = (viewModel, position) =>
                CharacterActivity.StartActivity(Context, viewModel.Model.Id,
                    BaseAniDroidv2Activity.ObjectBrowseRequestCode);
        }

        public override CardItem SetupCardItemViewHolder(CardItem item)
        {
            item.ButtonIcon.SetImageResource(Resource.Drawable.ic_favorite_white_24px);
            ImageViewCompat.SetImageTintList(item.ButtonIcon, FavoriteIconColor);

            return item;
        }
    }
}
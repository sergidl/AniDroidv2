using System;
using System.Collections.Generic;
using AniDroidv2.Adapters.Base;
using AniDroidv2.Adapters.ViewModels;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.CharacterModels;
using AniDroidv2.AniListObject.Character;
using AniDroidv2.Base;
using OneOf;

namespace AniDroidv2.Adapters.CharacterAdapters
{
    public class CharacterEdgeRecyclerAdapter : AniDroidv2RecyclerAdapter<CharacterEdgeViewModel, CharacterEdge>
    {
        public int ButtonIconResourceId { get; set; }

        public CharacterEdgeRecyclerAdapter(BaseAniDroidv2Activity context,
            IAsyncEnumerable<OneOf<IPagedData<CharacterEdge>, IAniListError>> enumerable, RecyclerCardType cardType,
            Func<CharacterEdge, CharacterEdgeViewModel> createViewModelFunc) : base(context, enumerable, cardType,
            createViewModelFunc)
        {
            ClickAction = (viewModel, position) =>
                CharacterActivity.StartActivity(Context, viewModel.Model.Node.Id,
                    BaseAniDroidv2Activity.ObjectBrowseRequestCode);
        }
    }
}
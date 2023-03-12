using System;
using System.Collections.Generic;
using AniDroidv2.Adapters.Base;
using AniDroidv2.Adapters.ViewModels;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.MediaModels;
using AniDroidv2.AniListObject.Media;
using AniDroidv2.Base;
using Google.Android.Material.Snackbar;
using OneOf;

namespace AniDroidv2.Adapters.MediaAdapters
{
    public class MediaEdgeRecyclerAdapter : AniDroidv2RecyclerAdapter<MediaEdgeViewModel, MediaEdge>
    {
        public MediaEdgeRecyclerAdapter(BaseAniDroidv2Activity context,
            IAsyncEnumerable<OneOf<IPagedData<MediaEdge>, IAniListError>> enumerable, RecyclerCardType cardType,
            Func<MediaEdge, MediaEdgeViewModel> createViewModelFunc) : base(context, enumerable, cardType,
            createViewModelFunc)
        {
            SetDefaultClickActions();
        }

        public MediaEdgeRecyclerAdapter(BaseAniDroidv2Activity context, List<MediaEdgeViewModel> items,
            RecyclerCardType cardType) : base(context, items, cardType)
        {
            SetDefaultClickActions();
        }

        private void SetDefaultClickActions()
        {
            ClickAction = (viewModel, position) =>
                MediaActivity.StartActivity(Context, viewModel.Model?.Node?.Id ?? 0, BaseAniDroidv2Activity.ObjectBrowseRequestCode);

            LongClickAction = (viewModel, position) =>
                Context.DisplaySnackbarMessage(viewModel.Model?.Node?.Title?.UserPreferred, Snackbar.LengthLong);
        }
    }
}
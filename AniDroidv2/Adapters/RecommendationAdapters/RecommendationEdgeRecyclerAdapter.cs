using System;
using System.Collections.Generic;
using AniDroidv2.Adapters.Base;
using AniDroidv2.Adapters.ViewModels;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models;
using AniDroidv2.AniList.Models.RecommendationModels;
using AniDroidv2.AniListObject.Media;
using AniDroidv2.Base;
using Google.Android.Material.Snackbar;
using OneOf;

namespace AniDroidv2.Adapters.RecommendationAdapters
{
    public class RecommendationEdgeRecyclerAdapter : AniDroidv2RecyclerAdapter<RecommendationEdgeViewModel,ConnectionEdge<Recommendation>>
    {
        public RecommendationEdgeRecyclerAdapter(BaseAniDroidv2Activity context, IAsyncEnumerable<OneOf<IPagedData<ConnectionEdge<Recommendation>>, IAniListError>> enumerable, RecyclerCardType cardType, Func<ConnectionEdge<Recommendation>, RecommendationEdgeViewModel> createViewModelFunc) : base(context, enumerable, cardType, createViewModelFunc)
        {
            SetDefaultClickActions();

            ValidateItemFunc = rec => rec.Node?.MediaRecommendation != null;
        }

        private void SetDefaultClickActions()
        {
            ClickAction = (viewModel, position) =>
                MediaActivity.StartActivity(Context, viewModel.Model.Node.MediaRecommendation.Id, BaseAniDroidv2Activity.ObjectBrowseRequestCode);

            LongClickAction = (viewModel, position) =>
                Context.DisplaySnackbarMessage(viewModel.Model.Node.MediaRecommendation.Title?.UserPreferred, Snackbar.LengthLong);
        }
    }
}
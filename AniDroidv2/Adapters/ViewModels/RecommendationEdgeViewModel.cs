using System.Linq;
using AniDroidv2.AniList.Models;
using AniDroidv2.AniList.Models.RecommendationModels;

namespace AniDroidv2.Adapters.ViewModels
{
    public class RecommendationEdgeViewModel : AniDroidv2AdapterViewModel<ConnectionEdge<Recommendation>>
    {
        private RecommendationEdgeViewModel(ConnectionEdge<Recommendation> model, RecommendationDetailType primaryRecommendationDetailType, RecommendationDetailType secondaryRecommendationDetailType) : base(model)
        {
            TitleText = Model.Node.MediaRecommendation.Title?.UserPreferred;
            DetailPrimaryText = GetDetail(primaryRecommendationDetailType);
            DetailSecondaryText = GetDetail(secondaryRecommendationDetailType);
            ImageUri = Model.Node.MediaRecommendation.CoverImage?.Large ?? Model.Node.MediaRecommendation.CoverImage?.Medium;
        }

        public enum RecommendationDetailType
        {
            None,
            Genres,
            Rating
        }

        public static RecommendationEdgeViewModel CreateRecommendationViewModel(ConnectionEdge<Recommendation> model)
        {
            return new RecommendationEdgeViewModel(model, RecommendationDetailType.Genres, RecommendationDetailType.Rating);
        }

        private string GetDetail(RecommendationDetailType recommendationDetailType)
        {
            var retString = recommendationDetailType switch
            {
                RecommendationDetailType.Genres => (Model.Node.MediaRecommendation.Genres?.Any() == true
                    ? string.Join(", ", Model.Node.MediaRecommendation.Genres)
                    : "(No Genres)"),
                RecommendationDetailType.Rating => $"Rating: {Model.Node.Rating:+#;-#;0}",
                _ => null
            };

            return retString;
        }
    }
}
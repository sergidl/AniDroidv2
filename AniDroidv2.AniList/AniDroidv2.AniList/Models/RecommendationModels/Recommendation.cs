using AniDroidv2.AniList.Models.MediaModels;
using AniDroidv2.AniList.Models.UserModels;

namespace AniDroidv2.AniList.Models.RecommendationModels
{
    public class Recommendation : AniListObject
    {
        public int Rating { get; set; }
        public Media Media { get; set; }
        public Media MediaRecommendation { get; set; }
        public User User { get; set; }
    }
}

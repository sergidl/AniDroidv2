using System.Collections.Generic;

namespace AniDroidv2.AniList.Models.UserModels
{
    public class Manga
    {
        public int Count { get; set; }
        public int ChaptersRead { get; set; }
        public List<AniListScoreDistribution> Scores { get; set; }
        public List<AniListStatusDistribution> Statuses{ get; set; }
    }
}

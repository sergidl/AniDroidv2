using System.Collections.Generic;

namespace AniDroidv2.AniList.Models.UserModels
{
    public class Anime
    {
        public int Count { get; set; }
        public int MinutesWatched { get; set; }
        public List<AniListScoreDistribution> Scores { get; set; }
        public List<AniListStatusDistribution> Statuses { get; set; }
    }
}

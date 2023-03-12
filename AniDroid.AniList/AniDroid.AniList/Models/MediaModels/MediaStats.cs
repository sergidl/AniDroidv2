using System.Collections.Generic;
using System.Linq;

namespace AniDroidv2.AniList.Models.MediaModels
{
    public class MediaStats
    {
        public List<AniListScoreDistribution> ScoreDistribution { get; set; }
        public List<AniListStatusDistribution> StatusDistribution { get; set; }
        public List<MediaAiringProgression> AiringProgression { get; set; }

        public bool AreStatsValid()
        {
            return ScoreDistribution?.Count(x => x.Count > 0) >= 3 || AiringProgression?.Count >= 3 ||
                   StatusDistribution?.Any(x => x.Count >= 3) == true;
        }
    }
}

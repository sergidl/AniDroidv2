using System.Collections.Generic;
using AniDroidv2.AniList.DataTypes;
using AniDroidv2.AniList.Enums.MediaEnums;

namespace AniDroidv2.AniList.Dto
{
    public class MediaListEditDto
    {
        public int MediaId { get; set; }
        public MediaListStatus Status { get; set; }
        public float? Score { get; set; }
        public int? Progress { get; set; }
        public int? ProgressVolumes { get; set; }
        public int? Repeat { get; set; }
        public string Notes { get; set; }
        public bool? Private { get; set; }
        public int? Priority { get; set; }
        public ICollection<string> CustomLists { get; set; }
        public bool? HiddenFromStatusLists { get; set; }
        public FuzzyDate StartDate { get; set; }
        public FuzzyDate FinishDate { get; set; }
        public List<float?> AdvancedScores { get; set; }
    }
}

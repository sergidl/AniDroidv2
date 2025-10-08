using System.Collections.Generic;
using AniDroidv2.AniList.Models.UserModels;

namespace AniDroidv2.AniList.Models.MediaModels
{
    public class MediaListCollection
    {
        public List<MediaListGroup> Lists { get; set; }
        public User User { get; set; }

    }
}
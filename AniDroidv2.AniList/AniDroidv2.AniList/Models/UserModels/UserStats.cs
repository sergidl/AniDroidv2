using System.Collections.Generic;

namespace AniDroidv2.AniList.Models.UserModels
{
    public class UserStats
    {
        public List<UserActivityHistory> ActivityHistory { get; set; }
        public Anime Anime { get; set; }
        public Manga Manga { get; set; }

    }
}

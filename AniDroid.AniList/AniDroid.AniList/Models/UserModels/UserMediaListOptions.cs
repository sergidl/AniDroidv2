﻿using AniDroidv2.AniList.Enums.UserEnums;

namespace AniDroidv2.AniList.Models.UserModels
{
    public class UserMediaListOptions
    {
        public ScoreFormat ScoreFormat { get; set; }
        public string RowOrder { get; set; }
        public bool UseLegacyLists { get; set; }
        public MediaListTypeOptions AnimeList { get; set; }
        public MediaListTypeOptions MangaList { get; set; }
    }
}
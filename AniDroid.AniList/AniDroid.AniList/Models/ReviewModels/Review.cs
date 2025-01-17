﻿using AniDroidv2.AniList.Enums.MediaEnums;
using AniDroidv2.AniList.Enums.ReviewEnums;
using AniDroidv2.AniList.Models.MediaModels;
using AniDroidv2.AniList.Models.UserModels;

namespace AniDroidv2.AniList.Models.ReviewModels
{
    public class Review : AniListObject
    {
        public int UserId { get; set; }
        public int MediaId { get; set; }
        public MediaType MediaType { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        public int Rating { get; set; }
        public int RatingAmount { get; set; }
        public ReviewRating UserRating { get; set; }
        public int Score { get; set; }
        public bool Private { get; set; }
        public string SiteUrl { get; set; }
        public int CreatedAt { get; set; }
        public int UpdatedAt { get; set; }
        public User User { get; set; }
        public Media Media { get; set; }
    }
}

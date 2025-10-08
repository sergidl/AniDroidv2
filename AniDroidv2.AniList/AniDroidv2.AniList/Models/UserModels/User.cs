namespace AniDroidv2.AniList.Models.UserModels
{
    public class User : AniListObject
    {
        public string Name { get; set; }
        public string About { get; set; }
        public AniListImage Avatar { get; set; }
        public string BannerImage { get; set; }
        public bool IsFollowing { get; set; }
        public UserOptions Options { get; set; }
        public UserMediaListOptions MediaListOptions { get; set; }
        public UserFavourites Favourites { get; set; }
        public UserStats Statistics { get; set; }
        public int UnreadNotificationCount { get; set; }
        public string SiteUrl { get; set; }
        public int DonatorTier { get; set; }
        public int UpdatedAt { get; set; }
    }
}

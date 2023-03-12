using AniDroidv2.AniList.Models.CharacterModels;
using AniDroidv2.AniList.Models.MediaModels;
using AniDroidv2.AniList.Models.StaffModels;
using AniDroidv2.AniList.Models.StudioModels;

namespace AniDroidv2.AniList.Models.UserModels
{
    public class UserFavourites
    {
        public Connection<MediaEdge, Media> Anime { get; set; }
        public Connection<MediaEdge, Media> Manga { get; set; }
        public Connection<CharacterEdge, Character> Characters { get; set; }
        public Connection<StaffEdge, Staff> Staff { get; set; }
        public Connection<StudioEdge, Studio> Studios { get; set; }
    }
}

using AniDroidv2.AniList.Enums.StaffEnums;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.CharacterModels;
using AniDroidv2.AniList.Models.MediaModels;

namespace AniDroidv2.AniList.Models.StaffModels
{
    public class Staff : AniListObject
    {
        public AniListName Name { get; set; }
        public StaffLanguage Language { get; set; }
        public AniListImage Image { get; set; }
        public string Description { get; set; }
        public bool IsFavourite { get; set; }
        public string SiteUrl { get; set; }
        public Connection<MediaEdge, Media> StaffMedia { get; set; }
        public Connection<CharacterEdge, Character> Characters { get; set; }
        public IPagedData<MediaEdge> Anime { get; set; }
        public IPagedData<MediaEdge> Manga { get; set; }

        #region Internal Classes

        #endregion

        #region Enum Classes

        #endregion
    }
}

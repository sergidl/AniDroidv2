using System.Collections.Generic;
using AniDroidv2.AniList.Enums.CharacterEnums;
using AniDroidv2.AniList.Enums.MediaEnums;
using AniDroidv2.AniList.Models.CharacterModels;
using AniDroidv2.AniList.Models.StaffModels;

namespace AniDroidv2.AniList.Models.MediaModels
{
    public class MediaEdge : ConnectionEdge<Media>
    {
        public MediaRelation RelationType { get; set; }
        public bool IsMainStudio { get; set; }
        public List<Character> Characters { get; set; }
        public CharacterRole CharacterRole { get; set; }
        public string StaffRole { get; set; }
        public List<Staff> VoiceActors { get; set; }
        public int FavouriteOrder { get; set; }
    }
}
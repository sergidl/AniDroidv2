using System.Collections.Generic;
using AniDroidv2.AniList.Enums.CharacterEnums;
using AniDroidv2.AniList.Models.MediaModels;
using AniDroidv2.AniList.Models.StaffModels;

namespace AniDroidv2.AniList.Models.CharacterModels
{
    public class CharacterEdge : ConnectionEdge<Character>
    {
        public CharacterRole Role { get; set; }
        public List<Staff> VoiceActors { get; set; }
        public List<Media> Media { get; set; }
        public int FavouriteOrder { get; set; }
    }
}
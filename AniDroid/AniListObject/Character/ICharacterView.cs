using AniDroidv2.Base;

namespace AniDroidv2.AniListObject.Character
{
    public interface ICharacterView : IAniListObjectView
    {
        int GetCharacterId();
        void SetupCharacterView(AniList.Models.CharacterModels.Character character);
    }
}
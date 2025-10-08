using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AniDroidv2.AniList.Dto;
using AniDroidv2.AniList.Enums.MediaEnums;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.MediaModels;
using AniDroidv2.Base;
using AniDroidv2.Utils.Interfaces;
using AniDroidv2.Utils.Logging;
using OneOf;

namespace AniDroidv2.AniListObject.Character
{
    public class CharacterPresenter : BaseAniDroidv2Presenter<ICharacterView>
    {
        public CharacterPresenter(IAniListService service, IAniDroidv2Settings settings,
            IAniDroidv2Logger logger) : base(service, settings, logger)
        {
        }

        public override async Task Init()
        {
            View.SetLoadingShown();
            var characterId = View.GetCharacterId();
            var characterResp = await AniListService.GetCharacterById(characterId, default(CancellationToken));

            characterResp.Switch(character =>
                {
                    View.SetIsFavorite(character.IsFavourite);
                    View.SetShareText(character.Name?.GetFormattedName(), character.SiteUrl);
                    View.SetContentShown(false);
                    View.SetupToolbar($"{character.Name?.First} {character.Name?.Last}".Trim());
                    View.SetupCharacterView(character);
                })
                .Switch(error => View.OnError(error));
        }

        public IAsyncEnumerable<OneOf<IPagedData<MediaEdge>, IAniListError>> GetCharacterMediaEnumerable(int characterId, MediaType mediaType, int perPage)
        {
            return AniListService.GetCharacterMedia(characterId, mediaType, perPage);
        }

        public async Task ToggleFavorite()
        {
            var characterId = View.GetCharacterId();
            var favResp = await AniListService.ToggleFavorite(new FavoriteDto {CharacterId = characterId},
                default(CancellationToken));

            favResp.Switch(error => View.OnError(error))
                .Switch(favorites => View.SetIsFavorite(favorites.Characters?.Nodes?.Any(x => x.Id == characterId) == true, true));
        }
    }
}
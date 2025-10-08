using System.Collections.Generic;
using System.Threading.Tasks;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.CharacterModels;
using AniDroidv2.AniList.Models.MediaModels;
using AniDroidv2.AniList.Models.StaffModels;
using AniDroidv2.AniList.Models.StudioModels;
using AniDroidv2.Base;
using AniDroidv2.Utils.Interfaces;
using AniDroidv2.Utils.Logging;
using OneOf;

namespace AniDroidv2.Favorites
{
    public class FavoritesPresenter : BaseAniDroidv2Presenter<IFavoritesView>
    {
        public FavoritesPresenter(IAniListService service, IAniDroidv2Settings settings, IAniDroidv2Logger logger) : base(service, settings, logger)
        {
        }

        public override async Task Init()
        {
            View.SetupFavoritesView();
        }

        public IAsyncEnumerable<OneOf<IPagedData<MediaEdge>, IAniListError>> GetUserFavoriteAnimeEnumerable(int userId, int count)
        {
            return AniListService.GetUserFavoriteAnime(userId, 25);
        }

        public IAsyncEnumerable<OneOf<IPagedData<MediaEdge>, IAniListError>> GetUserFavoriteMangaEnumerable(int userId, int count)
        {
            return AniListService.GetUserFavoriteManga(userId, 25);
        }

        public IAsyncEnumerable<OneOf<IPagedData<CharacterEdge>, IAniListError>> GetUserFavoriteCharactersEnumerable(int userId, int count)
        {
            return AniListService.GetUserFavoriteCharacters(userId, 25);
        }

        public IAsyncEnumerable<OneOf<IPagedData<StaffEdge>, IAniListError>> GetUserFavoriteStaffEnumerable(int userId, int count)
        {
            return AniListService.GetUserFavoriteStaff(userId, 25);
        }

        public IAsyncEnumerable<OneOf<IPagedData<StudioEdge>, IAniListError>> GetUserFavoriteStudiosEnumerable(int userId, int count)
        {
            return AniListService.GetUserFavoriteStudios(userId, 25);
        }
    }
}
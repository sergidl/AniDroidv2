using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AniDroidv2.AniList.Dto;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.MediaModels;
using AniDroidv2.Base;
using AniDroidv2.Utils.Interfaces;
using AniDroidv2.Utils.Logging;
using OneOf;

namespace AniDroidv2.AniListObject.Studio
{
    public class StudioPresenter : BaseAniDroidv2Presenter<IStudioView>
    {
        public StudioPresenter(IAniListService service, IAniDroidv2Settings settings,
            IAniDroidv2Logger logger) : base(service, settings, logger)
        {
        }

        public override async Task Init()
        {
            View.SetLoadingShown();
            var studioId = View.GetStudioId();
            var studioResp = await AniListService.GetStudioById(studioId, default(CancellationToken));

            studioResp.Switch(studio =>
                {
                    View.SetIsFavorite(studio.IsFavourite);
                    View.SetShareText(studio.Name, studio.SiteUrl);
                    View.SetContentShown(false);
                    View.SetupToolbar(studio.Name);
                    View.SetupStudioView(studio);
                })
                .Switch(error => View.OnError(error));
        }

        public IAsyncEnumerable<OneOf<IPagedData<MediaEdge>, IAniListError>> GetStudioMediaEnumerable(int studioId, int perPage)
        {
            return AniListService.GetStudioMedia(studioId, perPage);
        }

        public async Task ToggleFavorite()
        {
            var studioId = View.GetStudioId();
            var favResp = await AniListService.ToggleFavorite(new FavoriteDto { StudioId = studioId },
                default(CancellationToken));

            favResp.Switch(error => View.OnError(error))
                .Switch(favorites => View.SetIsFavorite(favorites.Studios?.Nodes?.Any(x => x.Id == studioId) == true, true));
        }
    }
}
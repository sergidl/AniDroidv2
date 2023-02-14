using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AniDroidv2.AniList.Dto;
using AniDroidv2.AniList.Enums.MediaEnums;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Models.CharacterModels;
using AniDroidv2.AniList.Models.MediaModels;
using AniDroidv2.Base;
using AniDroidv2.Utils.Interfaces;
using AniDroidv2.Utils.Logging;
using OneOf;

namespace AniDroidv2.AniListObject.Staff
{
    public class StaffPresenter : BaseAniDroidv2Presenter<IStaffView>
    {
        public StaffPresenter(IAniListService service, IAniDroidv2Settings settings,
            IAniDroidv2Logger logger) : base(service, settings, logger)
        {
        }

        public override async Task Init()
        {
            View.SetLoadingShown();
            var staffId = View.GetStaffId();
            var staffResp = await AniListService.GetStaffById(staffId, default(CancellationToken));

            staffResp.Switch(staff =>
                {
                    View.SetIsFavorite(staff.IsFavourite);
                    View.SetShareText(staff.Name?.GetFormattedName(), staff.SiteUrl);
                    View.SetContentShown(false);
                    View.SetupToolbar($"{staff.Name?.First} {staff.Name?.Last}".Trim());
                    View.SetupStaffView(staff);
                })
                .Switch(error => View.OnError(error));
        }

        public IAsyncEnumerable<OneOf<IPagedData<CharacterEdge>, IAniListError>> GetStaffCharactersEnumerable(int staffId, int perPage)
        {
            return AniListService.GetStaffCharacters(staffId, perPage);
        }

        public IAsyncEnumerable<OneOf<IPagedData<MediaEdge>, IAniListError>> GetStaffMediaEnumerable(int staffId, MediaType mediaType, int perPage)
        {
            return AniListService.GetStaffMedia(staffId, mediaType, perPage);
        }

        public async Task ToggleFavorite()
        {
            var staffId = View.GetStaffId();
            var favResp = await AniListService.ToggleFavorite(new FavoriteDto { StaffId = staffId },
                default(CancellationToken));

            favResp.Switch(error => View.OnError(error))
                .Switch(favorites => View.SetIsFavorite(favorites.Staff?.Nodes?.Any(x => x.Id == staffId) == true, true));
        }
    }
}
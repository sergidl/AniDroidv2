using System.Collections.Generic;
using System.Threading.Tasks;
using AniDroidv2.AniList.Dto;
using AniDroidv2.AniList.Enums.MediaEnums;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.Base;
using AniDroidv2.Utils.Interfaces;
using AniDroidv2.Utils.Logging;

namespace AniDroidv2.CurrentSeason
{
    public class CurrentSeasonPresenter : BaseAniDroidv2Presenter<ICurrentSeasonView>
    {
        private MediaSort _sortType;

        public CurrentSeasonPresenter(IAniListService service, IAniDroidv2Settings settings,
            IAniDroidv2Logger logger) : base(service, settings, logger)
        {
            var titleLanguage = settings?.LoggedInUser?.Options?.TitleLanguage ??
                             MediaTitleLanguage.English;

            if (titleLanguage.Equals(MediaTitleLanguage.Native) ||
                titleLanguage.Equals(MediaTitleLanguage.NativeStylised))
            {
                _sortType = MediaSort.TitleEnglish;
            }
            else if (titleLanguage.Equals(MediaTitleLanguage.Romaji) ||
                     titleLanguage.Equals(MediaTitleLanguage.RomajiStylised))
            {
                _sortType = MediaSort.TitleRomaji;
            }
            else
            {
                _sortType = MediaSort.TitleEnglish;
            }
        }

        public override Task Init()
        {
            return Task.CompletedTask;
        }

        public void GetCurrentSeasonLists()
        {
            View.ShowCurrentTv(AniListService.BrowseMedia(new BrowseMediaDto
            {
                Season = MediaSeason.Fall,
                SeasonYear = 2018,
                Type = MediaType.Anime,
                Format = MediaFormat.Tv,
                Sort = new List<MediaSort> { _sortType }
            }, 5));

        }
    }
}
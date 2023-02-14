using System.Threading.Tasks;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.Base;
using AniDroidv2.Torrent.NyaaSi;
using AniDroidv2.Utils.Interfaces;
using AniDroidv2.Utils.Logging;

namespace AniDroidv2.TorrentSearch
{
    public class TorrentSearchPresenter : BaseAniDroidv2Presenter<ITorrentSearchView>
    {
        public TorrentSearchPresenter(IAniListService service, IAniDroidv2Settings settings,
            IAniDroidv2Logger logger) : base(service, settings, logger)
        {
        }

        public override Task Init()
        {
            return Task.CompletedTask;
        }

        public void SearchNyaaSi(NyaaSiSearchRequest searchReq)
        {
            View.ShowNyaaSiSearchResults(NyaaSiService.GetSearchEnumerable(searchReq));
        }
    }
}
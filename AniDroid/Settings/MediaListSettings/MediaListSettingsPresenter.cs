using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AniDroidv2.Adapters.MediaAdapters;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.Base;
using AniDroidv2.Utils.Comparers;
using AniDroidv2.Utils.Interfaces;
using AniDroidv2.Utils.Logging;

namespace AniDroidv2.Settings.MediaListSettings
{
    public class MediaListSettingsPresenter : BaseAniDroidv2Presenter<IMediaListSettingsView>
    {
        public MediaListSettingsPresenter(IAniListService service, IAniDroidv2Settings settings, IAniDroidv2Logger logger) : base(service, settings, logger)
        {
        }

        public override Task Init()
        {
            if (AniDroidv2Settings.AnimeListOrder?.Any() == true)
            {
                View.CreateAnimeListTabOrderItem(() => AniDroidv2Settings.AnimeListOrder);
            }

            if (AniDroidv2Settings.MangaListOrder?.Any() == true)
            {
                View.CreateMangaListTabOrderItem(() => AniDroidv2Settings.MangaListOrder);
            }

            View.CreateGroupCompletedSettingItem(AniDroidv2Settings.GroupCompletedLists);
            View.CreateMediaListViewTypeSettingItem(AniDroidv2Settings.MediaViewType);
            View.CreateMediaListProgressDisplayItem(AniDroidv2Settings.MediaListProgressDisplay);
            View.CreateAnimeListSortItem(AniDroidv2Settings.AnimeListSortType,
                AniDroidv2Settings.AnimeListSortDirection);
            View.CreateMangaListSortItem(AniDroidv2Settings.MangaListSortType,
                AniDroidv2Settings.MangaListSortDirection);
            View.CreateHighlightPriorityMediaListItemsItem(AniDroidv2Settings.HighlightPriorityMediaListItems);
            View.CreateUseLongClickForEpisodeAddItem(AniDroidv2Settings.UseLongClickForEpisodeAdd);
            View.CreateUseSwipeToRefreshOnMediaListsItem(AniDroidv2Settings.UseSwipeToRefreshOnMediaLists);
            View.CreateShowEpisodeAddButtonForRepeatingMediaItem(AniDroidv2Settings
                .ShowEpisodeAddButtonForRepeatingMedia);
            View.CreateAutoFillDateForMediaListItem(AniDroidv2Settings.AutoFillDateForMediaListItem);

            return Task.CompletedTask;
        }

        public void SetGroupCompleted(bool groupCompleted)
        {
            AniDroidv2Settings.GroupCompletedLists = groupCompleted;
        }

        public void SetMediaListViewType(MediaListRecyclerAdapter.MediaListItemViewType viewType)
        {
            AniDroidv2Settings.MediaViewType = viewType;
        }

        public void SetHighlightPriorityMediaListItems(bool highlightListItems)
        {
            AniDroidv2Settings.HighlightPriorityMediaListItems = highlightListItems;
        }

        public void SetAnimeListTabOrder(List<KeyValuePair<string, bool>> animeLists)
        {
            AniDroidv2Settings.AnimeListOrder = animeLists;
        }

        public void SetMangaListTabOrder(List<KeyValuePair<string, bool>> mangaLists)
        {
            AniDroidv2Settings.MangaListOrder = mangaLists;
        }

        public void SetAnimeListSort(MediaListSortComparer.MediaListSortType sort,
            MediaListSortComparer.SortDirection direction)
        {
            AniDroidv2Settings.AnimeListSortType = sort;
            AniDroidv2Settings.AnimeListSortDirection = direction;
        }

        public void SetMangaListSort(MediaListSortComparer.MediaListSortType sort,
            MediaListSortComparer.SortDirection direction)
        {
            AniDroidv2Settings.MangaListSortType = sort;
            AniDroidv2Settings.MangaListSortDirection = direction;
        }

        public void SetUseLongClickForEpisodeAdd(bool useLongClickForEpisodeAdd)
        {
            AniDroidv2Settings.UseLongClickForEpisodeAdd = useLongClickForEpisodeAdd;
        }

        public void SetMediaListProgressDisplay(
            MediaListRecyclerAdapter.MediaListProgressDisplayType mediaListProgressDisplay)
        {
            AniDroidv2Settings.MediaListProgressDisplay = mediaListProgressDisplay;
        }

        public void SetUseSwipeToRefreshOnMediaLists(bool useSwipeToRefreshOnMediaLists)
        {
            AniDroidv2Settings.UseSwipeToRefreshOnMediaLists = useSwipeToRefreshOnMediaLists;
        }

        public void SetShowEpisodeAddButtonForRepeatingMedia(bool showEpisodeAddButtonForRewatchingAnime)
        {
            AniDroidv2Settings.ShowEpisodeAddButtonForRepeatingMedia = showEpisodeAddButtonForRewatchingAnime;
        }
        
        public void SetAutoFillDateForMediaListItem(bool autoFillDateForNewMediaListItem)
        {
            AniDroidv2Settings.AutoFillDateForMediaListItem = autoFillDateForNewMediaListItem;
        }
    }
}
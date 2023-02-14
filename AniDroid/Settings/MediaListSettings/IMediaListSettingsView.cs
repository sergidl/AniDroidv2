using System;
using System.Collections.Generic;
using AniDroidv2.Adapters.MediaAdapters;
using AniDroidv2.Base;
using AniDroidv2.Utils.Comparers;

namespace AniDroidv2.Settings.MediaListSettings
{
    public interface IMediaListSettingsView : IAniDroidv2View
    {
        void CreateGroupCompletedSettingItem(bool groupCompleted);
        void CreateMediaListViewTypeSettingItem(MediaListRecyclerAdapter.MediaListItemViewType viewType);
        void CreateHighlightPriorityMediaListItemsItem(bool highlightPriorityItems);
        void CreateAnimeListTabOrderItem(Func<List<KeyValuePair<string, bool>>> getAnimeLists);
        void CreateMangaListTabOrderItem(Func<List<KeyValuePair<string, bool>>> getMangaLists);
        void CreateAnimeListSortItem(MediaListSortComparer.MediaListSortType sort, MediaListSortComparer.SortDirection direction);
        void CreateMangaListSortItem(MediaListSortComparer.MediaListSortType sort, MediaListSortComparer.SortDirection direction);
        void CreateUseLongClickForEpisodeAddItem(bool useLongClickForEpisodeAdd);
        void CreateMediaListProgressDisplayItem(MediaListRecyclerAdapter.MediaListProgressDisplayType mediaListProgressDisplay);
        void CreateUseSwipeToRefreshOnMediaListsItem(bool useSwipeToRefreshOnMediaLists);
        void CreateShowEpisodeAddButtonForRepeatingMediaItem(bool showEpisodeAddButtonForRepeatingMedia);
        void CreateAutoFillDateForMediaListItem(bool autoFillDateForMediaListItem);
    }
}
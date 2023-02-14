using System;
using System.Collections.Generic;
using AniDroidv2.Adapters.Base;
using AniDroidv2.Adapters.MediaAdapters;
using AniDroidv2.AniList.Models.MediaModels;
using AniDroidv2.AniList.Models.UserModels;
using AniDroidv2.Base;
using AniDroidv2.Main;
using AniDroidv2.Utils.Comparers;

namespace AniDroidv2.Utils.Interfaces
{
    public interface IAniDroidv2Settings
    {
        #region Unauthenticated Settings

        int HighestVersionUsed { get; set; }
        BaseRecyclerAdapter.RecyclerCardType CardType { get; set; }
        BaseAniDroidv2Activity.AniDroidv2Theme Theme { get; set; }
        bool DisplayBanners { get; set; }
        bool DisplayUpcomingEpisodeTimeAsCountdown { get; set; }
        bool UseSwipeToRefreshHomeScreen { get; set; }
        IList<MediaTag> MediaTagCache { get; set; }
        IList<string> GenreCache { get; set; }

        #endregion

        #region Authenticated Settings

        string UserAccessCode { get; set; }
        bool IsUserAuthenticated { get; }
        void ClearUserAuthentication();
        User LoggedInUser { get; set; }
        bool ShowAllAniListActivity { get; set; }
        bool EnableNotificationService { get; set; }
        MainActivity.DefaultTab DefaultTab { get; set; }

        #endregion

        #region Media List Settings

        List<KeyValuePair<string, bool>> AnimeListOrder { get; set; }
        List<KeyValuePair<string, bool>> MangaListOrder { get; set; }
        bool GroupCompletedLists { get; set; }
        MediaListRecyclerAdapter.MediaListItemViewType MediaViewType { get; set; }
        bool HighlightPriorityMediaListItems { get; set; }
        MediaListSortComparer.MediaListSortType AnimeListSortType { get; set; }
        MediaListSortComparer.SortDirection AnimeListSortDirection { get; set; }
        MediaListSortComparer.MediaListSortType MangaListSortType { get; set; }
        MediaListSortComparer.SortDirection MangaListSortDirection { get; set; }
        bool UseLongClickForEpisodeAdd { get; set; }
        MediaListRecyclerAdapter.MediaListProgressDisplayType MediaListProgressDisplay { get; set; }
        bool UseSwipeToRefreshOnMediaLists { get; set; }
        bool ShowEpisodeAddButtonForRepeatingMedia { get; set; }
        bool AutoFillDateForMediaListItem { get; set; }
        
        #endregion

        #region Old Settings

        [Obsolete("No longer used, use MediaListProgressDisplay instead", true)]
        bool DisplayMediaListItemProgressColors { get; set; }
        [Obsolete("No longer used, use MediaListProgressDisplay instead", true)]
        bool AlwaysDisplayEpisodeProgressColor { get; set; }

        #endregion

        #region Methods

        void UpdateLoggedInUser(User user);
        void UpdateUserMediaListTabs(UserMediaListOptions mediaListOptions);

        #endregion
        
    }
}
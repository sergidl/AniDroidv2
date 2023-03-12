using System.Collections.Generic;
using AniDroidv2.AniList.Dto;
using AniDroidv2.AniList.Models.MediaModels;

namespace AniDroidv2.Browse
{
    public interface IBrowsePresenter
    {
        void BrowseAniListMedia(BrowseMediaDto browseDto);
        BrowseMediaDto GetBrowseDto();
        IList<MediaTag> GetMediaTags();
        IList<string> GetGenres();
    }
}
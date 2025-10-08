using System.Collections.Generic;
using AniDroidv2.AniList.Interfaces;

namespace AniDroidv2.AniList.DataTypes
{
    public class PagedData<T> : IPagedData<T>
    {
        public PageInfo PageInfo { get; set; }
        public ICollection<T> Data { get; set; }
    }
}

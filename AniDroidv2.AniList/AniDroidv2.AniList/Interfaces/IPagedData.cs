using System.Collections.Generic;
using AniDroidv2.AniList.DataTypes;

namespace AniDroidv2.AniList.Interfaces
{
    public interface IPagedData<T>
    {
        PageInfo PageInfo { get; set; }
        ICollection<T> Data { get; set; }
    }
}

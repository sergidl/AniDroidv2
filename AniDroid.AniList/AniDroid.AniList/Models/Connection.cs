using System.Collections.Generic;
using AniDroidv2.AniList.DataTypes;
using AniDroidv2.AniList.Interfaces;
using Newtonsoft.Json;

namespace AniDroidv2.AniList.Models
{
    public class Connection<TEdgeType, TNodeType> : IPagedData<TEdgeType> where TEdgeType : ConnectionEdge<TNodeType>
    {
        [JsonProperty("Edges")]
        public ICollection<TEdgeType> Data { get; set; }
        public ICollection<TNodeType> Nodes { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}

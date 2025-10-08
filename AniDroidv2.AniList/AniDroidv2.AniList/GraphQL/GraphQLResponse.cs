using System.Collections.Generic;

namespace AniDroidv2.AniList.GraphQL
{
    public class GraphQLResponse<T> where T : class
    {
        public Dictionary<string, T> Data { get; set; }
        public List<GraphQLError> Errors { get; set; }
        public T Value => Data?.ContainsKey("Data") == true ? Data["Data"] : null;
    }

    public class GraphQLResponse
    {
        public List<GraphQLError> Errors { get; set; }
    }
}

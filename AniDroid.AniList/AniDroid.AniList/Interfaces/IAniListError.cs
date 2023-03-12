using System;
using System.Collections.Generic;
using AniDroidv2.AniList.GraphQL;

namespace AniDroidv2.AniList.Interfaces
{
    public interface IAniListError
    {
        int StatusCode { get; }
        string ErrorMessage { get; }
        Exception ErrorException { get; }
        List<GraphQLError> GraphQLErrors { get; }
    }
}

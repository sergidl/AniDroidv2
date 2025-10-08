using System;
using System.Collections.Generic;
using AniDroidv2.AniList.GraphQL;
using AniDroidv2.AniList.Interfaces;

namespace AniDroidv2.AniList.Service
{
    public class AniListError : IAniListError
    {
        public AniListError(int statusCode, string errorMessage, Exception errorException, List<GraphQLError> graphQLErrors)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
            ErrorException = errorException;
            GraphQLErrors = graphQLErrors;
        }

        public int StatusCode { get; }
        public string ErrorMessage { get; }
        public Exception ErrorException { get; }
        public List<GraphQLError> GraphQLErrors { get; }
    }
}

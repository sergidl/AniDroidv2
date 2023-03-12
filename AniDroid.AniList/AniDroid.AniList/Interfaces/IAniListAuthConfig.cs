namespace AniDroidv2.AniList.Interfaces
{
    public interface IAniListAuthConfig
    {
        string ClientId { get; }
        string ClientSecret { get; }
        string RedirectUri { get; }
        string AuthTokenUri { get; }
    }
}

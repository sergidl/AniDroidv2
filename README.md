# Build Requirements

In order for the app to build, you will need to create one file not included in the repository. In addition, you will also need to create an API client on AniList (https://anilist.co/settings/developer), if you are not going to change the code use `anidroidv2://login` in Redirect URL

### appsettings.secret.json

This will go in the root of the `AniDroid` project. This file will contain AniList API access data. The following is an example of this file:

```json
{
  "AppCenterId": "be0dcae5-9128-4aae-8201-630fcc99f2bd",
  "ApiConfiguration": {
    "ClientId": "YOUR_ANILIST_CLIENT_ID",
    "ClientSecret": "YOUR_ANILIST_CLIENT_SECRET",
    "RedirectUrl": "anidroidv2://login",
    "AuthUrl": "https://anilist.co/api/v2/oauth/token",
    "BaseUrl": "https://graphql.anilist.co"
  }
}
```
using System.Threading;
using System.Threading.Tasks;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.Base;
using AniDroidv2.Utils.Interfaces;
using AniDroidv2.Utils.Logging;

namespace AniDroidv2.Login
{
    public class LoginPresenter : BaseAniDroidv2Presenter<ILoginView>
    {
        private readonly IAniListAuthConfig _authConfig;

        public LoginPresenter(IAniListService service, IAniDroidv2Settings settings,
            IAniListAuthConfig authConfig, IAniDroidv2Logger logger) : base(service, settings, logger)
        {
            _authConfig = authConfig;
        }

        public override Task Init()
        {
            return Task.CompletedTask;
        }

        public async Task Login(CancellationToken token)
        {
            AniDroidv2Settings.ClearUserAuthentication();
            var authCode = View.GetAuthCode();

            if (string.IsNullOrWhiteSpace(authCode))
            {
                View.OnErrorAuthorizing();
                return;
            }

            var authResp = await AniListService.AuthenticateUser(_authConfig, authCode, token);

            authResp.Switch((IAniListError error) => View.OnErrorAuthorizing())
                .Switch(async auth =>
                {
                    AniDroidv2Settings.UserAccessCode = auth.AccessToken;

                    var currentUser = await AniListService.GetCurrentUser(token);

                    currentUser.Switch((IAniListError error) =>
                    {
                        AniDroidv2Settings.ClearUserAuthentication();
                        View.OnErrorAuthorizing();
                    }).Switch(user =>
                    {
                        AniDroidv2Settings.LoggedInUser = user;
                        View.OnAuthorized();
                    });
                });
        }

        public string GetRedirectUrl(string authUrlTemplate)
        {
            return string.Format(authUrlTemplate, _authConfig.ClientId, _authConfig.RedirectUri);
        }
    }
}
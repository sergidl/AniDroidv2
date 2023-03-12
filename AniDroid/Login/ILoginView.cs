using AniDroidv2.Base;

namespace AniDroidv2.Login
{
    public interface ILoginView : IAniDroidv2View
    {
        string GetAuthCode();
        void OnErrorAuthorizing();
        void OnAuthorized();
    }
}
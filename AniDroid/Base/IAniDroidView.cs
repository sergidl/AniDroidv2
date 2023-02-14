using AniDroidv2.AniList.Interfaces;

namespace AniDroidv2.Base
{
    public interface IAniDroidv2View
    {
        void OnError(IAniListError error);
        void DisplaySnackbarMessage(string message, int length);
        void DisplayNotYetImplemented();
    }
}
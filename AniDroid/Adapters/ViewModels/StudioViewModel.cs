using AniDroidv2.AniList.Models.StudioModels;

namespace AniDroidv2.Adapters.ViewModels
{
    public class StudioViewModel : AniDroidv2AdapterViewModel<Studio>
    {
        public StudioEdge StudioEdge { get; protected set; }

        public StudioViewModel(Studio model, StudioDetailType primaryStudioDetailType, StudioDetailType secondaryStudioDetailType, bool isButtonVisible) : base(model)
        {
            TitleText = Model.Name;
            DetailPrimaryText = GetDetail(primaryStudioDetailType);
            DetailSecondaryText = GetDetail(secondaryStudioDetailType);
            IsButtonVisible = isButtonVisible;
        }

        public enum StudioDetailType
        {
            None,
            IsMainStudio
        }

        public static StudioViewModel CreateStudioViewModel(Studio model)
        {
            return new StudioViewModel(model, StudioDetailType.None, StudioDetailType.None, model.IsFavourite);
        }

        private string GetDetail(StudioDetailType detailType)
        {
            string retString = null;

            if (detailType == StudioDetailType.IsMainStudio)
            {
                retString = StudioEdge?.IsMain == true ? "Main Studio" : "";
            }

            return retString;
        }
    }
}
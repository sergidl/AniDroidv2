﻿using Android.Views;
using AniDroidv2.AniList.Models.StudioModels;

namespace AniDroidv2.Adapters.ViewModels
{
    public class StudioEdgeViewModel : AniDroidv2AdapterViewModel<StudioEdge>
    {
        public override ViewStates ImageVisibility => ViewStates.Gone;

        public StudioEdgeViewModel(StudioEdge model, StudioEdgeDetailType primaryStudioEdgeDetailType,
            StudioEdgeDetailType secondaryStudioEdgeDetailType) : base(model)
        {
            TitleText = Model.Node?.Name;
            DetailPrimaryText = GetDetailString(primaryStudioEdgeDetailType);
            DetailSecondaryText = GetDetailString(secondaryStudioEdgeDetailType);
        }

        public enum StudioEdgeDetailType
        {
            None,
            IsMainStudio
        }

        public static StudioEdgeViewModel CreateStudioEdgeViewModel(StudioEdge model)
        {
            return new StudioEdgeViewModel(model, StudioEdgeDetailType.IsMainStudio, StudioEdgeDetailType.None);
        }

        public static StudioEdgeViewModel CreateFavoriteStudioEdgeViewModel(StudioEdge model)
        {
            return new StudioEdgeViewModel(model, StudioEdgeDetailType.None, StudioEdgeDetailType.None);
        }

        private string GetDetailString(StudioEdgeDetailType detailType)
        {
            string retString = null;

            if (detailType == StudioEdgeDetailType.IsMainStudio)
            {
                retString = Model.IsMain ? "Main Studio" : null;
            }

            return retString;
        }
    }
}
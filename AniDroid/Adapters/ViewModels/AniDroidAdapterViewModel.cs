using Android.Views;
using AniDroidv2.Utils;

namespace AniDroidv2.Adapters.ViewModels
{
    public abstract class AniDroidv2AdapterViewModel<T> where T : class
    {
        public T Model { get; }

        public string TitleText { get; protected set; }
        public string DetailPrimaryText { get; protected set; }
        public string DetailSecondaryText { get; protected set; }
        public string ImageUri { get; protected set; }
        public bool IsButtonVisible { get; protected set; }
        public int? ButtonIcon { get; protected set; }
        public bool LoadImage { get; protected set; }

        public virtual ViewStates TitleVisibility => TitleText != null ? ViewStates.Visible : ViewStates.Gone;
        public virtual ViewStates DetailPrimaryVisibility => DetailPrimaryText != null ? ViewStates.Visible : ViewStates.Gone;
        public virtual ViewStates DetailSecondaryVisibility => DetailSecondaryText != null ? ViewStates.Visible : ViewStates.Gone;
        public virtual ViewStates ButtonVisibility => IsButtonVisible ? ViewStates.Visible : ViewStates.Gone;
        public virtual ViewStates ImageVisibility => ImageUri != null ? ViewStates.Visible : ViewStates.Invisible;

        protected AniDroidv2AdapterViewModel(T model)
        {
            Model = model;
            LoadImage = true;
        }

        public virtual void RecreateViewModel()
        {

        }
    }
}
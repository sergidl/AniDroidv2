using System.Threading.Tasks;
using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;
using AniDroidv2.AniList.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AniDroidv2.Base
{
    public abstract class BaseAniDroidv2Fragment<T> : BaseAniDroidv2Fragment where T : BaseAniDroidv2Presenter
    {
        protected T Presenter { get; set; }

        protected async Task CreatePresenter(Bundle savedInstanceState)
        {
            if (Presenter != null)
            {
                return;
            }

            Presenter = AniDroidv2Application.ServiceProvider.GetService<T>();
            await Presenter.BaseInit(this).ConfigureAwait(false);
        }
    }

    public abstract class BaseAniDroidv2Fragment : Fragment, IAniDroidv2View
    {
        private bool _pendingRecreate;

        protected new BaseAniDroidv2Activity Activity => base.Activity as BaseAniDroidv2Activity;
        protected new LayoutInflater LayoutInflater => Activity.LayoutInflater;

        public abstract string FragmentName { get; }

        public abstract void OnError(IAniListError error);

        public abstract View CreateView(ViewGroup container, Bundle savedInstanceState);

        public void DisplaySnackbarMessage(string message, int length) => Activity?.DisplaySnackbarMessage(message, length);

        public void DisplayNotYetImplemented() => Activity?.DisplayNotYetImplemented();

        public sealed override View OnCreateView(LayoutInflater inflater, ViewGroup container,
            Bundle savedInstanceState) => CreateView(container, savedInstanceState);

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HasOptionsMenu = HasMenu;
        }

        public override void OnResume()
        {
            base.OnResume();

            if (_pendingRecreate)
            {
                _pendingRecreate = false;
                RecreateFragment();
            }
        }

        public virtual bool HasMenu => false;

        public sealed override void OnPrepareOptionsMenu(IMenu menu)
        {
            base.OnPrepareOptionsMenu(menu);
            SetupMenu(menu);
        }

        public virtual void SetupMenu(IMenu menu)
        {
        }

        public void RecreateFragment()
        {
            if (IsDetached)
            {
                return;
            }

            if (IsStateSaved)
            {
                _pendingRecreate = true;
                return;
            }

            FragmentManager.BeginTransaction()
                .Detach(this)
                .Attach(this)
                .Commit();
        }
    }
}

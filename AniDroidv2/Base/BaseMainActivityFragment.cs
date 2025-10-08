using System;
using System.Threading.Tasks;
using Android.OS;
using Android.Views;
using AniDroidv2.Main;
using Microsoft.Extensions.DependencyInjection;

namespace AniDroidv2.Base
{
    public abstract class BaseMainActivityFragment<T> : BaseMainActivityFragment where T : BaseAniDroidv2Presenter
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

    public abstract class BaseMainActivityFragment : BaseAniDroidv2Fragment
    {
        protected new MainActivity Activity => base.Activity as MainActivity;

        protected abstract void SetInstance(BaseMainActivityFragment instance);

        public abstract void ClearState();

        public abstract View CreateMainActivityFragmentView(ViewGroup container, Bundle savedInstanceState);

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetInstance(this);
        }

        public sealed override View CreateView(ViewGroup container, Bundle savedInstanceState)
        {
            Activity.ShowSearchButton();
            return CreateMainActivityFragmentView(container, savedInstanceState);
        }

        public virtual Action GetSearchFabAction() => null;
    }
}
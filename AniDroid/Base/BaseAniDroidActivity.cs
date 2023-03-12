using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.Content;
using AniDroidv2.Adapters.Base;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.Start;
using AniDroidv2.Utils;
using AniDroidv2.Utils.Interfaces;
using AniDroidv2.Utils.Logging;
using Google.Android.Material.Snackbar;
using Square.Picasso;
using Microsoft.Extensions.DependencyInjection;

namespace AniDroidv2.Base
{
    public abstract class BaseAniDroidv2Activity<T> : BaseAniDroidv2Activity, IAniDroidv2View where T : BaseAniDroidv2Presenter
    {
        private const string PresenterStateKey = "KEY_PRESENTER_STATE";
        protected IAniDroidv2View View => this;

        protected T Presenter { get; set; }

        protected async Task CreatePresenter(Bundle savedInstanceState)
        {
            if (GetRetainedPresenter() != null)
            {
                Presenter = GetRetainedPresenter();
                Presenter.View = View;
            }
            else
            {
                Presenter = AniDroidv2Application.ServiceProvider.GetService<T>();
                await Presenter.BaseInit(View).ConfigureAwait(false);
            }
        }

        protected T GetRetainedPresenter()
        {
            var wrapper = (CustomNonConfigurationWrapper<T>) LastCustomNonConfigurationInstance;
            return wrapper?.Target;
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutStringArrayList(PresenterStateKey, Presenter?.SaveState());
        }

        public abstract override void DisplaySnackbarMessage(string message, int length = Snackbar.LengthShort);

        public sealed override void DisplayNotYetImplemented()
        {
            DisplaySnackbarMessage("Not Yet Implemented", Snackbar.LengthShort);
        }

        public sealed override bool OnCreateOptionsMenu(IMenu menu)
        {
            if (HasError)
            {
                MenuInflater.Inflate(Resource.Menu.Error_ActionBar, menu);
                return true;
            }

            return SetupMenu(menu);
        }

        public sealed override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (HasError)
            {
                if (item.ItemId == Resource.Id.Menu_Error_Refresh)
                {
                    Presenter.BaseInit(View).ConfigureAwait(false).GetAwaiter();
                    return true;
                }
            }

            return MenuItemSelected(item);
        }

        public sealed override void Recreate()
        {
            Presenter = null;
            base.Recreate();
        }
    }

    [Activity(Label = "@string/AppName")]
    public abstract class BaseAniDroidv2Activity : AppCompatActivity
    {
        public const int ObjectBrowseRequestCode = 9;

        private static AniDroidv2Theme _theme;
        public IAniDroidv2Settings Settings { get; private set; }
        public IAniDroidv2Logger Logger { get; private set; }
        protected bool HasError { get; set; }
        public sealed override LayoutInflater LayoutInflater => ThemedInflater;
        public BaseRecyclerAdapter.RecyclerCardType CardType { get; private set; }

        #region Overrides

        protected sealed override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Logger = AniDroidv2Application.ServiceProvider.GetService<IAniDroidv2Logger>();
            Settings = AniDroidv2Application.ServiceProvider.GetService<IAniDroidv2Settings>();
            _theme = Settings.Theme;
            CardType = Settings.CardType;
            SetTheme(GetThemeResource());

            await OnCreateExtended(savedInstanceState);
        }

        public sealed override void SetContentView(int layoutResId)
        {
            base.SetContentView(layoutResId);
            Cheeseknife.Inject(this);
        }

        #endregion

        #region Theme

        public override Resources.Theme Theme {
            get
            {
                var theme = GetThemeResource();

                var baseTheme = base.Theme;
                baseTheme.ApplyStyle(theme, true);

                return baseTheme;
            }
        }

        public int GetThemeResource()
        {
            var theme = Resource.Style.AniList;

            switch (_theme)
            {
                case AniDroidv2Theme.Black:
                    theme = Resource.Style.Black;
                    break;
                case AniDroidv2Theme.AniListDark:
                    theme = Resource.Style.AniListDark;
                    break;
                case AniDroidv2Theme.Dark:
                    theme = Resource.Style.Dark;
                    break;
            }

            return theme;
        }

        public int GetThemedResourceId(int attrId)
        {
            var typedVal = new TypedValue();
            Theme.ResolveAttribute(attrId, typedVal, true);
            return typedVal.ResourceId;
        }

        public int GetThemedColor(int attrId)
        {
            var typedVal = new TypedValue();
            Theme.ResolveAttribute(attrId, typedVal, true);
            return new Color(ContextCompat.GetColor(this, typedVal.ResourceId));
        }

        private LayoutInflater ThemedInflater
        {
            get
            {
                using (var contextThemeWrapper = new ContextThemeWrapper(this, GetThemeResource()))
                {
                    return base.LayoutInflater.CloneInContext(contextThemeWrapper);
                }
            }
        }

        public enum AniDroidv2Theme
        {
            AniList = 0,
            Black = 1,
            AniListDark = 2,
            Dark = 3
        }

        #endregion

        #region Context Utils

        public static ISpanned FromHtml(string source)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            return Build.VERSION.SdkInt >= BuildVersionCodes.N ? Html.FromHtml(source ?? "", FromHtmlOptions.ModeLegacy) : Html.FromHtml(source ?? "");
#pragma warning restore CS0618 // Type or member is obsolete
        }

        public float GetDimensionFromDp(float dpVal)
        {
            return TypedValue.ApplyDimension(ComplexUnitType.Dip, dpVal, Resources.DisplayMetrics);
        }

        public void RestartAniDroidv2()
        {
            var intent = new Intent(this, typeof(StartActivity));
            FinishAffinity();
            StartActivity(intent);
        }

        #endregion

        #region Abstract

        public abstract void OnError(IAniListError error);

        public abstract Task OnCreateExtended(Bundle savedInstanceState);

        public abstract void DisplaySnackbarMessage(string message, int length = Snackbar.LengthShort);

        public abstract void DisplayNotYetImplemented();

        #endregion

        #region Toolbar

        public virtual bool MenuItemSelected(IMenuItem item)
        {
            return base.OnOptionsItemSelected(item);
        }

        public virtual bool SetupMenu(IMenu menu)
        {
            return true;
        }

        #endregion

    }

    public class CustomNonConfigurationWrapper<T> : Java.Lang.Object
    {
        public readonly T Target;

        public CustomNonConfigurationWrapper(T target)
        {
            Target = target;
        }
    }
}
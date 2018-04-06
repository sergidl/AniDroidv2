﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using AniDroid.Adapters.Base;
using AniDroid.Adapters.MediaAdapters;
using AniDroid.AniList.Interfaces;
using AniDroid.Base;
using AniDroid.Dialogs;
using AniDroid.Main;
using AniDroid.Utils;
using Ninject;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace AniDroid.Settings
{
    [Activity(Label = "Settings")]
    public class SettingsActivity : BaseAniDroidActivity<SettingsPresenter>, ISettingsView
    {

        [InjectView(Resource.Id.Settings_CoordLayout)]
        private CoordinatorLayout _coordLayout;
        [InjectView(Resource.Id.Settings_Toolbar)]
        private Toolbar _toolbar;
        [InjectView(Resource.Id.Settings_Container)]
        private LinearLayout _settingsContainer;

        private bool _recreateActivity;

        protected override IReadOnlyKernel Kernel => new StandardKernel(new ApplicationModule<ISettingsView, SettingsActivity>(this));

        public override void OnError(IAniListError error)
        {
            // TODO: should this ever matter?
        }

        public override void DisplaySnackbarMessage(string message, int length)
        {
            Snackbar.Make(_coordLayout, message, length).Show();
        }

        public void CreateCardTypeSettingItem(BaseRecyclerAdapter.RecyclerCardType cardType)
        {
            var options = new List<string> {"Vertical", "Horizontal", "Flat Horizontal"};
            _settingsContainer.AddView(
                CreateSpinnerSettingRow(this, "Card Display Type", "Choose how you would like to display lists in AniDroid",
                    options, (int) cardType,
                    (sender, args) =>
                    {
                        Presenter.SetCardType((BaseRecyclerAdapter.RecyclerCardType) args.Position);

                        if (cardType != (BaseRecyclerAdapter.RecyclerCardType) args.Position)
                        {
                            _recreateActivity = true;
                            Intent.PutExtra(MainActivity.RecreateActivityIntentKey, true);
                        }
                    }));
            _settingsContainer.AddView(CreateSettingDivider(this));
        }

        public void CreateAniDroidThemeSettingItem(AniDroidTheme theme)
        {
            var options = new List<string> { "AniList", "Dark" };
            _settingsContainer.AddView(
                CreateSpinnerSettingRow(this, "AniDroid Theme", "Choose the theme you'd like to use", options, (int)theme, (sender, args) =>
                {
                    Presenter.SetTheme((AniDroidTheme) args.Position);

                    if (theme != (AniDroidTheme) args.Position)
                    {
                        Recreate();
                        Intent.PutExtra(MainActivity.RecreateActivityIntentKey, true);
                    }
                }));
            _settingsContainer.AddView(CreateSettingDivider(this));
        }

        public void CreateDisplayBannersSettingItem(bool displayBanners)
        {
            _settingsContainer.AddView(
                CreateSwitchSettingRow(this, "Display Banners", "Choose whether you'd like to display banner images for Media and Users", displayBanners, (sender, args) =>
                    Presenter.SetDisplayBanners(args.IsChecked)));
            _settingsContainer.AddView(CreateSettingDivider(this));
        }

        public void CreateWhatsNewSettingItem()
        {
            _settingsContainer.AddView(
                CreateSettingRow(this, "Show What's New", null, (sender, args) =>
                    WhatsNewDialog.Create(this)));
            _settingsContainer.AddView(CreateSettingDivider(this));
        }

        #region Auth Settings

        public void CreateGroupCompletedSettingItem(bool groupCompleted)
        {
            _settingsContainer.AddView(
                CreateSwitchSettingRow(this, "Group Completed Items", "Choose whether you'd like to group all completed lists together under one list, regardless of how you have it set on AniList", groupCompleted, (sender, args) =>
                    Presenter.SetGroupCompleted(args.IsChecked)));
            _settingsContainer.AddView(CreateSettingDivider(this));
        }

        public void CreateMediaListViewTypeSettingItem(MediaListRecyclerAdapter.MediaListItemViewType viewType)
        {
            var options = new List<string> { "Normal", "Compact", "Title Only" };
            _settingsContainer.AddView(
                CreateSpinnerSettingRow(this, "Media List View", "Choose how you'd like to show media list items", options, (int)viewType, (sender, args) =>
                {
                    Presenter.SetMediaListViewType((MediaListRecyclerAdapter.MediaListItemViewType)args.Position);

                    if (viewType != (MediaListRecyclerAdapter.MediaListItemViewType)args.Position)
                    {
                        _recreateActivity = true;
                        Intent.PutExtra(MainActivity.RecreateActivityIntentKey, true);
                    }
                }));
            _settingsContainer.AddView(CreateSettingDivider(this));
        }

        public void CreateHighlightPriorityMediaListItemsItem(bool highlightPriorityItems)
        {
            _settingsContainer.AddView(
                CreateSwitchSettingRow(this, "Highlight Priority Media List Items",
                    "Choose whether you'd like to show a highlighted background on all media list items that you've marked as Priority",
                    highlightPriorityItems,
                    (sender, args) =>
                    {
                        Presenter.SetHighlightPriorityMediaListItems(args.IsChecked);
                        _recreateActivity = true;
                        Intent.PutExtra(MainActivity.RecreateActivityIntentKey, true);
                    }));
            _settingsContainer.AddView(CreateSettingDivider(this));
        }

        public void CreateDisplayProgressColorsItem(bool displayProgressColors)
        {
            _settingsContainer.AddView(
                CreateSwitchSettingRow(this, "Show Airing Progress Colors",
                    "Turn this on to change the color of the titles of items on your anime lists as they correspond to the current number of aired episodes",
                    displayProgressColors,
                    (sender, args) =>
                    {
                        Presenter.SetDisplayProgressColorsItem(args.IsChecked);
                        _recreateActivity = true;
                        Intent.PutExtra(MainActivity.RecreateActivityIntentKey, true);
                    }));
            _settingsContainer.AddView(CreateSettingDivider(this));
        }

        #endregion

        public static void StartActivity(Activity context)
        {
            var intent = new Intent(context, typeof(SettingsActivity));
            context.StartActivityForResult(intent, 1);
        }

        public override async Task OnCreateExtended(Bundle savedInstanceState)
        {
            _recreateActivity = Intent.GetBooleanExtra(MainActivity.RecreateActivityIntentKey, false);

            SetContentView(Resource.Layout.Activity_Settings);

            SetupToolbar();

            await CreatePresenter(savedInstanceState);
        }

        public override void OnBackPressed()
        {
            if (_recreateActivity)
            {
                var resultIntent = new Intent();
                resultIntent.PutExtra(MainActivity.RecreateActivityIntentKey, true);
                SetResult(Result.Canceled, resultIntent);
                Finish();
            }
            else
            {
                base.OnBackPressed();
            }
        }

        private void DisplayLogoutDialog()
        {
            var alert = new Android.Support.V7.App.AlertDialog.Builder(this,
                    GetThemedResourceId(Resource.Attribute.Dialog_Theme))
                .SetMessage(Resource.String.LoginLogout_LogoutDialogMessage)
                .SetCancelable(true).Create();
            alert.SetButton((int) DialogButtonType.Neutral, "Cancel", (sender, eventArgs) => alert.Dismiss());
            alert.SetButton((int) DialogButtonType.Positive, "Logout", (sender, eventArgs) =>
                {
                    Settings.ClearUserAuthentication();
                    var resultIntent = new Intent();
                    resultIntent.PutExtra(MainActivity.RecreateActivityIntentKey, true);
                    SetResult(Result.Canceled, resultIntent);
                    Finish();
                });
            alert.Show();
        }

        #region Settings Views

        private static View CreateSettingRow(BaseAniDroidActivity context, string name, string description, EventHandler tapEvent)
        {
            var view = context.LayoutInflater.Inflate(Resource.Layout.View_SettingItem, null);
            var nameView = view.FindViewById<TextView>(Resource.Id.SettingItem_Name);
            var textTwoView = view.FindViewById<TextView>(Resource.Id.SettingItem_Details);

            if (!string.IsNullOrWhiteSpace(name))
            {
                nameView.Text = name;
            }
            else
            {
                nameView.Visibility = ViewStates.Gone;
            }

            if (!string.IsNullOrWhiteSpace(description))
            {
                textTwoView.Text = description;
            }
            else
            {
                textTwoView.Visibility = ViewStates.Gone;
            }

            view.Id = (int)DateTime.Now.Ticks;
            view.Click += tapEvent;

            return view;
        }

        public static View CreateSwitchSettingRow(BaseAniDroidActivity context, string name, string description, bool switchState, EventHandler<CompoundButton.CheckedChangeEventArgs> switchEvent)
        {
            var view = context.LayoutInflater.Inflate(Resource.Layout.View_SettingItem_Switch, null);
            var nameView = view.FindViewById<TextView>(Resource.Id.SettingItem_Name);
            var textTwoView = view.FindViewById<TextView>(Resource.Id.SettingItem_Details);

            if (!string.IsNullOrWhiteSpace(name))
            {
                nameView.Text = name;
            }
            else
            {
                nameView.Visibility = ViewStates.Gone;
                textTwoView.SetTextColor(new Color(context.GetThemedColor(Resource.Attribute.Background_Text)));
            }

            if (!string.IsNullOrWhiteSpace(description))
            {
                textTwoView.Text = description;
            }
            else
            {
                textTwoView.Visibility = ViewStates.Gone;
            }

            var switchView = view.FindViewById<SwitchCompat>(Resource.Id.SettingItem_Switch);
            switchView.Id = (int)DateTime.Now.Ticks;
            switchView.Checked = switchState;
            switchView.CheckedChange += switchEvent;

            return view;
        }

        public static View CreateCheckboxSettingRow(BaseAniDroidActivity context, string name, string description, bool isChecked, EventHandler<CompoundButton.CheckedChangeEventArgs> checkEvent)
        {
            var view = context.LayoutInflater.Inflate(Resource.Layout.View_SettingItem_Checkbox, null);

            var nameView = view.FindViewById<TextView>(Resource.Id.SettingItem_Name);
            var textTwoView = view.FindViewById<TextView>(Resource.Id.SettingItem_Details);

            if (!string.IsNullOrWhiteSpace(name))
            {
                nameView.Text = name;
            }
            else
            {
                nameView.Visibility = ViewStates.Gone;
                textTwoView.SetTextColor(new Color(context.GetThemedColor(Resource.Attribute.Background_Text)));
            }

            if (!string.IsNullOrWhiteSpace(description))
            {
                textTwoView.Text = description;
            }
            else
            {
                textTwoView.Visibility = ViewStates.Gone;
            }

            var checkboxView = view.FindViewById<AppCompatCheckBox>(Resource.Id.SettingItem_Checkbox);
            checkboxView.Id = (int)DateTime.Now.Ticks;
            checkboxView.Checked = isChecked;
            checkboxView.CheckedChange += checkEvent;

            return view;
        }

        public static View CreateSpinnerSettingRow(BaseAniDroidActivity context, string name, string description, IList<string> items, int selectedPosition, EventHandler<AdapterView.ItemSelectedEventArgs> selectedEvent)
        {
            var view = context.LayoutInflater.Inflate(Resource.Layout.View_SettingItem_Spinner, null);

            var nameView = view.FindViewById<TextView>(Resource.Id.SettingItem_Name);
            var textTwoView = view.FindViewById<TextView>(Resource.Id.SettingItem_Details);

            if (!string.IsNullOrWhiteSpace(name))
            {
                nameView.Text = name;
            }
            else
            {
                nameView.Visibility = ViewStates.Gone;
                textTwoView.SetTextColor(new Color(context.GetThemedColor(Resource.Attribute.Background_Text)));
            }

            if (!string.IsNullOrWhiteSpace(description))
            {
                textTwoView.Text = description;
            }
            else
            {
                textTwoView.Visibility = ViewStates.Gone;
            }

            var spinner = view.FindViewById<Spinner>(Resource.Id.SettingItem_Spinner);
            spinner.Id = (int)DateTime.Now.Ticks;
            spinner.Adapter = new ArrayAdapter<string>(context, Resource.Layout.View_SpinnerDropDownItem, items);
            spinner.SetSelection(selectedPosition);
            spinner.ItemSelected -= selectedEvent;
            spinner.ItemSelected += selectedEvent;

            return view;
        }

        public static View CreateSettingDivider(BaseAniDroidActivity context)
        {
            var typedValue = new TypedValue();
            context.Theme.ResolveAttribute(Resource.Attribute.ListItem_Divider, typedValue, true);
            var dividerView = new LinearLayout(context) { LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, 2) };
            dividerView.SetBackgroundResource(typedValue.ResourceId);

            return dividerView;
        }

        #endregion

        #region Toolbar

        private void SetupToolbar()
        {
            SetSupportActionBar(_toolbar);
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_arrow_back_white_24px);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        public override bool SetupMenu(IMenu menu)
        {
            menu?.Clear();
            MenuInflater.Inflate(Resource.Menu.Settings_ActionBar, menu);
            menu?.FindItem(Resource.Id.Menu_Settings_Logout)?.SetVisible(Settings.IsUserAuthenticated);
            return true;
        }

        public override bool MenuItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    if (_recreateActivity)
                    {
                        var resultIntent = new Intent();
                        resultIntent.PutExtra(MainActivity.RecreateActivityIntentKey, true);
                        SetResult(Result.Canceled, resultIntent);
                    }
                    Finish();
                    return true;
                case Resource.Id.Menu_Settings_Logout:
                    DisplayLogoutDialog();
                    return true;
            }

            return false;
        }

        #endregion

    }
}
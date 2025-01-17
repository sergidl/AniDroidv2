﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using AndroidX.CoordinatorLayout.Widget;
using AndroidX.Transitions;
using AniDroidv2.AniList;
using AniDroidv2.AniList.DataTypes;
using AniDroidv2.AniList.Dto;
using AniDroidv2.AniList.Enums.MediaEnums;
using AniDroidv2.AniList.Enums.UserEnums;
using AniDroidv2.AniList.Models.MediaModels;
using AniDroidv2.AniList.Models.UserModels;
using AniDroidv2.AniListObject.Media;
using AniDroidv2.Base;
using AniDroidv2.Settings;
using AniDroidv2.Widgets;
using Google.Android.Material.Snackbar;
using Newtonsoft.Json.Linq;
using AlertDialog = AndroidX.AppCompat.App.AlertDialog;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace AniDroidv2.Dialogs
{
    public static class EditMediaListItemDialog
    {
        public static void Create(BaseAniDroidv2Activity context, IAniListMediaListEditPresenter presenter, Media media, AniList.Models.MediaModels.MediaList mediaList, UserMediaListOptions mediaListOptions, bool completeMedia = false)
        {
            var dialog = new EditMediaListItemDialogFragment(presenter, media, mediaList, mediaListOptions, completeMedia) {Cancelable = true};
            var transaction = context.SupportFragmentManager.BeginTransaction();
            transaction.SetTransition((int)FragmentTransit.FragmentOpen);
            transaction.Add(Android.Resource.Id.Content, dialog).AddToBackStack(EditMediaListItemDialogFragment.BackstackTag).Commit();
        }

        public class EditMediaListItemDialogFragment : AppCompatDialogFragment
        {
            public const string BackstackTag = "EDIT_MEDIA_DIALOG";

            private const int DefaultMaxPickerValue = 9999;

            private readonly IAniListMediaListEditPresenter _presenter;
            private readonly Media _media;
            private readonly AniList.Models.MediaModels.MediaList _mediaList;
            private readonly UserMediaListOptions _mediaListOptions;
            private readonly List<string> _mediaStatusList;
            private readonly HashSet<string> _customLists;
            private readonly bool _completeMedia;
            private bool _customScoringEnabled;
            private new BaseAniDroidv2Activity Activity => base.Activity as BaseAniDroidv2Activity;
            private bool _isPrivate;
            private bool _hideFromStatusLists;
            private int _priority;
            private bool _pendingDismiss;
            private List<float?> _advancedScores;
            private int? _initialProgress;
            private int _initialStatus;

            private CoordinatorLayout _coordLayout;
            private Picker _scorePicker;
            private AppCompatSpinner _statusSpinner;
            private Picker _progressPicker;
            private Picker _progressVolumesPicker;
            private Picker _repeatPicker;
            private EditText _notesView;
            private DatePickerTextView _startDateView;
            private DatePickerTextView _finishDateView;


            public EditMediaListItemDialogFragment(IAniListMediaListEditPresenter presenter, Media media, AniList.Models.MediaModels.MediaList mediaList, UserMediaListOptions mediaListOptions, bool completeMedia)
            {
                _presenter = presenter;
                _media = media;
                _mediaList = mediaList;
                _mediaListOptions = mediaListOptions;
                _isPrivate = mediaList?.Private ?? false;
                _priority = mediaList?.Priority ?? 0;
                _hideFromStatusLists = mediaList?.HiddenFromStatusLists ?? false;
                _completeMedia = completeMedia;
                _customLists = (mediaList?.CustomLists?.Where(x => x.Enabled).Select(x => x.Name).ToList() ??
                               new List<string>()).ToHashSet();
                _customScoringEnabled = mediaListOptions.AnimeList?.AdvancedScoringEnabled ??
                                 mediaListOptions.MangaList?.AdvancedScoringEnabled == true;

                _mediaStatusList = AniListEnum.GetEnumValues<MediaListStatus>().OrderBy(x => x.Index)
                    .Select(x => x.DisplayValue).ToList();
            }

            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                // if the presenter is null, we are probably trying to recreate the fragment after it has been removed from memory.
                // don't allow that to happen
                if (_presenter == null)
                {
                    DismissAllowingStateLoss();
                    return null;
                }

                var view = Activity.LayoutInflater.Inflate(Resource.Layout.Fragment_EditMediaListItem, container,
                    false);

                _coordLayout = view.FindViewById<CoordinatorLayout>(Resource.Id.EditMediaListItem_CoordLayout);

                SetupToolbar(view.FindViewById<Toolbar>(Resource.Id.EditMediaListItem_Toolbar));
                SetupScore(_scorePicker = view.FindViewById<Picker>(Resource.Id.EditMediaListItem_ScorePicker), view.FindViewById<Button>(Resource.Id.EditMediaListItem_CustomScoringButton));
                SetupStatus(_statusSpinner = view.FindViewById<AppCompatSpinner>(Resource.Id.EditMediaListItem_StatusSpinner));
                SetupProgress(_progressPicker = view.FindViewById<Picker>(Resource.Id.EditMediaListItem_ProgressPicker),
                    view.FindViewById<TextView>(Resource.Id.EditMediaListItem_ProgressLabel));
                SetupVolumeProgress(view.FindViewById(Resource.Id.EditMediaListItem_VolumeProgressContainer),
                    _progressVolumesPicker = view.FindViewById<Picker>(Resource.Id.EditMediaListItem_VolumeProgressPicker));
                SetupRepeat(_repeatPicker = view.FindViewById<Picker>(Resource.Id.EditMediaListItem_RewatchedPicker),
                    view.FindViewById<TextView>(Resource.Id.EditMediaListItem_RewatchedLabel));
                SetupNotes(_notesView = view.FindViewById<EditText>(Resource.Id.EditMediaListItem_Notes));
                SetupCustomLists(view.FindViewById(Resource.Id.EditMediaListItem_CustomListsContainer),
                    view.FindViewById<LinearLayout>(Resource.Id.EditMediaListItem_CustomLists));
                SetupStartDate(_startDateView = view.FindViewById<DatePickerTextView>(Resource.Id.EditMediaListItem_StartDate));
                SetupFinishDate(_finishDateView = view.FindViewById<DatePickerTextView>(Resource.Id.EditMediaListItem_FinishDate));
                SetupDeleteButton(view.FindViewById(Resource.Id.EditMediaListItem_DeleteButton));

                return view;
            }

            public override void OnResume()
            {
                base.OnResume();

                if (_pendingDismiss)
                {
                    Activity.SupportFragmentManager.PopBackStack(BackstackTag, (int)PopBackStackFlags.Inclusive);
                    DismissAllowingStateLoss();
                }
            }

            private void SetupToolbar(Toolbar toolbar)
            {
                toolbar.Title = $"{(_completeMedia ? "Completing" : (_mediaList == null ? "Adding" : "Editing"))}: {_media.Title.UserPreferred}";
                toolbar.InflateMenu(Resource.Menu.EditMediaListItem_ActionBar);

                var privateItem = toolbar.Menu.FindItem(Resource.Id.Menu_EditMediaListItem_MarkPrivate);
                SetupIsPrivate(privateItem);

                var priorityItem = toolbar.Menu.FindItem(Resource.Id.Menu_EditMediaListItem_MarkPriority);
                SetupPriority(priorityItem);

                toolbar.MenuItemClick += async (sender, args) =>
                {
                    if (args.Item.ItemId == Resource.Id.Menu_EditMediaListItem_Save)
                    {
                        await SaveMediaListItem(args.Item);
                    }
                    else if (args.Item.ItemId == Resource.Id.Menu_EditMediaListItem_MarkPrivate)
                    {
                        _isPrivate = !_isPrivate;
                        SetupIsPrivate(privateItem);
                    }
                    else if (args.Item.ItemId == Resource.Id.Menu_EditMediaListItem_MarkPriority)
                    {
                        _priority = _priority > 0 ? 0 : 1;
                        SetupPriority(priorityItem);
                    }
                };
            }

            private void SetupIsPrivate(IMenuItem isPrivateItem)
            {
                isPrivateItem.SetIcon(_isPrivate ? Resource.Drawable.svg_eye_off : Resource.Drawable.svg_eye);
                isPrivateItem.SetTitle(_isPrivate ? "Mark as Public" : "Mark as Private");
            }

            private void SetupPriority(IMenuItem isPriorityItem)
            {
                isPriorityItem.SetIcon(_priority > 0 ? Resource.Drawable.svg_star : Resource.Drawable.svg_star_outline);
                isPriorityItem.SetTitle(_priority > 0 ? "Unprioritize" : "Prioritize");
            }

            private void SetupScore(Picker scorePicker, Button customScoresButton)
            {
                if (_mediaListOptions.ScoreFormat == ScoreFormat.FiveStars)
                {
                    var list = new List<string> { "", "★", "★★", "★★★", "★★★★", "★★\n★★★" };
                    scorePicker.SetStringItems(list, (int)(_mediaList?.Score ?? 3));
                    _customScoringEnabled = false;
                }
                else if (_mediaListOptions.ScoreFormat == ScoreFormat.Hundred)
                {
                    scorePicker.SetNumericValues(100, 0, false, _mediaList?.Score);
                }
                else if (_mediaListOptions.ScoreFormat == ScoreFormat.Ten)
                {
                    scorePicker.SetNumericValues(10, 0, false, _mediaList?.Score);
                    _customScoringEnabled = false;
                }
                else if (_mediaListOptions.ScoreFormat == ScoreFormat.TenDecimal)
                {
                    scorePicker.SetNumericValues(10, 1, false, _mediaList?.Score);
                }
                else if (_mediaListOptions.ScoreFormat == ScoreFormat.ThreeSmileys)
                {
                    scorePicker.SetDrawableItems(new List<int> { 0, Resource.Drawable.svg_sad, Resource.Drawable.svg_neutral, Resource.Drawable.svg_happy }, (int)(_mediaList?.Score ?? 2));
                    _customScoringEnabled = false;
                }

                if (_customScoringEnabled)
                {
                    try
                    {
                        // TODO: this is a massive hack. need to see about getting these values returned as an array
                        _advancedScores = (_mediaList?.AdvancedScores as JObject)?.PropertyValues().Select(x => x.Value<float?>()).ToList();

                        if (_advancedScores == null && _mediaList != null)
                        {
                            throw new Exception("Advanced Scores failed to parse");
                        }
                    }
                    catch (Exception e)
                    {
                        Activity.Logger.Error("CUSTOM_SCORING", "Error occurred while setting up custom scoring", e);
                        Toast.MakeText(Activity, "Error occurred while getting advanced scores", ToastLength.Short).Show();
                        return;
                    }


                    customScoresButton.Visibility = ViewStates.Visible;
                    customScoresButton.Click += (sender, e) =>
                        CustomScoringDialog.Create(Activity, _mediaListOptions.AnimeList.AdvancedScoring, _mediaListOptions.ScoreFormat, _advancedScores, SetAdvancedScoresToSave);
                }
                else
                {
                    customScoresButton.Visibility = ViewStates.Gone;
                }
            }

            private void SetupStatus(AppCompatSpinner statusSpinner)
            {
                statusSpinner.Adapter = new ArrayAdapter<string>(Activity, Resource.Layout.View_SpinnerDropDownItem, _mediaStatusList);

                if (_completeMedia)
                {
                    statusSpinner.SetSelection(MediaListStatus.Completed.Index);
                }
                else if (_mediaList?.Status != null)
                { 
                    statusSpinner.SetSelection(_mediaList.Status.Index);
                }
                else if (_media.Status == MediaStatus.Releasing  || _media.Status == MediaStatus.Cancelled)
                {
                    statusSpinner.SetSelection(MediaListStatus.Current.Index);
                }
                else
                {
                    statusSpinner.SetSelection(MediaListStatus.Planning.Index);
                }

                _initialStatus = statusSpinner.SelectedItemPosition;

                statusSpinner.ItemSelected += (sender, args) =>
                {
                    var statusEnum = AniListEnum.GetEnum<MediaListStatus>(args.Position);
                    var initialStatusEnum = AniListEnum.GetEnum<MediaListStatus>(_initialStatus);

                    if (statusEnum.Equals(MediaListStatus.Completed))
                    {
                        if (!_initialProgress.HasValue)
                        {
                            _initialProgress = (int?) _progressPicker.GetValue();
                        }

                        if (!_finishDateView.SelectedDate.HasValue && Activity.Settings.AutoFillDateForMediaListItem)
                        {
                            _finishDateView.SelectedDate = DateTime.Now;
                        }

                        _progressPicker.SetValue(_media.Episodes);
                    }
                    else if (statusEnum.Equals(MediaListStatus.Current))
                    {
                        if (!_startDateView.SelectedDate.HasValue && Activity.Settings.AutoFillDateForMediaListItem)
                        {
                            _startDateView.SelectedDate = DateTime.Now;
                            _finishDateView.SelectedDate = null;
                        }

                        if (_initialProgress.HasValue)
                        {
                            _progressPicker.SetValue(_initialProgress);
                            _initialProgress = null;
                        }
                    }
                    else if (statusEnum.Equals(MediaListStatus.Planning) && _mediaList == null)
                    {
                        _startDateView.SelectedDate = null;
                        _finishDateView.SelectedDate = null;
                        _scorePicker.SetValue(null);
                    }
                    else if (statusEnum.Equals(MediaListStatus.Repeating) && initialStatusEnum.Equals(MediaListStatus.Completed))
                    {
                        _progressPicker.SetValue(null);
                    }
                    else if (statusEnum.EqualsAny(MediaListStatus.Paused, MediaListStatus.Dropped, MediaListStatus.Repeating, MediaListStatus.Planning))
                    {
                        if (_initialProgress.HasValue)
                        {
                            _progressPicker.SetValue(_initialProgress);
                            _initialProgress = null;
                        }
                    }
                };
            }

            private void SetupProgress(Picker progressPicker, TextView progressLabel)
            {
                if (_media.Type == MediaType.Anime)
                {
                    var episodes = _media.Episodes > 0
                        ? _media.Episodes.Value
                        : (_media?.NextAiringEpisode?.Episode > 0 ? _media.NextAiringEpisode.Episode : DefaultMaxPickerValue);

                    progressPicker.SetNumericValues(episodes, 0, false, _completeMedia ? episodes : _mediaList?.Progress);
                }
                else if (_media.Type == MediaType.Manga)
                {
                    progressLabel.Text = "Chapters";
                    progressPicker.SetNumericValues(_media.Chapters > 0 ? _media.Chapters.Value : DefaultMaxPickerValue, 0, false, _completeMedia ? _media.Chapters : _mediaList?.Progress);
                }
            }

            private void SetupVolumeProgress(View volumeProgressContainer, Picker volumeProgressPicker)
            {
                if (_media.Type != MediaType.Manga)
                {
                    volumeProgressContainer.Visibility = ViewStates.Gone;
                    return;
                }

                volumeProgressContainer.Visibility = ViewStates.Visible;
                volumeProgressPicker.SetNumericValues(_media.Volumes > 0 ? _media.Volumes.Value : DefaultMaxPickerValue, 0, false, _mediaList?.ProgressVolumes);
            }

            private void SetupRepeat(Picker rewatchedPicker, TextView rewatchedLabel)
            {
                if (_media.Type == MediaType.Manga)
                {
                    rewatchedLabel.Text = "Reread";
                }

                rewatchedPicker.SetNumericValues(DefaultMaxPickerValue, 0, false, _mediaList?.Repeat);
            }

            private void SetupNotes(TextView notesView)
            {
                notesView.Text = _mediaList?.Notes;
            }

            private void SetupStartDate(DatePickerTextView startDateView)
            {
                startDateView.SelectedDate = _mediaList?.StartedAt?.GetDate();
                startDateView.DateChanged += (sender, args) => {
                    if (_finishDateView.SelectedDate < args.Date)
                    {
                        Snackbar.Make(_coordLayout, "Start date must be on or before Finish date!", Snackbar.LengthShort)
                            .Show();
                        _startDateView.SelectedDate = null;
                    }

                };
            }

            private void SetupFinishDate(DatePickerTextView finishDateView)
            {
                finishDateView.SelectedDate = _completeMedia ? DateTime.Now : _mediaList?.CompletedAt?.GetDate();
                finishDateView.DateChanged += (sender, args) => {
                    if (_startDateView.SelectedDate > args.Date)
                    {
                        Snackbar.Make(_coordLayout, "Finish date must be on or after Start date!", Snackbar.LengthShort)
                            .Show();
                        _finishDateView.SelectedDate = null;
                    }

                };
            }

            private void SetupCustomLists(View customListsContainer, LinearLayout customLists)
            {
                var lists = _media.Type == MediaType.Anime
                    ? _mediaListOptions?.AnimeList?.CustomLists
                    : _mediaListOptions?.MangaList?.CustomLists;

                if (lists?.Any() != true)
                {
                    customListsContainer.Visibility = ViewStates.Gone;
                    return;
                }

                var hideSwitchRow =
                    SettingsActivity.CreateSwitchSettingRow(Activity, null, "Hide from status lists", _hideFromStatusLists, false,
                        (sender, eventArgs) => {
                            if (eventArgs.IsChecked && _customLists.Count == 0)
                            {
                                (sender as SwitchCompat).Checked = false;
                                Snackbar.Make(_coordLayout, "Must be on at least one list!", Snackbar.LengthShort)
                                    .Show();
                                return;
                            }

                            _hideFromStatusLists = eventArgs.IsChecked;
                        });
                var hideSwitchView = hideSwitchRow.FindViewById<SwitchCompat>(Resource.Id.SettingItem_Switch);

                foreach (var list in lists)
                {
                    customLists.AddView(SettingsActivity.CreateCheckboxSettingRow(Activity, null, list, _customLists.Contains(list),
                        (sender, eventArgs) => {
                            if (eventArgs.IsChecked)
                            {
                                _customLists.Add(list);
                            }
                            else
                            {
                                _customLists.Remove(list);

                                if (_customLists.Count == 0)
                                {
                                    hideSwitchView.Checked = false;
                                }
                            }
                        }));
                }

                customLists.AddView(SettingsActivity.CreateSettingDivider(Activity));
                customLists.AddView(hideSwitchRow);
            }

            private void SetupDeleteButton(View deleteButton)
            {
                if (_mediaList == null)
                {
                    deleteButton.Visibility = ViewStates.Gone;
                    return;
                }

                deleteButton.Click += (sender, args) =>
                {
                    var confirmation = new AlertDialog.Builder(Activity,
                        Activity.GetThemedResourceId(Resource.Attribute.Dialog_Theme));

                    confirmation.SetMessage(
                        "Do you really wish to delete this item from your lists? This action CAN NOT be undone!");
                    confirmation.SetTitle($"Delete {_media.Title.UserPreferred}");
                    confirmation.SetNegativeButton("Cancel", (cancelSender, cancelArgs) => { });
                    confirmation.SetPositiveButton("Delete", async (delSender, delArgs) => await DeleteMediaListItem());

                    confirmation.Show();
                };
            }

            private void SetAdvancedScoresToSave(List<float?> scores)
            {
                _advancedScores = scores;

                var score = scores.Sum() / scores.Count(x => x > 0);

                _scorePicker.SetValue(score);
            }

            private async Task SaveMediaListItem(IMenuItem menuItem)
            {
                menuItem.SetEnabled(false);
                menuItem.SetActionView(new ProgressBar(Activity) {Indeterminate = true});

                var editDto = new MediaListEditDto
                {
                    MediaId = _media.Id,
                    Status = AniListEnum.GetEnum<MediaListStatus>(_statusSpinner.SelectedItemPosition),
                    Score = _scorePicker.GetValue(),
                    Progress = (int?) _progressPicker.GetValue(),
                    ProgressVolumes =
                        _media.Type == MediaType.Manga ? (int?) _progressVolumesPicker.GetValue() : null,
                    Repeat = (int?) _repeatPicker.GetValue(),
                    Notes = _notesView.Text,
                    Private = _isPrivate,
                    Priority = _priority,
                    CustomLists = _customLists.ToList(),
                    HiddenFromStatusLists = _hideFromStatusLists,
                    StartDate = new FuzzyDate(_startDateView.SelectedDate),
                    FinishDate = new FuzzyDate(_finishDateView.SelectedDate),
                    AdvancedScores = _advancedScores
                };

                if ((_mediaListOptions.ScoreFormat == ScoreFormat.FiveStars ||
                     _mediaListOptions.ScoreFormat == ScoreFormat.ThreeSmileys) && !editDto.Score.HasValue)
                {
                    editDto.Score = 0;
                }

                var transition = new Fade(Visibility.ModeOut);
                transition.SetDuration(300);
                ExitTransition = transition;

                await _presenter.SaveMediaListEntry(editDto, () =>
                {
                    menuItem.SetActionView(null);

                    // TODO: there has to be a better way to do this (crashing on this line when minimizing app during save)
                    try
                    {
                        Activity.SupportFragmentManager.PopBackStack(BackstackTag, (int) PopBackStackFlags.Inclusive);
                        DismissAllowingStateLoss();
                    }
                    catch
                    {
                        _pendingDismiss = true;
                    }
                }, () =>
                {
                    Snackbar.Make(_coordLayout, "Error occurred while saving list entry", Snackbar.LengthShort)
                        .Show();
                    menuItem.SetEnabled(true);
                    menuItem.SetActionView(null);
                });
            }

            private async Task DeleteMediaListItem()
            {
                var transition = new Fade(Visibility.ModeOut);
                transition.SetDuration(300);
                ExitTransition = transition;

                await _presenter.DeleteMediaListEntry(_mediaList.Id, () =>
                {
                    try
                    {
                        Activity.SupportFragmentManager.PopBackStack(BackstackTag, (int) PopBackStackFlags.Inclusive);
                        DismissAllowingStateLoss();
                    }
                    catch
                    {
                        _pendingDismiss = true;
                    }
                }, () =>
                {
                    Snackbar.Make(_coordLayout, "Error occurred while deleting list entry", Snackbar.LengthShort)
                        .Show();
                });
            }
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AniDroid.Adapters.Base;
using AniDroid.AniList.Models;
using AniDroid.Base;
using AniDroid.Utils.Interfaces;
using AniDroid.Utils.Storage;

namespace AniDroid.Utils
{
    internal class AniDroidSettings : IAniDroidSettings
    {
        private readonly SettingsStorage _settingStorage;
        private readonly AuthSettingsStorage _authSettingStorage;

        public AniDroidSettings(SettingsStorage settingsStorage, AuthSettingsStorage authSettingsStorage)
        {
            _settingStorage = settingsStorage;
            _authSettingStorage = authSettingsStorage;
        }

        public BaseRecyclerAdapter.RecyclerCardType CardType
        {
            get => _settingStorage.Get(StorageKeys.CardTypeKey, BaseRecyclerAdapter.RecyclerCardType.Vertical);
            set => _settingStorage.Put(StorageKeys.CardTypeKey, value);
        }

        public BaseAniDroidActivity.AniDroidTheme Theme
        {
            get => _settingStorage.Get(StorageKeys.ThemeKey, BaseAniDroidActivity.AniDroidTheme.AniList);
            set => _settingStorage.Put(StorageKeys.ThemeKey, value);
        }

        public bool DisplayBanners
        {
            get => _settingStorage.Get(StorageKeys.DisplayBannersKey, true);
            set => _settingStorage.Put(StorageKeys.DisplayBannersKey, value);
        }

        public string UserAccessCode
        {
            get => _authSettingStorage.Get(StorageKeys.AccessCode);
            set => _authSettingStorage.Put(StorageKeys.AccessCode, value);
        }

        public bool IsUserAuthenticated => !string.IsNullOrWhiteSpace(UserAccessCode);

        public void ClearUserAuthentication()
        {
            UserAccessCode = null;
            LoggedInUser = null;
        }

        public User LoggedInUser
        {
            get => _authSettingStorage.Get<User>(StorageKeys.LoggedInUser);
            set => _authSettingStorage.Put(StorageKeys.LoggedInUser, value);
        }

        public bool ShowAllAniListActivity
        {
            get => _authSettingStorage.Get(StorageKeys.ShowAllActivityKey, false);
            set => _authSettingStorage.Put(StorageKeys.ShowAllActivityKey, value);
        }

        public List<KeyValuePair<string, bool>> AnimeListOrder
        {
            get => _authSettingStorage.Get(StorageKeys.AnimeListOrderKey, (List<KeyValuePair<string, bool>>)null);
            set => _authSettingStorage.Put(StorageKeys.AnimeListOrderKey, value);
        }

        public List<KeyValuePair<string, bool>> MangaListOrder
        {
            get => _authSettingStorage.Get(StorageKeys.MangaListOrderKey, (List<KeyValuePair<string, bool>>)null);
            set => _authSettingStorage.Put(StorageKeys.MangaListOrderKey, value);
        }

        public bool GroupCompletedLists
        {
            get => _authSettingStorage.Get(StorageKeys.GroupCompletedLists, false);
            set => _authSettingStorage.Put(StorageKeys.GroupCompletedLists, value);
        }

        #region Constants

        private static class StorageKeys
        {
            public const string CardTypeKey = "CARD_TYPE";
            public const string ThemeKey = "THEME";
            public const string DisplayBannersKey = "DISPLAY_BANNERS";

            public const string AccessCode = "ACCESS_CODE";
            public const string LoggedInUser = "LOGGED_IN_USER";
            public const string AnimeListOrderKey = "ANIME_LIST_ORDER_KEY";
            public const string MangaListOrderKey = "MANGA_LIST_ORDER_KEY";
            public const string GroupCompletedLists = "GROUP_COMPLETED_LISTS";

            public const string ShowAllActivityKey = "SHOW_ALL_ACTIVITY";
        }

        #endregion
    }
}
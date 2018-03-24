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
        private readonly AniDroidStorage _settingStorage;

        public AniDroidSettings(AniDroidStorage settingsStorage)
        {
            _settingStorage = settingsStorage;
        }

        public BaseRecyclerAdapter.CardType CardType
        {
            get => _settingStorage.Get(StorageKeys.CardTypeKey, BaseRecyclerAdapter.CardType.Vertical);
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
            get => _settingStorage.Get(StorageKeys.AccessCode);
            set => _settingStorage.Put(StorageKeys.AccessCode, value);
        }

        public bool IsUserAuthenticated => !string.IsNullOrWhiteSpace(UserAccessCode);

        public void ClearUserAuthentication()
        {
            UserAccessCode = null;
            LoggedInUser = null;
        }

        public User LoggedInUser
        {
            get => _settingStorage.Get<User>(StorageKeys.LoggedInUser);
            set => _settingStorage.Put(StorageKeys.LoggedInUser, value);
        }

        public bool ShowAllAniListActivity
        {
            get => _settingStorage.Get(StorageKeys.ShowAllActivityKey, false);
            set => _settingStorage.Put(StorageKeys.ShowAllActivityKey, value);
        }

        public List<KeyValuePair<string, bool>> AnimeListOrder
        {
            get => _settingStorage.Get(StorageKeys.AnimeListOrderKey, (List<KeyValuePair<string, bool>>)null);
            set => _settingStorage.Put(StorageKeys.AnimeListOrderKey, value);
        }

        public List<KeyValuePair<string, bool>> MangaListOrder
        {
            get => _settingStorage.Get(StorageKeys.MangaListOrderKey, (List<KeyValuePair<string, bool>>)null);
            set => _settingStorage.Put(StorageKeys.MangaListOrderKey, value);
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

            public const string ShowAllActivityKey = "SHOW_ALL_ACTIVITY";
        }

        #endregion
    }
}
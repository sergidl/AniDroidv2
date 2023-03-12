using System;
using System.IO;
using System.Reflection;
using Android.App;
using Android.OS;
using Android.Runtime;
using AniDroidv2.AniList.Interfaces;
using AniDroidv2.AniList.Service;
using AniDroidv2.AniListObject.Character;
using AniDroidv2.AniListObject.Media;
using AniDroidv2.AniListObject.Staff;
using AniDroidv2.AniListObject.Studio;
using AniDroidv2.AniListObject.User;
using AniDroidv2.Browse;
using AniDroidv2.CurrentSeason;
using AniDroidv2.Discover;
using AniDroidv2.Favorites;
using AniDroidv2.Home;
using AniDroidv2.Jobs;
using AniDroidv2.Login;
using AniDroidv2.Main;
using AniDroidv2.MediaList;
using AniDroidv2.SearchResults;
using AniDroidv2.Settings;
using AniDroidv2.Settings.MediaListSettings;
using AniDroidv2.TorrentSearch;
using AniDroidv2.Utils;
using AniDroidv2.Utils.Integration;
using AniDroidv2.Utils.Interfaces;
using AniDroidv2.Utils.Logging;
using AniDroidv2.Utils.Storage;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xamarin.Essentials;

namespace AniDroidv2
{
#if DEBUG
    [Application(AllowBackup = true, Theme = "@style/AniList", Label= "@string/AppName", Icon = "@drawable/IconDebug")]
#else
    [Application(AllowBackup = true, Theme = "@style/AniList", Label= "@string/AppName", Icon = "@drawable/Icon")]
#endif
    public class AniDroidv2Application : Application
    {
        public static IServiceProvider ServiceProvider { get; set; }

        protected AniDroidv2Application(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            Platform.Init(this);

            var serviceProvider = InitServiceProvider();

            var appCenterId = serviceProvider.GetService<IConfiguration>()["AppCenterId"];

            AppCenter.Start(appCenterId,
                typeof(Analytics), typeof(Crashes));
            
            VersionTracking.Track();

            CreateNotificationsChannel();
        }

        private void CreateNotificationsChannel()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(Resources.GetString(Resource.String.NotificationsChannelId), Resources.GetString(Resource.String.NotificationsChannelName),
                    NotificationImportance.Default);

                channel.EnableVibration(true);
                channel.EnableLights(true);

                var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }
        }

        public static IServiceProvider InitServiceProvider()
        {
            var configFile = ExtractResource("AniDroidv2.appsettings.json", FileSystem.AppDataDirectory);
            var secretConfigFile = ExtractResource("AniDroidv2.appsettings.secret.json", FileSystem.AppDataDirectory);

            var host = new HostBuilder()
                .UseContentRoot(FileSystem.AppDataDirectory)
                .ConfigureHostConfiguration(c =>
                {

                    // Tell the host configuration where to file the file (this is required for Xamarin apps)
                    //c.AddCommandLine(new[] { $"ContentRoot={FileSystem.AppDataDirectory}" });

                    c.AddJsonFile(configFile);

                    c.AddJsonFile(secretConfigFile);
                })
                .ConfigureServices(ConfigureServices)
                .ConfigureLogging(l =>
                {
                    //l.AddConsole(o =>
                    //{
                    //    //setup a console logger and disable colors since they don't have any colors in VS
                    //    o.DisableColors = true;
                    //});
                })
                .Build();

            //Save our service provider so we can use it later.
            return ServiceProvider = host.Services;
        }

        private static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            services.AddHttpClient();

            services.TryAddSingleton<IAniDroidv2Logger, AppCenterLogger>();

            services.TryAddSingleton<IAniListAuthConfig>(x => new AniDroidv2AniListAuthConfig(ctx.Configuration["ApiConfiguration:ClientId"],
                ctx.Configuration["ApiConfiguration:ClientSecret"], ctx.Configuration["ApiConfiguration:RedirectUrl"],
                ctx.Configuration["ApiConfiguration:AuthUrl"]));
            services.TryAddSingleton<IAniDroidv2Settings>(x => new AniDroidv2Settings(new SettingsStorage(Application.Context), new AuthSettingsStorage(Application.Context)));
            services.TryAddSingleton<IAuthCodeResolver, AniDroidv2AuthCodeResolver>();
            services.TryAddSingleton<IAniListServiceConfig>(x => new AniDroidv2AniListServiceConfig(ctx.Configuration["ApiConfiguration:BaseUrl"]));

            services.TryAddTransient<IAniListService, AniListService>();

            ConfigurePresenters(services);
        }

        private static void ConfigurePresenters(IServiceCollection services)
        {
            services.TryAddTransient<MainPresenter>();
            services.TryAddTransient<MediaListPresenter>();
            services.TryAddTransient<MediaPresenter>();
            services.TryAddTransient<CharacterPresenter>();
            services.TryAddTransient<StaffPresenter>();
            services.TryAddTransient<BrowsePresenter>();
            services.TryAddTransient<CurrentSeasonPresenter>();
            services.TryAddTransient<DiscoverPresenter>();
            services.TryAddTransient<HomePresenter>();
            services.TryAddTransient<LoginPresenter>();
            services.TryAddTransient<SearchResultsPresenter>();
            services.TryAddTransient<SettingsPresenter>();
            services.TryAddTransient<TorrentSearchPresenter>();
            services.TryAddTransient<UserPresenter>();
            services.TryAddTransient<MediaListSettingsPresenter>();
            services.TryAddTransient<StudioPresenter>();
            services.TryAddTransient<FavoritesPresenter>();

        }

        private static string ExtractResource(string filename, string location)
        {
            var a = Assembly.GetExecutingAssembly();
            using var resFilestream = a.GetManifestResourceStream(filename);

            if (resFilestream != null)
            {
                using var stream = File.Create(Path.Combine(location, filename));
                resFilestream.CopyTo(stream);
            }

            return Path.Combine(location, filename);
        }
    }
}
namespace Chaotic;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			 .UseSentry(options => {
				 // The DSN is the only required setting.
				 options.Dsn = builder.Configuration["Sentry:Dsn"];

				 // Use debug mode if you want to see what the SDK is doing.
				 // Debug messages are written to stdout with Console.Writeline,
				 // and are viewable in your IDE's debug console or with 'adb logcat', etc.
				 // This option is not recommended when deploying your application.
				 options.Debug = true;

				 // Enable logs to be sent to Sentry
				 options.EnableLogs = true;

			 })
			.UseMauiCommunityToolkit()
#if DEBUG
			.UseDebugRainbows(new DebugRainbowsOptions { })
#endif
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddSingleton<MainViewModel>();

		builder.Services.AddSingleton<SearchViewModel>();

		builder.Services.AddSingleton<AnimeListViewModel>();

		builder.Services.AddSingleton<AnimeViewModel>();

		builder.Services.AddSingleton<MangaListViewModel>();

		builder.Services.AddSingleton<MangaViewModel>();

		builder.Services.AddSingleton<CharacterViewModel>();

		builder.Services.AddSingleton<StaffViewModel>();

		builder.Services.AddSingleton<StudiosViewModel>();

		builder.Services.AddSingleton<UserInfoViewModel>();

		builder.Services.AddSingleton<FavoritesViewModel>();

		builder.Services.AddSingleton<ActivityViewModel>();

		return builder.Build();
	}
}

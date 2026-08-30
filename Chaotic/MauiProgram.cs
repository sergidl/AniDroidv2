namespace Chaotic;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseSentry(options =>
			{
				// TODO: Set the Sentry Dsn
				options.Dsn = "https://examplePublicKey@o0.ingest.sentry.io/0";
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

using System.Reflection;
using Microsoft.Extensions.Configuration;
namespace Chaotic;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();

		var assambly = typeof(MauiProgram).Assembly;
		using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("appsettings.json");

		builder.Configuration.AddJsonStream(stream);

		builder
			.UseMauiApp<App>()
			 .UseSentry(options => {
				 // The DSN is the only required setting.
                 options.Dsn = builder.Configuration["Secrets:SentryDsn"];

				 // Use debug mode if you want to see what the SDK is doing.
				 // Debug messages are written to stdout with Console.Writeline,
				 // and are viewable in your IDE's debug console or with 'adb logcat', etc.
				 // This option is not recommended when deploying your application.
				 options.Debug = true;

				 // Enable logs to be sent to Sentry
				 options.EnableLogs = true;

			 })
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddTransient<MainPage>();

		builder.Services.AddTransient<LoginPage>();

        builder.Services.AddTransient<SearchPage>();

		builder.Services.AddTransient<AnimeListPage>();

		builder.Services.AddTransient<AnimePage>();

		builder.Services.AddTransient<MangaListPage>();

		builder.Services.AddTransient<MangaPage>();

		builder.Services.AddTransient<CharacterPage>();

		builder.Services.AddTransient<StaffPage>();

		builder.Services.AddTransient<StudiosPage>();

		builder.Services.AddTransient<UserInfoPage>();

		builder.Services.AddTransient<FavoritesPage>();

		builder.Services.AddTransient<ActivityPage>();



        builder.Services.AddTransient<MainViewModel>();

        builder.Services.AddTransient<LoginViewModel>();

        builder.Services.AddTransient<SearchViewModel>();

        builder.Services.AddTransient<AnimeListViewModel>();

        builder.Services.AddTransient<AnimeViewModel>();

        builder.Services.AddTransient<MangaListViewModel>();

        builder.Services.AddTransient<MangaViewModel>();

        builder.Services.AddTransient<CharacterViewModel>();

        builder.Services.AddTransient<StaffViewModel>();

        builder.Services.AddTransient<StudiosViewModel>();

        builder.Services.AddTransient<UserInfoViewModel>();

        builder.Services.AddTransient<FavoritesViewModel>();

        builder.Services.AddTransient<ActivityViewModel>();

        return builder.Build();
	}
}

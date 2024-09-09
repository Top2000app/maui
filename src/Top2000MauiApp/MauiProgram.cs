using Microsoft.Extensions.Logging;
using Top2000.Data.ClientDatabase;
using Top2000.Features.SQLite;
using Top2000MauiApp.AskForReview;
using Top2000MauiApp.Globalisation;
using Top2000MauiApp.NavigationShell;
using Top2000MauiApp.Themes;

namespace Top2000MauiApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<Top2000MauiApp.XamarinForms.App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("MaterialIcons.ttf", "MaterialIcons");
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<ILocalisationService, LocalisationService>();
        // services.AddTransient<IStoreReview, StoreReviewImplementation>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services
            .AddClientDatabase(new DirectoryInfo(FileSystem.Current.AppDataDirectory))
            .AddFeaturesWithSQLite()
            .AddSingleton<IThemeService, ThemeService>()
            .AddTransient<Top2000MauiApp.Overview.Position.ViewModel>()
            .AddTransient<Top2000MauiApp.Overview.Date.ViewModel>()
            .AddTransient<Top2000MauiApp.TrackInformation.ViewModel>()
            .AddTransient<Top2000MauiApp.Searching.ViewModel>()
            .AddTransient<IAskForReview, ReviewModule>()
            // .AddSingleton<Xamarin.Essentials.Interfaces.IPreferences, Xamarin.Essentials.Implementation.PreferencesImplementation>()
            .AddSingleton<ICulture>(new SupportedCulture("nl"))
            .AddSingleton<ICulture>(new SupportedCulture("en"))
            .AddSingleton<ICulture>(new SupportedCulture("fr"))
        ;

        if (IsTop2000Live())
        {
            builder.Services.AddSingleton<IMainShell, Top2000MauiApp.NavigationShell.LiveTop2000.View>();
        }
        else
        {
            builder.Services.AddSingleton<IMainShell, Top2000MauiApp.NavigationShell.View>();
        }

        // builder.Services.Configure<AskForReviewConfiguration>(builder.Configuration.GetSection(nameof(AskForReviewConfiguration)));

        var serviceProvider = builder.Build();

        Top2000MauiApp.XamarinForms.App.ServiceProvider = serviceProvider.Services;
        Top2000MauiApp.XamarinForms.App.EnsureDatabaseIsCreatedAsync().GetAwaiter().GetResult();


        return serviceProvider;
    }

    private static bool IsTop2000Live()
    {
        var current = DateTime.UtcNow;

        var first = new DateTime(current.Year, 12, 24, 23, 0, 0, DateTimeKind.Utc); // first day of Christmas for CET in UTC time
        var last = new DateTime(current.Year, 12, 31, 23, 0, 0, DateTimeKind.Utc); // new year for CET in UTC time

        return current > first && current < last;
    }
}

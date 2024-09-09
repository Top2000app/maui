using Top2000MauiApp.Globalisation;
using Top2000MauiApp.NavigationShell;
using Top2000MauiApp.Themes;
using Top2000.Data.ClientDatabase;
using System.Globalization;


namespace Top2000MauiApp.XamarinForms
{
    public partial class App : Application
    {
        public static readonly IFormatProvider DateTimeFormatProvider = DateTimeFormatInfo.InvariantInfo;

        public static readonly IFormatProvider NumberFormatProvider = NumberFormatInfo.InvariantInfo;

        private static IServiceProvider? serviceProvider;

        public App()
        {
            InitializeComponent();
            SetCulture();
            SetTheme();

            var navigationShell = GetService<IMainShell>();

            MainPage = navigationShell as Shell ??
                throw new NotSupportedException($"{nameof(IMainShell)} must be implemented inside a Shell view");

            navigationShell.SetTitles();
        }

        public static IServiceProvider ServiceProvider
        {
            get
            {
                return serviceProvider ??
                    throw new InvalidOperationException("Application isn't booted yet");
            }
            set
            {
                serviceProvider = value;
            }
        }

        public static T GetService<T>() where T : notnull => ServiceProvider.GetRequiredService<T>();

        public static Task EnsureDatabaseIsCreatedAsync()
        {
            var databaseGen = GetService<IUpdateClientDatabase>();
            var top2000 = GetService<Top2000AssemblyDataSource>();

            return databaseGen.RunAsync(top2000);
        }

        public static async Task CheckForOnlineUpdates()
        {
            try
            {
                await Task.Delay(3_1000);
                var databasGen = GetService<IUpdateClientDatabase>();
                var onlineStore = GetService<OnlineDataSource>();

                await databasGen.RunAsync(onlineStore);
            }
            catch
            {
                // I don't want a crash here, just continue. 
            }
        }

        protected override void OnStart()
        {
          //  AppCenter.Start("89fbeb5b-5ec9-4456-86c7-214421330f73", typeof(Analytics), typeof(Crashes));
        }

        private void SetCulture()
        {
            var localisationService = App.GetService<ILocalisationService>();
            localisationService.SetCultureFromSetting();
        }

        private void SetTheme()
        {
            var themeService = GetService<IThemeService>();
            themeService.SetThemeFromSetting();
        }
    }
}

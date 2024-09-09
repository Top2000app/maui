using Microsoft.Maui;
using Microsoft.Maui.Controls.Xaml;

namespace Top2000MauiApp.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class About : ContentPage
    {
        public About()
        {
            InitializeComponent();
        }

        async private void GoToFacebook(object sender, System.EventArgs e)
        {
            await Launcher.OpenAsync(new Uri("https://www.facebook.com/Top2000App/"));
        }

        async private void MailMe(object sender, EventArgs e)
        {
            await Launcher.OpenAsync(new Uri("mailto:rick.neeft@outlook.com"));
        }
    }
}

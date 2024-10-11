namespace Top2000MauiApp.Pages.Settings;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class About : ContentPage
{
    public About()
    {
        InitializeComponent();
    }

    private async void GoToFacebook(object sender, System.EventArgs e)
    {
        await Launcher.OpenAsync(new Uri("https://www.facebook.com/Top2000App/"));
    }

    private async void MailMe(object sender, EventArgs e)
    {
        await Launcher.OpenAsync(new Uri("mailto:rick.neeft@outlook.com"));
    }
}

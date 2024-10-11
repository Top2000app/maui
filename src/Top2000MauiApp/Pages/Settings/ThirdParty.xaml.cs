using Top2000MauiApp.Globalisation;

namespace Top2000MauiApp.Pages.Settings;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ThirdParty : ContentPage
{
    public ThirdParty()
    {
        InitializeComponent();

        var source = new HtmlWebViewSource
        {
            Html = Translator.Instance["Credits"]
        };

        WebViewer.Source = source;
    }
}

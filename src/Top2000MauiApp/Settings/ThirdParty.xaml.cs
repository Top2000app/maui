using Top2000MauiApp.Globalisation;

using Microsoft.Maui.Controls.Xaml;

namespace Top2000MauiApp.Settings
{
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
}

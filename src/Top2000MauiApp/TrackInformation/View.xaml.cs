using Top2000MauiApp.XamarinForms;
using System;
using System.Threading.Tasks;


using Microsoft.Maui.Controls.Xaml;

namespace Top2000MauiApp.TrackInformation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View : Grid
    {
        public View()
        {
            BindingContext = App.GetService<ViewModel>();
            InitializeComponent();
        }

        public ViewModel ViewModel => (ViewModel)BindingContext;

        private Func<Task>? OnClose { get; set; }

        async public Task LoadTrackDetailsAsync(int trackId, Func<Task> onClose)
        {
            this.OnClose = onClose;
            await positionsScroll.ScrollToAsync(0, 0, animated: false);

            await ViewModel.LoadTrackDetailsAsync(trackId);
        }

        async private void OnCloseButtonClick(object sender, System.EventArgs e)
        {
            if (OnClose != null)
            {
                await OnClose.Invoke();
            }
        }

        async private void OnViewVideoClick(object sender, EventArgs e)
        {
            var trackTitle = ViewModel.Title.Replace(' ', '+');
            var artistName = ViewModel.Artist.Replace(' ', '+');

            var url = new Uri($"https://duckduckgo.com/?q=!ducky+onsite:www.youtube.com+{trackTitle}+{artistName}");

            await Launcher.OpenAsync(url);
        }
    }
}

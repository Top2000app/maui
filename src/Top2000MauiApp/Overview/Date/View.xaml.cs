using Top2000MauiApp.XamarinForms;

namespace Top2000MauiApp.Overview.Date
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View : ContentPage
    {
        public View()
        {
            this.BindingContext = App.GetService<ViewModel>();
            this.InitializeComponent();
        }

        public ViewModel ViewModel => (ViewModel)this.BindingContext;

        private static int FirstVisibleItemIndex { get; set; }

        public async Task ScrollToCorrectPositionAsync(int index, ScrollToPosition scrollToPosition = ScrollToPosition.Start)
        {
            var tries = 0;

            while (index != FirstVisibleItemIndex && tries < 6)
            {
                listings.ScrollTo(index, position: scrollToPosition, animate: false);

                tries++;

                await Task.Delay(200);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (GroupFlyout.IsVisible)
            {
                Shell.SetTabBarIsVisible(this, true);
                Shell.SetNavBarIsVisible(this, true);
                GroupFlyout.TranslateTo(this.Width * -1, 0);
                GroupFlyout.IsVisible = false;

                return true;
            }

            if (trackInformation.IsVisible)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                this.CloseTrackInformationAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                return true;
            }

            return base.OnBackButtonPressed();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (this.ViewModel.Listings.Count == 0)
            {
                await this.ViewModel.InitialiseViewModelAsync();
            }

            await this.JumpWhenTop2000IsOn();
        }

        private async Task JumpWhenTop2000IsOn()
        {
            var first = this.ViewModel.Listings[0].Key;
            var last = this.ViewModel.Listings[0].Key;
            var current = DateTime.Now;

            if (current > first && current < last)
            {
                await this.JumpToSelectedDateTime(current);
            }
            else
            {
                this.ToolbarItems.Remove(jumpToToday);
            }
        }

        private async void OnJumpGroupButtonClick(object sender, EventArgs e)
        {
            Shell.SetNavBarIsVisible(this, false);
            Shell.SetTabBarIsVisible(this, false);

            await GroupFlyout.TranslateTo(this.Width * -1, 0, 0);

            GroupFlyout.IsVisible = true;
            await GroupFlyout.TranslateTo(0, 0);
        }

        private async void OnGroupSelected(object sender, SelectionChangedEventArgs e)
        {
            if (dates.SelectedItem is DateTime date)
            {
                Shell.SetTabBarIsVisible(this, true);
                Shell.SetNavBarIsVisible(this, true);
                await GroupFlyout.TranslateTo(this.Width * -1, 0);
                GroupFlyout.IsVisible = false;

                await this.JumpToSelectedDateTime(date);
                dates.SelectedItem = null;
            }
        }

        private async Task JumpToSelectedDateTime(DateTime selectedDate)
        {
            var tracksGrouped = this.ViewModel.Listings;
            var groupsBefore = tracksGrouped.Where(x => x.Key <= selectedDate);
            var group = groupsBefore.LastOrDefault();

            if (group != null)
            {
                var firstGroup = group.FirstOrDefault();
                if (firstGroup != null)
                {
                    var position = group.First().Position;
                    var totalTracks = this.ViewModel.Listings.SelectMany(x => x).Count();
                    var groupsBeforeCount = groupsBefore.Count();

                    const int ShowGroup = 1;
                    var index = totalTracks - position + groupsBeforeCount - ShowGroup;

                    if (totalTracks == 500)
                    {
                        index = totalTracks - (position - 2000) + groupsBeforeCount - ShowGroup;
                    }


                    if (index < 0)
                    {
                        index = 0;
                    }

                    await this.ScrollToCorrectPositionAsync(index);
                }
            }
        }

        private void Tracks_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            FirstVisibleItemIndex = e.FirstVisibleItemIndex;
        }

        private async void OpenTodayClick(object sender, EventArgs e)
        {
            await this.JumpToSelectedDateTime(DateTime.Now);
        }

        private async void OnListingSelected(object sender, SelectionChangedEventArgs e)
        {
            if (this.ViewModel.SelectedListing is null || trackInformation.IsVisible)
            {
                return;
            }

            var infoTask = trackInformation.LoadTrackDetailsAsync(this.ViewModel.SelectedListing.TrackId, this.CloseTrackInformationAsync);

            Shell.SetNavBarIsVisible(this, false);
            Shell.SetTabBarIsVisible(this, false);

            await trackInformation.TranslateTo(this.Width * -1, 0, 0);

            trackInformation.IsVisible = true;
            await trackInformation.TranslateTo(0, 0);

            await infoTask;
        }

        private async Task CloseTrackInformationAsync()
        {
            this.ViewModel.SelectedListing = null;

            Shell.SetTabBarIsVisible(this, true);
            Shell.SetNavBarIsVisible(this, true);
            await trackInformation.TranslateTo(this.Width * -1, 0);
            trackInformation.IsVisible = false;
        }
    }
}

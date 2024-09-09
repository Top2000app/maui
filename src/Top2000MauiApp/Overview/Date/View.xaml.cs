using Top2000MauiApp.XamarinForms;

namespace Top2000MauiApp.Overview.Date
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View : ContentPage
    {
        public View()
        {
            BindingContext = App.GetService<ViewModel>();
            InitializeComponent();
        }

        public ViewModel ViewModel => (ViewModel)BindingContext;

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
            if (this.GroupFlyout.IsVisible)
            {
                Shell.SetTabBarIsVisible(this, true);
                Shell.SetNavBarIsVisible(this, true);
                this.GroupFlyout.TranslateTo(this.Width * -1, 0);
                this.GroupFlyout.IsVisible = false;

                return true;
            }

            if (this.trackInformation.IsVisible)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                CloseTrackInformationAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                return true;
            }

            return base.OnBackButtonPressed();
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModel.Listings.Count == 0)
            {
                await ViewModel.InitialiseViewModelAsync();
            }

            await JumpWhenTop2000IsOn();
        }

        async private Task JumpWhenTop2000IsOn()
        {
            var first = ViewModel.Listings.First().Key;
            var last = ViewModel.Listings.Last().Key;
            var current = DateTime.Now;

            if (current > first && current < last)
            {
                await JumpToSelectedDateTime(current);
            }
            else
            {
                this.ToolbarItems.Remove(jumpToToday);
            }
        }

        async private void OnJumpGroupButtonClick(object sender, EventArgs e)
        {
            Shell.SetNavBarIsVisible(this, false);
            Shell.SetTabBarIsVisible(this, false);

            await GroupFlyout.TranslateTo(this.Width * -1, 0, 0);

            GroupFlyout.IsVisible = true;
            await GroupFlyout.TranslateTo(0, 0);
        }

        async private void OnGroupSelected(object sender, SelectionChangedEventArgs e)
        {
            if (dates.SelectedItem is DateTime date)
            {
                Shell.SetTabBarIsVisible(this, true);
                Shell.SetNavBarIsVisible(this, true);
                await GroupFlyout.TranslateTo(this.Width * -1, 0);
                this.GroupFlyout.IsVisible = false;

                await JumpToSelectedDateTime(date);
                dates.SelectedItem = null;
            }
        }

        private async Task JumpToSelectedDateTime(DateTime selectedDate)
        {
            var tracksGrouped = ViewModel.Listings;
            var groupsBefore = tracksGrouped.Where(x => x.Key <= selectedDate);
            var group = groupsBefore.LastOrDefault();

            if (group != null)
            {
                var firstGroup = group.FirstOrDefault();
                if (firstGroup != null)
                {
                    var position = group.First().Position;
                    var totalTracks = ViewModel.Listings.SelectMany(x => x).Count();
                    var groupsBeforeCount = groupsBefore.Count();

                    const int ShowGroup = 1;
                    var index = totalTracks - position + groupsBeforeCount - ShowGroup;

                    if (totalTracks == 500)
                    {
                        index = totalTracks - (position - 2000) + groupsBeforeCount - ShowGroup;
                    }


                    if (index < 0) index = 0;

                    await ScrollToCorrectPositionAsync(index);
                }
            }
        }

        private void Tracks_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            FirstVisibleItemIndex = e.FirstVisibleItemIndex;
        }

        async private void OpenTodayClick(object sender, EventArgs e)
        {
            await JumpToSelectedDateTime(DateTime.Now);
        }

        async private void OnListingSelected(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.SelectedListing is null) return;

            var infoTask = trackInformation.LoadTrackDetailsAsync(ViewModel.SelectedListing.TrackId, CloseTrackInformationAsync);

            Shell.SetNavBarIsVisible(this, false);
            Shell.SetTabBarIsVisible(this, false);

            await trackInformation.TranslateTo(this.Width * -1, 0, 0);

            trackInformation.IsVisible = true;
            await trackInformation.TranslateTo(0, 0);

            await infoTask;
        }

        private async Task CloseTrackInformationAsync()
        {
            ViewModel.SelectedListing = null;

            Shell.SetTabBarIsVisible(this, true);
            Shell.SetNavBarIsVisible(this, true);
            await this.trackInformation.TranslateTo(this.Width * -1, 0);
            this.trackInformation.IsVisible = false;
        }
    }
}

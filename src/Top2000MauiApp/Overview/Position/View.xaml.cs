using Top2000MauiApp.Globalisation;
using Top2000MauiApp.XamarinForms;
using Top2000.Features.AllEditions;
using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Maui.Controls.Xaml;

namespace Top2000MauiApp.Overview.Position
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

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModel.Editions.Count == 0)
            {
                await ViewModel.InitialiseViewModelAsync();
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (this.EditionsFlyout.IsVisible)
            {
                Shell.SetTabBarIsVisible(this, true);
                Shell.SetNavBarIsVisible(this, true);
                this.EditionsFlyout.TranslateTo(this.Width * -1, 0);
                this.EditionsFlyout.IsVisible = false;

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

        async private void OnSelectYearButtonClick(object sender, System.EventArgs e)
        {
            Shell.SetNavBarIsVisible(this, false);
            Shell.SetTabBarIsVisible(this, false);

            await EditionsFlyout.TranslateTo(this.Width * -1, 0, 0);

            EditionsFlyout.IsVisible = true;
            await EditionsFlyout.TranslateTo(0, 0);
        }

        async private void OnJumpGroupButtonClick(object sender, System.EventArgs e)
        {
            var groups = ViewModel.Listings.Select(x => x.Key)
                                         .ToArray();

            var result = await DisplayActionSheet(AppResources.JumpToGroup, AppResources.Cancel, null, groups);

            if (!string.IsNullOrWhiteSpace(result) && result != AppResources.Cancel)
            {
                JumpIntoList(result);
            }
        }

        private void JumpIntoList(string groupElected)
        {
            var groupIndex = ViewModel.Listings.FindIndex(x => x.Key == groupElected);
            var group = ViewModel.Listings.Single(x => x.Key == groupElected);

            var position = group.First().Position;

            const int ShowGroup = 1;
            var index = position + groupIndex - ShowGroup;

            if (ViewModel.CountOfItems == 500)
            {
                index = index - 2000;
            }

            if (index < 0) index = 0;

            listings.ScrollTo(index, position: ScrollToPosition.Start, animate: false);
        }

        async private void NewEditionSelected(object sender, SelectionChangedEventArgs e)
        {
            if (AllEditions.SelectedItem is Edition edition)
            {
                ViewModel.SelectedEdition = edition;
                ViewModel.SelectedEditionYear = edition.Year;

                var loadingTask = ViewModel.LoadAllListingsAsync();

                Shell.SetTabBarIsVisible(this, true);
                Shell.SetNavBarIsVisible(this, true);
                await EditionsFlyout.TranslateTo(this.Width * -1, 0);
                this.EditionsFlyout.IsVisible = false;

                await loadingTask;

                JumpIntoList(ViewModel.Listings.First().Key);

                AllEditions.SelectedItem = null;
            }
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

        async private void OnCloseButtonClick(object sender, EventArgs e)
        {
            Shell.SetTabBarIsVisible(this, true);
            Shell.SetNavBarIsVisible(this, true);
            await EditionsFlyout.TranslateTo(this.Width * -1, 0);
            this.EditionsFlyout.IsVisible = false;
        }
    }
}

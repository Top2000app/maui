using Top2000.Features.AllEditions;
using Top2000MauiApp.Globalisation;

namespace Top2000MauiApp.Pages.Overview.Position;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class View : ContentPage
{
    public View()
    {
        this.BindingContext = App.GetService<ViewModel>();
        this.InitializeComponent();
    }

    public ViewModel ViewModel => (ViewModel)this.BindingContext;

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (this.ViewModel.Editions.Count == 0)
        {
            await this.ViewModel.InitialiseViewModelAsync();
        }
    }

    protected override bool OnBackButtonPressed()
    {
        if (EditionsFlyout.IsVisible)
        {
            Shell.SetTabBarIsVisible(this, true);
            Shell.SetNavBarIsVisible(this, true);
            EditionsFlyout.TranslateTo(this.Width * -1, 0);
            EditionsFlyout.IsVisible = false;

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

    private async void OnSelectYearButtonClick(object sender, System.EventArgs e)
    {
        Shell.SetNavBarIsVisible(this, false);
        Shell.SetTabBarIsVisible(this, false);

        await EditionsFlyout.TranslateTo(this.Width * -1, 0, 0);

        EditionsFlyout.IsVisible = true;
        await EditionsFlyout.TranslateTo(0, 0);
    }

    private async void OnJumpGroupButtonClick(object sender, System.EventArgs e)
    {
        var groups = this.ViewModel.Listings.Select(x => x.Key)
                                     .ToArray();

        var result = await this.DisplayActionSheet(AppResources.JumpToGroup, AppResources.Cancel, null, groups);

        if (!string.IsNullOrWhiteSpace(result) && result != AppResources.Cancel)
        {
            this.JumpIntoList(result);
        }
    }

    private void JumpIntoList(string groupElected)
    {
        var group = this.ViewModel.Listings.Single(x => x.Key == groupElected);

        listings.ScrollTo(group.First(), position: ScrollToPosition.Center, animate: false);
    }

    private async void NewEditionSelected(object sender, SelectionChangedEventArgs e)
    {
        if (AllEditions.SelectedItem is Edition edition)
        {
            this.ViewModel.SelectedEdition = edition;
            this.ViewModel.SelectedEditionYear = edition.Year;

            var loadingTask = this.ViewModel.LoadAllListingsAsync();

            Shell.SetTabBarIsVisible(this, true);
            Shell.SetNavBarIsVisible(this, true);
            await EditionsFlyout.TranslateTo(this.Width * -1, 0);
            EditionsFlyout.IsVisible = false;

            await loadingTask;

            this.JumpIntoList(this.ViewModel.Listings[0].Key);

            AllEditions.SelectedItem = null;
        }
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

    private async void OnCloseButtonClick(object sender, EventArgs e)
    {
        Shell.SetTabBarIsVisible(this, true);
        Shell.SetNavBarIsVisible(this, true);
        await EditionsFlyout.TranslateTo(this.Width * -1, 0);
        EditionsFlyout.IsVisible = false;
    }
}

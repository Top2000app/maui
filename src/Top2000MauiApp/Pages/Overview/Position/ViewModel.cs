using Top2000.Features.AllEditions;
using Top2000.Features.AllListingsOfEdition;
using Top2000MauiApp.Common;

namespace Top2000MauiApp.Pages.Overview.Position;

public partial class ViewModel : ObservableObject
{
    private readonly IMediator mediator;

    public ViewModel(IMediator mediator)
    {
        this.mediator = mediator;
        this.Listings = [];
        this.Editions = [];
    }

    public ObservableGroupedList<string, TrackListing> Listings { get; }

    [ObservableProperty]
    public TrackListing? selectedListing;

    public ObservableList<Edition> Editions { get; }

    [ObservableProperty]
    public Edition? selectedEdition;

    [ObservableProperty]
    public int selectedEditionYear;

    [ObservableProperty]
    public int countOfItems;

    public async Task InitialiseViewModelAsync()
    {
        var editions = await mediator.Send(new AllEditionsRequest());
        this.SelectedEdition = editions.First();
        this.SelectedEditionYear = this.SelectedEdition.Year;
        this.Editions.ClearAddRange(editions);

        await this.LoadAllListingsAsync();
    }

    public async Task InitialiseViewModelAsync(Edition newEdition)
    {
        this.SelectedEdition = newEdition;
        this.SelectedEditionYear = this.SelectedEdition.Year;

        await this.LoadAllListingsAsync();
    }

    public async Task LoadAllListingsAsync()
    {
        if (this.SelectedEdition is null)
        {
            return;
        }

        var listings = await mediator.Send(new AllListingsOfEditionRequest { Year = this.SelectedEdition.Year });
        this.CountOfItems = listings.Count;
        this.Listings.ClearAddRange(listings.GroupByPosition());

        this.SelectedListing = null;
    }
}

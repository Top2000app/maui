using Top2000.Features.AllEditions;
using Top2000.Features.AllListingsOfEdition;
using Top2000MauiApp.Common;

namespace Top2000MauiApp.Pages.Overview.Position;

public class ViewModel : ObservableBase
{
    private readonly IMediator mediator;

    public ViewModel(IMediator mediator)
    {
        this.mediator = mediator;
        this.Listings = [];
        this.Editions = [];
    }

    public ObservableGroupedList<string, TrackListing> Listings { get; }

    public TrackListing? SelectedListing
    {
        get { return this.GetPropertyValue<TrackListing?>(); }
        set { this.SetPropertyValue(value); }
    }

    public ObservableList<Edition> Editions { get; }

    public Edition? SelectedEdition
    {
        get { return this.GetPropertyValue<Edition?>(); }
        set { this.SetPropertyValue(value); }
    }

    public int SelectedEditionYear
    {
        get { return this.GetPropertyValue<int>(); }
        set { this.SetPropertyValue(value); }
    }

    public int CountOfItems
    {
        get { return this.GetPropertyValue<int>(); }
        set { this.SetPropertyValue(value); }
    }

    public async Task InitialiseViewModelAsync()
    {
        var editions = await mediator.Send(new AllEditionsRequest());
        this.SelectedEdition = editions.First();
        this.SelectedEditionYear = this.SelectedEdition.Year;
        this.Editions.ClearAddRange(editions);

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

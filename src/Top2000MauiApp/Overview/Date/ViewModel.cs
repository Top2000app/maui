using Top2000.Features.AllEditions;
using Top2000.Features.AllListingsOfEdition;
using Top2000MauiApp.Common;

namespace Top2000MauiApp.Overview.Date;

public class ViewModel : ObservableBase
{
    private readonly IMediator mediator;

    public ViewModel(IMediator mediator)
    {
        this.mediator = mediator;
        this.Listings = [];
        this.Dates = [];
    }

    public ObservableGroupedList<DateTime, TrackListing> Listings { get; }

    public ObservableGroupedList<DateTime, DateTime> Dates { get; }

    public int SelectedEditionYear
    {
        get { return this.GetPropertyValue<int>(); }
        set { this.SetPropertyValue(value); }
    }

    public TrackListing? SelectedListing
    {
        get { return this.GetPropertyValue<TrackListing?>(); }
        set { this.SetPropertyValue(value); }
    }

    public static DateTime LocalPlayDateAndTime(TrackListing listing) => listing.PlayUtcDateAndTime.ToLocalTime();

    public async Task InitialiseViewModelAsync()
    {
        var editions = await mediator.Send(new AllEditionsRequest());
        this.SelectedEditionYear = editions.First().Year;

        await this.LoadAllListingsAsync();
    }

    public async Task LoadAllListingsAsync()
    {
        var tracks = await mediator.Send(new AllListingsOfEditionRequest { Year = this.SelectedEditionYear });

        var listings = tracks
            .OrderByDescending(x => x.Position)
            .GroupBy(LocalPlayDateAndTime);

        var dates = listings
            .Select(x => x.Key)
            .GroupBy(this.LocalPlayDate);

        this.Listings.ClearAddRange(listings);
        this.Dates.ClearAddRange(dates);
    }

    private DateTime LocalPlayDate(DateTime arg) => arg.Date;
}

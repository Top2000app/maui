using Top2000.Features.TrackInformation;
using Top2000MauiApp.Common;

namespace Top2000MauiApp.TrackInformation;

public class ViewModel : ObservableBase
{
    private readonly IMediator mediator;

    public ViewModel(IMediator mediator)
    {
        this.mediator = mediator;
        this.Listings = [];
    }

    public string Title
    {
        get { return this.GetPropertyValue<string>(); }
        set { this.SetPropertyValue(value); }
    }

    public string Artist
    {
        get { return this.GetPropertyValue<string>(); }
        set { this.SetPropertyValue(value); }
    }

    public string ArtistWithYear
    {
        get { return this.GetPropertyValue<string>(); }
        set { this.SetPropertyValue(value); }
    }

    public ObservableList<ListingInformation> Listings { get; }

    public int TotalListings
    {
        get { return this.GetPropertyValue<int>(); }
        set { this.SetPropertyValue(value); }
    }

    public ListingInformation Highest
    {
        get { return this.GetPropertyValue<ListingInformation>(); }
        set { this.SetPropertyValue(value); }
    }

    public ListingInformation Lowest
    {
        get { return this.GetPropertyValue<ListingInformation>(); }
        set { this.SetPropertyValue(value); }
    }

    public ListingInformation Latest
    {
        get { return this.GetPropertyValue<ListingInformation>(); }
        set { this.SetPropertyValue(value); }
    }

    public ListingInformation First
    {
        get { return this.GetPropertyValue<ListingInformation>(); }
        set { this.SetPropertyValue(value); }
    }

    public int Appearances
    {
        get { return this.GetPropertyValue<int>(); }
        set { this.SetPropertyValue(value); }
    }

    public bool IsLatestListed
    {
        get { return this.GetPropertyValue<bool>(); }
        set { this.SetPropertyValue(value); }
    }

    public int AppearancesPossible
    {
        get { return this.GetPropertyValue<int>(); }
        set { this.SetPropertyValue(value); }
    }

    public int AppearancesPossiblePercentage
    {
        get { return this.GetPropertyValue<int>(); }
        set { this.SetPropertyValue(value); }
    }

    public int TotalTop2000Percentage
    {
        get { return this.GetPropertyValue<int>(); }
        set { this.SetPropertyValue(value); }
    }

    public async Task LoadTrackDetailsAsync(int trackId)
    {
        var track = await mediator.Send(new TrackInformationRequest { TrackId = trackId });

        this.Title = track.Title;
        this.ArtistWithYear = $"{track.Artist} ({track.RecordedYear})";
        this.Artist = track.Artist;
        this.Highest = track.Highest;
        this.Lowest = track.Lowest;
        this.Latest = track.Latest;
        this.First = track.First;
        this.Appearances = track.Appearances;
        this.AppearancesPossible = track.AppearancesPossible;
        this.IsLatestListed = track.Listings.First().Status != ListingStatus.NotListed;
        this.Listings.Clear();
        this.Listings.ClearAddRange(track.Listings);
        this.AppearancesPossiblePercentage = 100 * this.Appearances / this.AppearancesPossible;
        this.TotalTop2000Percentage = 100 * this.Appearances / this.Listings.Count;

        this.TotalListings = this.Listings.Count;
    }
}

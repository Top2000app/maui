using Top2000.Features.AllListingsOfEdition;
using Top2000MauiApp.Common;

namespace Top2000MauiApp.Pages.Overview;

public class TrackListingSymbolFontSizeConverter : ValueConverterBase<TrackListing, int>
{
    public override int Convert(TrackListing track)
    {
        var value = track.Delta;

        if (value is null || !value.HasValue || value.Value == 0)
        {
            return 20;
        }

        return 11;
    }
}

using Top2000.Features.AllListingsOfEdition;
using Top2000MauiApp.Common;
using Top2000MauiApp.Themes;


namespace Top2000MauiApp.Pages.Overview;

public class DeltaToSymbolColour : ValueConverterBase<TrackListing, Color>
{
    public override Color Convert(TrackListing track)
    {
        var value = track.Delta;

        if (!value.HasValue)
        {
            return Colours.YellowColour;
        }

        if (value.Value > 0)
        {
            return Colours.GreenColour;
        }

        if (value.Value < 0)
        {
            return Colours.RedColour;
        }

        return Colours.GreyColour;
    }
}

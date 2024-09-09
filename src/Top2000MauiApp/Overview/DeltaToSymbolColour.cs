using Top2000.Features.AllListingsOfEdition;
using Top2000MauiApp.Common;


namespace Top2000MauiApp.Overview;

public class DeltaToSymbolColour : ValueConverterBase<TrackListing, Color>
{
    private static readonly Color YellowColour = Color.FromRgba(255, 192, 0, 255);
    private static readonly Color RedColour = Color.FromRgba(221, 48, 57, 255);
    private static readonly Color GreenColour = Color.FromRgba(112, 173, 71, 255);
    private static readonly Color GreyColour = Color.FromRgba(103, 103, 103, 255);

    public override Color Convert(TrackListing track)
    {
        var value = track.Delta;

        if (!value.HasValue)
        {
            return YellowColour;
        }

        if (value.Value > 0)
        {
            return GreenColour;
        }

        if (value.Value < 0)
        {
            return RedColour;
        }

        return GreyColour;
    }
}

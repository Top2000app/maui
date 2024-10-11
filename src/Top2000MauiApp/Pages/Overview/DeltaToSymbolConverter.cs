using Top2000.Features.AllListingsOfEdition;
using Top2000MauiApp.Common;

namespace Top2000MauiApp.Pages.Overview;

public class DeltaToSymbolConverter : ValueConverterBase<TrackListing, string>
{
    public override string Convert(TrackListing track)
    {
        var value = track.Delta;

        if (value is null)
        {
            if (track.IsRecurring)
            {
                return Symbols.BackInList;
            }

            return Symbols.New;
        }

        if (value.Value > 0)
        {
            return Symbols.Up;
        }

        if (value.Value < 0)
        {
            return Symbols.Down;
        }

        return Symbols.Same;
    }
}

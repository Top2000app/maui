using Top2000.Features.TrackInformation;
using Top2000MauiApp.Common;

namespace Top2000MauiApp.TrackInformation;

public class StatusToSymbolConverter : ValueConverterBase<ListingStatus, string>
{
    public override string Convert(ListingStatus value) => value switch
    {
        ListingStatus.New => Symbols.Flag,
        ListingStatus.Decreased => Symbols.Down,
        ListingStatus.Increased => Symbols.Up,
        ListingStatus.Unchanged => Symbols.Same,
        ListingStatus.Back => Symbols.BackInList,
        _ => Symbols.Minus
    };
}

using Top2000.Features.TrackInformation;
using Top2000MauiApp.Common;


namespace Top2000MauiApp.TrackInformation;

public class StatusToColourConverter : ValueConverterBase<ListingStatus, Color>
{
    private static readonly Color YellowColour = Color.FromRgba(255, 192, 0, 255);
    private static readonly Color RedColour = Color.FromRgba(221, 48, 57, 255);
    private static readonly Color GreenColour = Color.FromRgba(112, 173, 71, 255);
    private static readonly Color GreyColour = Color.FromRgba(103, 103, 103, 255);

    public override Color Convert(ListingStatus value) => value switch
    {
        ListingStatus.New => YellowColour,
        ListingStatus.Back => YellowColour,
        ListingStatus.Decreased => RedColour,
        ListingStatus.Increased => GreenColour,
        _ => GreyColour,
    };
}

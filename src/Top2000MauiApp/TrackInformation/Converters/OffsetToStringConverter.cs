using Top2000MauiApp.Common;
using Top2000MauiApp.XamarinForms;

namespace Top2000MauiApp.TrackInformation;

public class OffsetToStringConverter : ValueConverterBase<int?, string>
{
    public override string Convert(int? offset)
    {
        return offset.HasValue && offset.Value != 0
            ? this.PositiveInteger(offset.Value).ToString(App.NumberFormatProvider)
            : string.Empty;
    }

    private int PositiveInteger(int value)
    {
        return value < 0
            ? value * -1
            : value;
    }
}

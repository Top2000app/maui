using Top2000MauiApp.Common;
using Top2000MauiApp.XamarinForms;

namespace Top2000MauiApp.TrackInformation;

public class PositionToStringConverter : ValueConverterBase<int?, string>
{
    public override string Convert(int? value)
    {
        return value.HasValue
            ? value.Value.ToString(App.NumberFormatProvider)
            : "-";
    }
}

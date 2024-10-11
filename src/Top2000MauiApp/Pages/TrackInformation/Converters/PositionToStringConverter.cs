using Top2000MauiApp.Common;
using Top2000MauiApp.XamarinForms;

namespace Top2000MauiApp.Pages.TrackInformation.Converters;

public class PositionToStringConverter : ValueConverterBase<int?, string>
{
    public override string Convert(int? value)
    {
        return value.HasValue
            ? value.Value.ToString(App.NumberFormatProvider)
            : "-";
    }
}

using Top2000MauiApp.Common;

namespace Top2000MauiApp.Pages.Overview;

public class DeltaToStringConverter : ValueConverterBase<int?, string>
{
    public override string Convert(int? offset)
    {
        return offset.HasValue && offset.Value != 0
            ? PositiveInteger(offset.Value).ToString(App.NumberFormatProvider)
            : string.Empty;
    }

    private static int PositiveInteger(int value)
    {
        return value < 0
            ? value * -1
            : value;
    }
}

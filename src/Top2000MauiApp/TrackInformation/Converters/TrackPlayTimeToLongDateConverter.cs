using Top2000MauiApp.Common;
using Top2000MauiApp.XamarinForms;

namespace Top2000MauiApp.TrackInformation;

public class TrackPlayTimeToLongDateConverter : ValueConverterBase<DateTime, string>
{
    public override string Convert(DateTime value)
    {
        var hour = value.TimeOfDay.Hours;
        var untilHour = hour + 1;

        if (untilHour > 24)
        {
            untilHour = 0;
        }

        var date = value.ToString("dddd d MMMM yyyy", App.DateTimeFormatProvider);

        return $"{date} {hour}:00 - {untilHour}:00";
    }
}

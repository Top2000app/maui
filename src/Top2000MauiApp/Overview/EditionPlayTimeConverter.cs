using System.Globalization;
using Top2000MauiApp.Common;

namespace Top2000MauiApp.Overview;

public class EditionPlayTimeConverter : ValueConverterBase<DateTime, string>
{
    private const string ShortFormat = "dd MMM yyyy HH:mm";
    private static readonly IFormatProvider formatProvider = DateTimeFormatInfo.InvariantInfo;

    public override string Convert(DateTime dateTime) => dateTime.ToString(ShortFormat, formatProvider);
}

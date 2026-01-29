namespace Commons.Util;

public static class Converter
{
    public static decimal? ToBigDecimal(string? value)
    {
        return To(value, decimal.Parse);
    }

    public static decimal ToBigDecimal(double value)
    {
        return (decimal)value;
    }

    public static string? FromBigDecimal(decimal? value)
    {
        return From(value, v => v.ToString());
    }

    public static string FromDouble(double value)
    {
        return ((decimal)value).ToString();
    }

    public static int? ToInteger(string? value)
    {
        return To(value, int.Parse);
    }

    public static string? FromInteger(int? value)
    {
        return From(value, v => v.ToString());
    }

    public static double? ToDouble(string? value)
    {
        return To(value, double.Parse);
    }

    public static DateTime? ToInstant(string? value)
    {
        return To(value, DateTime.Parse);
    }

    public static string? FromInstant(DateTime? value)
    {
        return From(value, v => v.ToString("O")); // ISO 8601 format
    }

    private static string? From<TSource>(TSource? value, Func<TSource, string> converter)
        where TSource : struct
    {
        return value == null ? null : converter(value.Value);
    }

    private static TResult? To<TResult>(string? value, Func<string, TResult> converter)
        where TResult : struct
    {
        return string.IsNullOrEmpty(value) ? null : converter(value);
    }
}

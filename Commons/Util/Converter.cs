namespace Commons.Util;

public static class Converter
{
    public static decimal? ToDecimal(string? value)
    {
        if (value == null)
        {
            return null;
        }
        return decimal.Parse(value);
    }

    public static decimal ToDecimal(double value)
    {
        return (decimal)value;
    }

    public static string? FromDecimal(decimal? value)
    {
        if (value == null)
        {
            return null;
        }
        return value.Value.ToString();
    }

    public static string FromDouble(double value)
    {
        return ((decimal)value).ToString();
    }

    public static int? ToInteger(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }
        return int.Parse(value);
    }

    public static string? FromInteger(int? value)
    {
        if (value == null)
        {
            return null;
        }
        return value.Value.ToString();
    }

    public static double? ToDouble(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }
        return double.Parse(value);
    }

    public static DateTime? ToInstant(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }
        return DateTime.Parse(value);
    }

    public static string? FromInstant(DateTime? value)
    {
        if (value == null)
        {
            return null;
        }
        return value.Value.ToString("O"); // ISO 8601 format
    }
}

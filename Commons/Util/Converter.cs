namespace Commons.Util;

public static class Converter
{
    public static decimal ToDecimal(string? value)
    {
        if (value == null)
        {
            return 0m;
        }
        return decimal.Parse(value);
    }

    public static decimal ToDecimal(double value)
    {
        return (decimal)value;
    }

    public static string FromDecimal(decimal value)
    {
        return value.ToString();
    }

    public static string FromDouble(double value)
    {
        return ((decimal)value).ToString();
    }
}

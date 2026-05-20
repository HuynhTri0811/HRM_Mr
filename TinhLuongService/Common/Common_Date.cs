public static class Common_Date
{
    public static DateTime StartOfMonth(this DateTime dateTime) => new DateTime(dateTime.Year, dateTime.Month, 1);
    public static DateTime EndOfMonth(this DateTime dateTime) => new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
    public static DateTime AddMilliseconds(this DateTime dateTime)
    {
        return new DateTime(dateTime.Ticks - (dateTime.Ticks % TimeSpan.TicksPerMillisecond), DateTimeKind.Utc);
    }
}
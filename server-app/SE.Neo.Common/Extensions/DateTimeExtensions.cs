namespace SE.Neo.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime AddWorkDays(this DateTime originalDate, int workDays)
        {
            int weeks = workDays / 5;
            workDays %= 5;
            while (originalDate.DayOfWeek == DayOfWeek.Saturday || originalDate.DayOfWeek == DayOfWeek.Sunday)
                originalDate = originalDate.AddDays(1);

            while (workDays-- > 0)
            {
                originalDate = originalDate.AddDays(1);
                if (originalDate.DayOfWeek == DayOfWeek.Saturday)
                    originalDate = originalDate.AddDays(2);
            }
            return originalDate.AddDays(weeks * 7);
        }
    }
}
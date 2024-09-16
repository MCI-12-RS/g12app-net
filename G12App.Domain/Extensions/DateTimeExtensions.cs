using G12App.Domain.Model;
using System;

namespace G12App.Domain.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime GetStartOfWeek(this DateTime date)
        {
            return date.Date.AddDays(
                (int)SystemConstants.BrazilCulture.DateTimeFormat.FirstDayOfWeek -
                (int)date.DayOfWeek);
        }

        public static DateTime GetEndOfWeek(this DateTime date)
        {
            return GetStartOfWeek(date) + TimeSpan.FromDays(7) - TimeSpan.FromSeconds(1);
        }
    }
}

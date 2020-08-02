using System;
using NodaTime;
using NodaTime.Text;

namespace Meteor.Utils
{
    public static class NodaTimeExtensions
    {
        private static readonly LocalDatePattern LocalDatePattern =
            LocalDatePattern.CreateWithInvariantCulture("yyyy-MM-dd");

        private static readonly InstantPattern InstantPattern =
            InstantPattern.CreateWithInvariantCulture("yyyy-MM-dd HH:mm:ss");

        public static string ToDateString(this LocalDate date) =>
            LocalDatePattern.Format(date);

        public static ParseResult<LocalDate> ToLocalDate(this string date) =>
            LocalDatePattern.Parse(date);

        public static string ToInstantString(this Instant date) =>
            InstantPattern.Format(date);

        public static ParseResult<Instant> ToInstant(this string date) =>
            InstantPattern.Parse(date);

        public static Instant ToInstant(this DateTime dateTime) =>
            Instant.FromDateTimeUtc(dateTime.ToUniversalTime());

        public static Instant? ToInstant(this DateTime? dateTime) =>
            dateTime.HasValue ? Instant.FromDateTimeUtc(dateTime.Value.ToUniversalTime()) : (Instant?) null;
    }
}
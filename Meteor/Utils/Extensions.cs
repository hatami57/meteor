using System.Globalization;

namespace Meteor.Utils
{
    public static class Extensions
    {
        public static string ToInvariantString(this decimal n) =>
            n.ToString(CultureInfo.InvariantCulture);
    }
}
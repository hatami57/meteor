using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Meteor.Utils
{
    public static class Extensions
    {
        public static string ToInvariantString(this decimal n) =>
            n.ToString(CultureInfo.InvariantCulture);

        public static async Task<TOut> Then<T, TOut>(this Task<T> task, Func<T, Task<TOut>> func) =>
            await func(await task.ConfigureAwait(false))
                .ConfigureAwait(false);
    }
}
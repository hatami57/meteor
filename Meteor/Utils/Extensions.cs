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

        public static async Task<int> ValidateDatabaseAffectedRows(this Task<int> task, object? errorDetails = null)
        {
            var res = await task.ConfigureAwait(false);
            if (res <= 0)
                throw Errors.DatabaseError(errorDetails);
            
            return res;
        }
        
        public static async Task<bool> ValidateDatabaseResult(this Task<bool> task, object? errorDetails = null)
        {
            var res = await task.ConfigureAwait(false);
            if (!res)
                throw Errors.DatabaseError(errorDetails);
            
            return res;
        }
    }
}
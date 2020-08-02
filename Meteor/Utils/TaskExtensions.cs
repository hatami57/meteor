using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meteor.Utils
{
    public static class TaskExtensions
    {
        public static async Task<List<T>> ToListAsync<T>(this Task<IEnumerable<T>> task) =>
            (await task.ConfigureAwait(false)).ToList();
    }
}
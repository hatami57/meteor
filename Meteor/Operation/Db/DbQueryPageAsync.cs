using System.Threading.Tasks;
using Meteor.Database;
using Meteor.Database.SqlDialect;
using Meteor.Utils;

namespace Meteor.Operation.Db
{
    public abstract class DbQueryPageAsync<TInput, TOutput> : DbOperationAsync<TInput, QueryPage<TOutput>>
        where TInput : IQueryPageInput
    {
        protected override Task<TInput> PrepareInputAsync()
        {
            if (Input.Page <= 0)
                Input.Page = 1;
            if (Input.Take <= 0)
                Input.Take = 10;

            return Task.FromResult(Input);
        }
    }
}
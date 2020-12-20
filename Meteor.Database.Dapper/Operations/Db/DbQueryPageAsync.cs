using System.Threading.Tasks;

namespace Meteor.Database.Dapper.Operations.Db
{
    public abstract class DbQueryPageAsync<TInput, TOutput> : DbOperationAsync<TInput, QueryPage<TOutput>>
        where TInput : Database.Dapper.Operations.Db.IQueryPageInput
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
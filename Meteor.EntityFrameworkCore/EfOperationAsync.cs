using Meteor.Operation;
using Meteor.Utils;
using Microsoft.EntityFrameworkCore;

namespace Meteor.EntityFrameworkCore
{
    public abstract class EfOperationAsync<TDbContext, TInput, TOutput> : OperationAsync<TInput, TOutput>
        where TDbContext : DbContext
    {
        public TDbContext? DbContext { get; set; }

        public override IOperationAsync SetOperationFactory(OperationFactory operationFactory)
        {
            base.SetOperationFactory(operationFactory);

            DbContext = OperationFactory?.GetService<TDbContext>() ??
                        throw Errors.InvalidOperation($"no_db_context={typeof(TDbContext).Name}");
            
            return this;
        }
    }
}
using System;
using System.Data;
using System.Threading.Tasks;
using Meteor.Database;
using Meteor.Database.SqlDialect;
using Meteor.Utils;

namespace Meteor.Operation.Db
{
	public abstract class DbOutOperationAsync<TOut> : DbOperationAsync<NoType, TOut>
	{
		public DbOutOperationAsync(LazyDbConnection lazyDbConnection, ISqlFactory sqlFactory) : base(lazyDbConnection, sqlFactory)
		{
		}
	}
}
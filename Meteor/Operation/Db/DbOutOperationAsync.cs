using System;
using System.Data;
using System.Threading.Tasks;
using Meteor.Database;
using Meteor.Database.SqlDialect;
using Meteor.Utils;

namespace Meteor.Operation.Db
{
	public abstract class DbInOperationAsync<TIn> : DbOperationAsync<TIn, NoType>
	{
		public DbInOperationAsync(LazyDbConnection lazyDbConnection, ISqlFactory sqlFactory) : base(lazyDbConnection, sqlFactory)
		{
		}
	}
}
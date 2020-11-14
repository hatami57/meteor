using System;
using System.Data;
using System.Threading.Tasks;
using Meteor.Database;
using Meteor.Database.SqlDialect;
using Meteor.Utils;

namespace Meteor.Operation.Db
{
	public abstract class DbOperationAsync<TInput, TOutput> : OperationAsync<TInput, TOutput>
	{
		public LazyDbConnection LazyDbConnection { get; private set; }
		protected ISqlFactory SqlFactory { get; private set; }

		public DbOperationAsync(LazyDbConnection lazyDbConnection, ISqlFactory sqlFactory)
		{
			LazyDbConnection = lazyDbConnection;
			SqlFactory = sqlFactory;
		}

		/// <summary>
		/// Ensure execute a function with a transaction.
		/// It use the Transaction object if it already existed,
		/// or create a new one with a given isolation level.
		/// If it creates a new transaction, it always commit after the function execution.
		/// If rollback is needed, the function should throws.
		/// </summary>
		/// <param name="func"></param>
		/// <param name="isolationLevel"></param>
		/// <typeparam name="TOut"></typeparam>
		/// <returns></returns>
		public Task<TOut> EnsureInTransactionAsync<TOut>(Func<LazyDbConnection, IDbTransaction, Task<TOut>> func,
			IsolationLevel isolationLevel)
		{
			if (func == null)
				throw Errors.InvalidInput("null_method");

			if (LazyDbConnection == null)
				throw Errors.InvalidOperation("null_lazy_db_connection");

			return LazyDbConnection.EnsureInTransactionAsync(func, isolationLevel);
		}

		protected RunnableSql NewSql(Func<ISqlDialect, ISqlDialect> sql, object? param = null) =>
			new(LazyDbConnection, PrepareSql(sql), param ?? this);

		protected RunnableSql NewSql(string sqlText, object? param = null) =>
			new(LazyDbConnection, sqlText, param ?? this);

		private string PrepareSql(Func<ISqlDialect, ISqlDialect> sql)
		{
			if (sql == null) throw Errors.InvalidInput("null_sql");
			if (SqlFactory == null) throw Errors.InvalidInput("null_sql_factory");

			return sql(SqlFactory.Create()).SqlText;
		}
	}
}
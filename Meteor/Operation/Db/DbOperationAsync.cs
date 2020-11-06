using System;
using System.Data;
using System.Threading.Tasks;
using Meteor.Database;
using Meteor.Database.SqlDialect;
using Meteor.Utils;

namespace Meteor.Operation.Db
{
    public abstract class DbOperationAsync<T> : OperationAsync<T>
    {
        public LazyDbConnection LazyDbConnection { get; private set; }
        protected ISqlDialect? SqlDialect { get; private set; }

        public DbOperationAsync(LazyDbConnection lazyDbConnection, ISqlDialect? sqlDialect)
        {
            LazyDbConnection = lazyDbConnection;
            SqlDialect = sqlDialect;
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

        protected RunnableSql NewSql(Action<ISqlDialect> sql, object? param = null) =>
            new RunnableSql(LazyDbConnection, PrepareSql(sql), param ?? this);
        
        protected RunnableSql NewSql(string sqlText, object? param = null) =>
            new RunnableSql(LazyDbConnection, sqlText, param ?? this);

        private string PrepareSql(Action<ISqlDialect> sql)
        {
            if (sql == null) throw Errors.InvalidInput("null_sql");
            if (SqlDialect == null) throw Errors.InvalidInput("null_sql_dialect");
            
            SqlDialect.Clear();
            sql(SqlDialect);
            
            return SqlDialect.SqlText;
        }
    }
}
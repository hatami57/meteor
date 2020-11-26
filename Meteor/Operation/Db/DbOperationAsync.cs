using System;
using System.Data;
using System.Threading.Tasks;
using Meteor.Database;
using Meteor.Database.SqlDialect;
using Meteor.Utils;

namespace Meteor.Operation.Db
{
    public abstract class DbOperationAsync : DbOperationAsync<NoType, NoType>
    {
    }

    public abstract class DbInOperationAsync<TInput> : DbOperationAsync<TInput, NoType>
    {
    }

    public abstract class DbOutOperationAsync<TOutput> : DbOperationAsync<NoType, TOutput>
    {
    }

    public abstract class DbOperationAsync<TInput, TOutput> : OperationAsync<TInput, TOutput>
    {
        public LazyDbConnection? LazyDbConnection { get; set; }
        public ISqlFactory? SqlFactory { get; set; }

        public override IOperationAsync SetOperationFactory(OperationFactory operationFactory)
        {
            base.SetOperationFactory(operationFactory);
            
            LazyDbConnection = OperationFactory?.GetService<LazyDbConnection>();
            SqlFactory = OperationFactory?.GetService<ISqlFactory>();

            return this;
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
        protected Task<TOut> EnsureInTransactionAsync<TOut>(Func<Task<TOut>> func,
            IsolationLevel isolationLevel)
        {
            if (func == null)
                throw Errors.InvalidInput("func==null");

            if (LazyDbConnection == null)
                throw Errors.InvalidOperation("LazyDbConnection==null");

            return LazyDbConnection.EnsureInTransactionAsync(func, isolationLevel);
        }

        protected RunnableSql NewSql(Func<ISqlDialect, ISqlDialect> sql, object? param = null) =>
            NewSql(PrepareSql(sql), param ?? Input);

        protected RunnableSql NewSql(string sqlText, object? param = null)
        {
            if (LazyDbConnection == null)
                throw Errors.InvalidOperation("LazyDbConnection==null");
            
            return new RunnableSql(LazyDbConnection, sqlText, param ?? Input);
        }

        private string PrepareSql(Func<ISqlDialect, ISqlDialect> sql)
        {
            if (sql == null) throw Errors.InvalidInput("null_sql");
            if (SqlFactory == null) throw Errors.InvalidInput("null_sql_factory");

            return sql(SqlFactory.Create()).SqlText;
        }
    }
}
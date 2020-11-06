using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Meteor.Database.SqlDialect;
using Meteor.Utils;

namespace Meteor.Database
{
    public class RunnableSql
    {
        private readonly LazyDbConnection _lazyDbConnection;
        private readonly string _sqlText;
        private readonly object? _param;

        public RunnableSql(LazyDbConnection lazyDbConnection, string sqlText, object? param)
        {
            _lazyDbConnection = lazyDbConnection;
            _sqlText = sqlText;
            _param = param;
        }

        public Task<SqlMapper.GridReader> QueryMultipleAsync() =>
            _lazyDbConnection.QueryMultipleAsync(_sqlText, _param);

        public Task<IEnumerable<T>> QueryAsync<T>() =>
            _lazyDbConnection.QueryAsync<T>(_sqlText, _param);

        public Task<IEnumerable<TReturn>> QueryAsync<TModel1, TModel2, TReturn>(
            Func<TModel1, TModel2, TReturn> func, string splitOn = "Id") =>
            _lazyDbConnection.QueryAsync(_sqlText, func, _param, splitOn);

        public Task<IEnumerable<TReturn>> QueryAsync<TModel1, TModel2, TModel3, TReturn>(
            Func<TModel1, TModel2, TModel3, TReturn> func, string splitOn = "Id") =>
            _lazyDbConnection.QueryAsync(_sqlText, func, _param, splitOn);

        public Task<T> QueryFirstOrDefaultAsync<T>() =>
            _lazyDbConnection.QueryFirstOrDefaultAsync<T>(_sqlText, _param);

        public Task<int> ExecuteAsync() =>
            _lazyDbConnection.ExecuteAsync(_sqlText, _param);

        public Task<T> ExecuteScalarAsync<T>() =>
            _lazyDbConnection.ExecuteScalarAsync<T>(_sqlText, _param);
    }
}
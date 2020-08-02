using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace Meteor.Database.Sql
{
    public class SqlGenerator
    {
        private readonly LazyDbConnection _lazyDbConnection;
        private readonly IDbTransaction _transaction;
        public object ParamObject { get; set; }
        public string Sql { get; set; }

        public SqlGenerator(LazyDbConnection lazyDbConnection, string sql, object paramObject,
            IDbTransaction transaction = null)
        {
            _lazyDbConnection = lazyDbConnection;
            Sql = sql;
            SetParamObject(paramObject);
            _transaction = transaction;
        }

        public SqlGenerator(LazyDbConnection lazyDbConnection, object paramObject)
            : this(lazyDbConnection, "", paramObject)
        {
        }

        public SqlGenerator SetParamObject(object paramObject)
        {
            ParamObject = paramObject;
            return this;
        }

        public SqlGenerator NewSql()
        {
            Sql = "";
            return this;
        }

        public SqlGenerator Where(string where)
        {
            Sql += " " + new SqlWhereBuilder(where).GetSql();
            return this;
        }

        public SqlGenerator Where(Action<SqlWhereBuilder> whereBuilder)
        {
            var builder = new SqlWhereBuilder();
            whereBuilder(builder);
            Sql += " " + builder.GetSql();
            return this;
        }

        public SqlGenerator With(string name, Action<SqlGenerator> sqlBuilder)
        {
            var sql = new SqlGenerator(null, null);
            sqlBuilder(sql);
            Sql += $"WITH {name} AS ( {sql.Sql} ) ";
            return this;
        }

        public SqlGenerator Select(string tableName, string fieldNames = "*")
        {
            Sql += $"SELECT {fieldNames} FROM {tableName}";
            return this;
        }

        public SqlGenerator ForUpdate() =>
            Append("FOR UPDATE");
        
        public SqlGenerator ForShare() =>
            Append("FOR SHARE");

        public SqlGenerator InnerJoin(string tableName, string onClause) =>
            Join(tableName, onClause);

        public SqlGenerator LeftJoin(string tableName, string onClause) =>
            Join(tableName, onClause, "LEFT");

        public SqlGenerator RightJoin(string tableName, string onClause) =>
            Join(tableName, onClause, "RIGHT");

        public SqlGenerator Join(string tableName, string onClause, string type = "INNER")
        {
            Sql += $" {type} JOIN {tableName} ON {onClause}";
            return this;
        }

        public SqlGenerator OrderBy(string orderBy)
        {
            Sql += " ORDER BY " + orderBy;
            return this;
        }

        public SqlGenerator GroupBy(string groupBy)
        {
            Sql += " GROUP BY " + groupBy;
            return this;
        }
        
        public SqlGenerator Having(string having)
        {
            Sql += " HAVING " + having;
            return this;
        }

        public SqlGenerator Limit(string limit)
        {
            Sql += " LIMIT " + limit;
            return this;
        }

        public SqlGenerator Offset(string offset)
        {
            Sql += " OFFSET " + offset;
            return this;
        }

        public SqlGenerator Insert(string tableName, string fieldNames, string values)
        {
            Sql += $"INSERT INTO {tableName} ({fieldNames}) VALUES ({values})";
            return this;
        }

        public SqlGenerator Update(string tableName, string setFields)
        {
            Sql += new SqlUpdateBuilder(tableName, setFields).GetSql();
            return this;
        }

        public SqlGenerator Update(string tableName, Action<SqlUpdateBuilder> updateBuilder)
        {
            var builder = new SqlUpdateBuilder(tableName);
            updateBuilder(builder);
            Sql += builder.GetSql();
            return this;
        }

        public SqlGenerator Delete(string tableName)
        {
            Sql += $"DELETE FROM {tableName}";
            return this;
        }

        public SqlGenerator EndStatement()
        {
            Sql += ";";
            return this;
        }

        public SqlGenerator Append(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
                return this;

            Sql += " " + sql.Trim();
            return this;
        }
        
        public async Task<SqlMapper.GridReader> QueryMultipleAsync(IDbTransaction transaction = null) =>
            await (await _lazyDbConnection.GetConnectionAsync().ConfigureAwait(false))
                .QueryMultipleAsync(Sql, ParamObject, transaction ?? _transaction).ConfigureAwait(false);

        public async Task<IEnumerable<T>> QueryAsync<T>(IDbTransaction transaction = null) =>
            await (await _lazyDbConnection.GetConnectionAsync().ConfigureAwait(false))
                .QueryAsync<T>(Sql, ParamObject, transaction ?? _transaction).ConfigureAwait(false);

        public async Task<IEnumerable<TReturn>> QueryAsync<TModel1, TModel2, TReturn>(
            Func<TModel1, TModel2, TReturn> func, IDbTransaction transaction = null, string splitOn = "Id") =>
            await (await _lazyDbConnection.GetConnectionAsync().ConfigureAwait(false))
                .QueryAsync(Sql, func, ParamObject, transaction ?? _transaction, splitOn: splitOn).ConfigureAwait(false);
        
        public async Task<IEnumerable<TReturn>> QueryAsync<TModel1, TModel2, TModel3, TReturn>(
            Func<TModel1, TModel2, TModel3, TReturn> func, IDbTransaction transaction = null, string splitOn = "Id") =>
            await (await _lazyDbConnection.GetConnectionAsync().ConfigureAwait(false))
                .QueryAsync(Sql, func, ParamObject, transaction ?? _transaction, splitOn: splitOn).ConfigureAwait(false);

        public async Task<T> QueryFirstOrDefaultAsync<T>(IDbTransaction transaction = null) =>
            await (await _lazyDbConnection.GetConnectionAsync().ConfigureAwait(false))
                .QueryFirstOrDefaultAsync<T>(Sql, ParamObject, transaction ?? _transaction).ConfigureAwait(false);

        public async Task<int> ExecuteAsync(IDbTransaction transaction = null) =>
            await (await _lazyDbConnection.GetConnectionAsync().ConfigureAwait(false))
                .ExecuteAsync(Sql, ParamObject, transaction ?? _transaction).ConfigureAwait(false);

        public async Task<T> ExecuteScalarAsync<T>(IDbTransaction transaction = null) =>
            await (await _lazyDbConnection.GetConnectionAsync().ConfigureAwait(false))
                .ExecuteScalarAsync<T>(Sql, ParamObject, transaction ?? _transaction).ConfigureAwait(false);
    }
}
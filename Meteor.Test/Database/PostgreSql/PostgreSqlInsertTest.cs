using Meteor.Database.SqlDialect;
using Meteor.Database.SqlDialect.PostgreSql;
using Xunit;

namespace Meteor.Test.Database.PostgreSql
{
    public class PostgreSqlInsertTest
    {
        [Fact]
        public void NormalInsertReturnId()
        {
            var sql = new PostgreSqlDialect();
            sql.InsertReturnId("table1", "c1, c2, c3", "1, 2, 3", "key");
            
            Assert.Equal("INSERT INTO table1 (c1, c2, c3) VALUES (1, 2, 3) RETURNING key;", sql.SqlText);
        }
    }
}
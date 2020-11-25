using Meteor.Database.SqlDialect.Sqlite;
using Xunit;

namespace Meteor.Test.Database.Sqlite
{
    public class SqliteInsertTest
    {
        [Fact]
        public void NormalInsertReturnId()
        {
            var sql = new SqliteDialect();
            sql.InsertReturnId("table1", "c1, c2, c3", "1, 2, 3");
            
            Assert.Equal("INSERT INTO table1 (c1, c2, c3) VALUES (1, 2, 3); SELECT last_insert_rowid();", sql.SqlText);
        }
    }
}
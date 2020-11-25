using Meteor.Database.SqlDialect.MsSql;
using Xunit;

namespace Meteor.Test.Database.MsSql
{
    public class MsSqlInsertTest
    {
        [Fact]
        public void SelectSpecificColumns()
        {
            var sql = new MsSqlDialect();
            sql.InsertReturnId("table1", "c1, c2, c3", "1, 2, 3", "Key");
            
            Assert.Equal("INSERT INTO table1 (c1, c2, c3) OUTPUT INSERTED.Key VALUES (1, 2, 3)", sql.SqlText);
        }
    }
}
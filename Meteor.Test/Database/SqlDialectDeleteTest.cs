using Meteor.Database.SqlDialect;
using Meteor.Operation.Db;
using Xunit;

namespace Meteor.Test.Database
{
    public class SqlDialectDeleteTest
    {
        [Fact]
        public void NormalDelete()
        {
            var sql = new SqlDialect();
            sql.Delete("table1");
            
            Assert.Equal("DELETE FROM table1", sql.SqlText);
        }
        
        [Fact]
        public void DeleteThisId()
        {
            var sql = new SqlDialect();
            sql.DeleteThisId("table1");
            Assert.Equal("DELETE FROM table1 WHERE id=@Id", sql.SqlText);
        }
    }
}
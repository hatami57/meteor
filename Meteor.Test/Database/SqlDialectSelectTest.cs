using Meteor.Database.SqlDialect;
using Xunit;

namespace Meteor.Test.Database
{
    public class SqlDialectSelectTest
    {
        [Fact]
        public void SelectSpecificColumnsTest()
        {
            var sql = new SqlDialect();
            sql.Select("table1", "c1, c2, c3");
            
            Assert.Equal("SELECT c1, c2, c3 FROM table1", sql.SqlText);
        }
        
        [Fact]
        public void SelectAllColumnsTest()
        {
            var sql = new SqlDialect();
            sql.Select("table1");
            
            Assert.Equal("SELECT * FROM table1", sql.SqlText);
        }
    }
}
using Meteor.Database.SqlDialect;
using Meteor.Operation.Db;
using Xunit;

namespace Meteor.Test.Database
{
    public class SqlDialectUpdateTest
    {
        [Fact]
        public void NormalUpdate()
        {
            var sql = new SqlDialect();
            sql.Update("table1", "c1=1, c2='c2', c3=9")
                .Where("c1=5 AND c2='c23'");
            
            Assert.Equal("UPDATE table1 SET c1=1, c2='c2', c3=9 WHERE c1=5 AND c2='c23'", sql.SqlText);
        }
        
        [Fact]
        public void UpdateBuilder()
        {
            var sql = new SqlDialect();
            sql.Update("table1", x => x.Set("c1=5")
                    .Set("c2='c2'")
                    .When(false, "c3=7")
                    .When(true, "c4=9"));
            
            Assert.Equal("UPDATE table1 SET c1=5, c2='c2', c4=9", sql.SqlText);
        }
        
        [Fact]
        public void UpdateThisId()
        {
            var sql = new SqlDialect();
            sql.UpdateThisId("table1", "c1=7");
            
            Assert.Equal("UPDATE table1 SET c1=7 WHERE id=@Id", sql.SqlText);
        }
    }
}
using Meteor.Database.SqlDialect;
using Meteor.Operation.Db;
using Xunit;

namespace Meteor.Test.Database
{
    public class SqlDialectWhereTest
    {
        [Fact]
        public void NormalWhereTest()
        {
            var sql = new SqlDialect();
            sql.Select("table1", "c1, c2, c3")
                .Where("c1=5 AND c2='c2'");
            
            Assert.Equal("SELECT c1, c2, c3 FROM table1 WHERE c1=5 AND c2='c2'", sql.SqlText);
        }
        
        [Fact]
        public void WhereBuilderTest()
        {
            var sql = new SqlDialect();
            sql.Select("table1")
                .Where(x => x.Where("c1=5")
                    .And("c2='c2'")
                    .When(false, "c3=7")
                    .When(true, "c4=9"));
            
            Assert.Equal("SELECT * FROM table1 WHERE c1=5 AND c2='c2' AND c4=9", sql.SqlText);
        }
        
        [Fact]
        public void WhereThisIdTest()
        {
            var sql = new SqlDialect();
            sql.Select("table1")
                .WhereThisId();
            
            Assert.Equal("SELECT * FROM table1 WHERE id=@Id", sql.SqlText);
        }
    }
}
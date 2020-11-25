using Meteor.Database.SqlDialect;
using Xunit;

namespace Meteor.Test.Database
{
    public class SqlDialectInsertTest
    {
        [Fact]
        public void NormalInsert()
        {
            var sql = new SqlDialect();
            sql.Insert("table1", "c1, c2, c3", "1, 'c2', 5");
            
            Assert.Equal("INSERT INTO table1 (c1, c2, c3) VALUES (1, 'c2', 5)", sql.SqlText);
        }
        
        [Fact]
        public void InsertWithoutColumnNames()
        {
            var sql = new SqlDialect();
            sql.Insert("table1", "", "1, 'c2', 5");
            Assert.Equal("INSERT INTO table1 VALUES (1, 'c2', 5)", sql.SqlText);

            sql.Clear();
            
            sql.Insert("table1", null, "DEFAULT");
            Assert.Equal("INSERT INTO table1 VALUES (DEFAULT)", sql.SqlText);
        }
        
        [Fact]
        public void InsertWithCustomValues()
        {
            var sql = new SqlDialect();
            sql.InsertCustomValues("table1", "c1, c2", "SELECT c1, c2 FROM table2");
            Assert.Equal("INSERT INTO table1 (c1, c2) SELECT c1, c2 FROM table2", sql.SqlText);

            sql.Clear();
            
            sql.InsertCustomValues("table1", null, "SELECT * FROM table2");
            Assert.Equal("INSERT INTO table1 SELECT * FROM table2", sql.SqlText);
        }
    }
}
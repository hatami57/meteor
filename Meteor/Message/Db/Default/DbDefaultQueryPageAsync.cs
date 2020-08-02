using System.Threading.Tasks;

namespace Meteor.Message.Db.Default
{
    public class DbDefaultQueryPageAsync<T> : DbQueryPageAsync<T>
    {
        protected string TableName { get; set; }
        
        public DbDefaultQueryPageAsync(string tableName)
        {
            TableName = tableName;
        }
        protected override Task<QueryPage<T>> ExecuteMessageAsync() => 
            NewSql().SelectQueryPageAsync(TableName, this);
    }
}
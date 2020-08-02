using System.Threading.Tasks;

namespace Meteor.Message.Db.Default
{
    public class DbDefaultSelectAsync<T> : DbMessageAsync<T>
    {
        protected string TableName { get; set; }

        public DbDefaultSelectAsync(string tableName)
        {
            TableName = tableName;
        }

        protected override Task<T> ExecuteMessageAsync() =>
            NewSql().SelectThisIdAsync<T>(TableName);
    }
}
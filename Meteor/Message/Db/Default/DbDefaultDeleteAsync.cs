using System.Threading.Tasks;

namespace Meteor.Message.Db.Default
{
    public class DbDefaultDeleteAsync : DbMessageAsync<bool>
    {
        protected string TableName { get; set; }

        public DbDefaultDeleteAsync(string tableName)
        {
            TableName = tableName;
        }

        protected override async Task<bool> ExecuteMessageAsync() =>
            await NewSql().DeleteThisIdAsync(TableName)
                .ConfigureAwait(false) > 0;
    }
}
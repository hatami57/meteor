using System.Threading.Tasks;

namespace Meteor.Message.Db.Default
{
    public class DbDefaultUpdateAsync : DbMessageAsync<bool>
    {
        protected string TableName { get; set; }
        protected string SetFields { get; set; }

        public DbDefaultUpdateAsync(string tableName, string setFields)
        {
            TableName = tableName;
            SetFields = setFields;
        }

        protected override async Task<bool> ExecuteMessageAsync() =>
            await NewSql().UpdateThisIdAsync(TableName, SetFields)
                .ConfigureAwait(false) > 0;
    }
}
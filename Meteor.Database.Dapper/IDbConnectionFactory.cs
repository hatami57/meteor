using System.Data.Common;
using System.Threading.Tasks;

namespace Meteor.Database.Dapper
{
    public interface IDbConnectionFactory
    {
        Task<DbConnection> OpenNewConnectionAsync();
    }
}
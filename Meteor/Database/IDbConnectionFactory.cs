using System.Data.Common;
using System.Threading.Tasks;

namespace Meteor.Database
{
    public interface IDbConnectionFactory
    {
        Task<DbConnection> OpenNewConnectionAsync();
        LazyDbConnection OpenNewLazyConnection();
    }
}
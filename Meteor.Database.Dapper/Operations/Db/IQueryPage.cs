using System.Collections.Generic;

namespace Meteor.Database.Dapper.Operations.Db
{
    public interface IQueryPage<T>
    {
        int PageNo { get; set; }
        int PageSize { get; set; }
        long TotalCount { get; set; }
        int TotalPages { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
        List<T> Items { get; }
    }
}
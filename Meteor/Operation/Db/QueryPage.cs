using System;
using System.Collections.Generic;

namespace Meteor.Operation.Db
{
    public class QueryPage<T> : IQueryPage<T>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public long TotalCount { get; set; }
        public int TotalPages => (int) Math.Ceiling(TotalCount / (double) PageSize);
        public bool HasPreviousPage => PageNo > 1;
        public bool HasNextPage => PageNo < TotalPages;
        public List<T> Items { get; }

        public QueryPage(List<T> items, int pageNo, int pageSize, long totalCount)
        {
            PageNo = pageNo;
            PageSize = pageSize;
            TotalCount = totalCount;
            Items = items;
        }
        
        public QueryPage(IEnumerable<T> items, int pageNo, int pageSize, long totalCount)
        {
            PageNo = pageNo;
            PageSize = pageSize;
            TotalCount = totalCount;
            Items = new List<T>(items);
        }
    }
}
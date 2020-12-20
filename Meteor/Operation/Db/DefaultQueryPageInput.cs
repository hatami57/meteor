using System;
using System.Collections.Generic;
using System.Text;

namespace Meteor.Operation.Db
{
    public record DefaultQueryPageInput : IQueryPageInput
    {
        public int Page { get; set; }
        public int Take { get; set; }
        public int Offset => (Page - 1) * Take;
    }
}

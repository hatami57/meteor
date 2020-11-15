using System;
using System.Collections.Generic;
using System.Text;

namespace Meteor.Operation.Db
{
    public class DefaultQueryPageInput : IQueryPageInput
    {
        public int Page { get; set; }
        public int Take { get; set; }
    }
}

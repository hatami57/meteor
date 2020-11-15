using System;
using System.Collections.Generic;
using System.Text;

namespace Meteor.Operation.Db.Default
{
    public interface IHaveId<T>
    {
        public T Id { get; set; }
    }

    public interface IHaveId : IHaveId<int>
    {
    }
}

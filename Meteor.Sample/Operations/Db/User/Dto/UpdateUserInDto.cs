using Meteor.Operation.Db.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meteor.Sample.Operations.Db.User.Dto
{
    public class UpdateUserInDto : AddUserInDto, IHaveId
    {
        public int Id { get; set; }
    }
}

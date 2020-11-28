using System;
using System.Threading.Tasks;
using Meteor.Database;
using Meteor.Database.SqlDialect;
using Meteor.Operation.Db;

namespace test.Messages.Test
{
    public class AddTest : DbOperationAsync<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public AddTest(LazyDbConnection lazyDbConnection, ISqlFactory sqlFactory) : base(lazyDbConnection, sqlFactory)
        {
            Console.WriteLine("");
        }

        protected override Task ExecutionAsync()
        {
            Console.WriteLine("");
            return Task.CompletedTask;
        }
    }
}
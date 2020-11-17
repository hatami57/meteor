using System;
using System.Text.Json;

namespace Meteor
{
    public class Error : Exception
    {
        public int Code { get; set; }
        public object? Details { get; set; }

        public Error(int code, string message, object? details = null, Exception? innerException = null)
            : base(message, innerException)
        {
            Code = code;
            Details = details;
        }

        public Error()
        {
        }

        public Error(string message) : base(message)
        {
        }

        public Error(string message, Exception innerException) : base(message, innerException)
        {
        }

        public override string ToString()
        {
            return $"Meteor.Error: [{Code}] {Message} ({JsonSerializer.Serialize(Details)})";
        }
    }
}
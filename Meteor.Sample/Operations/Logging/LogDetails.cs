namespace Meteor.Sample.Operations.Logging
{
    public record LogDetails
    {
        public int UserId { get; set; }
        public bool Result { get; set; }
        public object? Input { get; set; }
        public object? Output { get; set; }
    }
}
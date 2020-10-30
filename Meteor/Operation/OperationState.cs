namespace Meteor.Operation
{
    public enum OperationState
    {
        Created,
        PreparedProperties,
        ValidatedProperties,
        PreparedExecution,
        ValidatedBeforeExecution,
        Executed,
        Succeed,
        Failed,
    }
}
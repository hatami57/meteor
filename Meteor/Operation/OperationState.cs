namespace Meteor.Operation
{
    public enum OperationState
    {
        Created,
        ValidatedInput,
        PreparedInput,
        PreparedExecution,
        ValidatedBeforeExecution,
        Executed,
        Succeed,
        Failed,
    }
}
namespace Meteor.Operation
{
    public abstract class OperationAsync : SharedOperationAsync<object?>
    {
    }
    
    public abstract class OperationAsync<TResult> : SharedOperationAsync<TResult, object?>
    {
    }
}
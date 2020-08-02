using System;
using System.Threading.Tasks;
using Meteor.Utils;
using Serilog;

namespace Meteor.Message
{
    public abstract class MessageAsync<T>
    {
        public virtual Task<MessageAsync<T>> PreparePropertiesAsync() =>
            Task.FromResult(this);
        public virtual Task ValidatePropertiesAsync() =>
            Task.CompletedTask;
        public virtual Task PrepareExecutionAsync() =>
            Task.CompletedTask;
        public virtual Task ValidateBeforeExecutionAsync() =>
            Task.CompletedTask;
        public virtual Task ValidateAfterExecutionAsync(T result) =>
            Task.CompletedTask;
        protected virtual Task FinalizeAsync() =>
            Task.CompletedTask;

        protected abstract Task<T> ExecuteMessageAsync();

        public async Task<T> ExecuteAsync()
        {
            MessageAsync<T> msg = null;
            var messageName = GetType().FullName;
            
            try
            {
                Log.Debug("start executing {MessageName} message, with {@Properties}", messageName, this);

                Log.Verbose("calling {MethodName}", nameof(PreparePropertiesAsync));
                msg = await PreparePropertiesAsync().ConfigureAwait(false);
                Log.Verbose("calling {MethodName}", nameof(ValidatePropertiesAsync));
                await msg.ValidatePropertiesAsync().ConfigureAwait(false);
                Log.Verbose("calling {MethodName}", nameof(PrepareExecutionAsync));
                await msg.PrepareExecutionAsync().ConfigureAwait(false);
                Log.Verbose("calling {MethodName}", nameof(ValidateBeforeExecutionAsync));
                await msg.ValidateBeforeExecutionAsync().ConfigureAwait(false);
                Log.Verbose("calling {MethodName}", nameof(ExecuteMessageAsync));
                var res = await msg.ExecuteMessageAsync().ConfigureAwait(false);
                Log.Verbose("calling {MethodName}", nameof(ValidateAfterExecutionAsync));
                await msg.ValidateAfterExecutionAsync(res).ConfigureAwait(false);

                Log.Debug("message executed successfully");
                return res;
            }
            catch (Exception e)
            {
                Log.Error(e, "message execution failed");
                throw;
            }
            finally
            {
                if (msg != null)
                {
                    Log.Verbose("calling {MethodName}", nameof(FinalizeAsync));
                    await msg.FinalizeAsync().ConfigureAwait(false);
                }
                
                Log.Debug("finish executing {MessageName} message", messageName);
            }
        }

        public Task<OperationResult<T>> TryExecuteAsync() =>
            OperationResult<T>.Try(ExecuteAsync);
    }
}
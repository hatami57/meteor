using System;
using System.Threading.Tasks;
using Meteor.Utils;
using Serilog;

namespace Meteor.Operation
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public Error? Error { get; set; }

        public OperationResult()
        {
        }

        public OperationResult(bool success, Error? error = null)
        {
            Success = success;
            Error = error;
        }
    }

    public class OperationResult<TResult> : OperationResult
    {
        public TResult? Result { get; set; }

        public OperationResult() : this(false)
        {
        }

        public OperationResult(bool success, TResult? result = default, Error? error = null)
            : base(success, error)
        {
            Result = result;
        }
    }

    public static class OperationResultFactory
    {
        public static async Task<OperationResult> Try(Func<Task> func)
        {
            if (func == null) throw Errors.InvalidInput("null_func");
            
            try
            {
                await func().ConfigureAwait(false);
                return new OperationResult(true);
            }
            catch (Exception e)
            {
                Log.Error(e, "OperationResult");
                return new OperationResult(false, e as Error ?? Errors.InternalError(null, e));
            }
        }

        public static async Task<OperationResult> Try(Func<Task<OperationResult>> func)
        {
            if (func == null) throw Errors.InvalidInput("null_func");
            
            try
            {
                return await func().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Log.Error(e, "OperationResult");
                return new OperationResult(false,
                    e as Error ?? Errors.InternalError(null, e));
            }
        }
        
        public static async Task<OperationResult<TResult>> Try<TResult>(Func<Task<TResult>> func)
        {
            if (func == null) throw Errors.InvalidInput("null_func");
            try
            {
                var value = await func().ConfigureAwait(false);
                return new OperationResult<TResult>(true, value);
            }
            catch (Exception e)
            {
                Log.Error(e, "OperationResult<{T}>", typeof(TResult).FullName);
                return new OperationResult<TResult>(false, default,
                    e as Error ?? Errors.InternalError(null, e));
            }
        }
        
        public static async Task<OperationResult<TResult>> Try<TResult>(Func<Task<OperationResult<TResult>>> func)
        {
            if (func == null) throw Errors.InvalidInput("null_func");
            try
            {
                return await func().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Log.Error(e, "OperationResult<{T}>", typeof(TResult).FullName);
                return new OperationResult<TResult>(false, default,
                    e as Error ?? Errors.InternalError(null, e));
            }
        }
    }
}
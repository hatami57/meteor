using System;
using System.Threading.Tasks;
using Serilog;

namespace Meteor.Utils
{
    public class OperationResult
    {
        public bool Success { get; set;  }
        public Error Error { get; set; }

        public OperationResult() : this(false)
        {
        }

        public OperationResult(bool success, Error error = null)
        {
            Success = success;
            Error = error;
        }

        public async Task<OperationResult> Then(Func<OperationResult, Task> func) =>
            (Error == null) ? await Try(() => func(this)).ConfigureAwait(false) : this;
        public async Task<OperationResult> Catch(Func<OperationResult, Task> func) =>
            (Error != null) ? await Try(() => func(this)).ConfigureAwait(false) : this;

        public static async Task<OperationResult> Try(Func<Task> func)
        {
            try
            {
                await func().ConfigureAwait(false);
                return new OperationResult(true);
            }
            catch (Exception e)
            {
                Log.Error(e, "OperationResult");
                return new OperationResult(false,
                    e as Error ?? Errors.InternalError(null, e));
            }
        }
        
        public static async Task<OperationResult> Try(Func<Task<OperationResult>> func)
        {
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
    }

    public class OperationResult<T> : OperationResult
    {
        public T Result { get; set; }

        public OperationResult() : this(false, default)
        {
        }

        public OperationResult(bool success, T result, Error error = null)
            : base(success, error)
        {
            Result = result;
        }
        
        public async Task<OperationResult<T>> Then(Func<OperationResult<T>, Task<T>> func) =>
            (Error == null) ? await Try(() => func(this)).ConfigureAwait(false) : this;
        public async Task<OperationResult<T>> Catch(Func<OperationResult<T>, Task<T>> func) =>
            (Error != null) ? await Try(() => func(this)).ConfigureAwait(false) : this;

        public static async Task<OperationResult<T>> Try(Func<Task<T>> func)
        {
            try
            {
                return new OperationResult<T>(true, await func().ConfigureAwait(false), null);
            }
            catch (Exception e)
            {
                Log.Error(e, "OperationResult<{T}>", typeof(T).FullName);
                return new OperationResult<T>(false, default,
                    e as Error ?? Errors.InternalError(null, e));
            }
        }
        
        public static async Task<OperationResult<T>> Try(Func<Task<OperationResult<T>>> func)
        {
            try
            {
                return await func().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Log.Error(e, "OperationResult<{T}>", typeof(T).FullName);
                return new OperationResult<T>(false, default,
                    e as Error ?? Errors.InternalError(null, e));
            }
        }
    }
    
    public class Error : Exception
    {
        public int Code { get; set; }
        public object Details { get; set; }

        public Error(int code, string message, object details = null, Exception innerException = null)
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
    }
}

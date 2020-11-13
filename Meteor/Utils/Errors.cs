using System;
using System.Threading.Tasks;

namespace Meteor.Utils
{
    public static class Errors
    {
#pragma warning disable CA1303 // Do not pass literals as localized parameters
        public static Error InternalError(object? details = null, Exception? innerException = null) =>
            new Error(1, "internal_error", details, innerException);

        public static Error NotFound(object? details = null) =>
            new Error(2, "not_found", details);

        public static Error AlreadyDone(object? details = null) =>
            new Error(3, "already_done", details);

        public static Error InvalidOperation(object? details = null) =>
            new Error(4, "invalid_operation", details);

        public static Error DatabaseError(object? details = null) =>
            new Error(5, "database_error", details);

        public static Error DuplicateKey(object? details = null) =>
            new Error(6, "duplicate_key", details);

        public static Error AccessDenied(object? details = null) =>
            new Error(7, "access_denied", details);

        public static Error InvalidInput(object? details = null) =>
            new Error(8, "invalid_input", details);
#pragma warning restore CA1303 // Do not pass literals as localized parameters

        /// <summary>
        /// Runs an operation and ignores any exceptions that occur.
        /// Returns true or false depending on whether catch was triggered
        /// </summary>
        /// <param name="operation">lambda that performs an operation that might throw</param>
        /// <returns></returns>
        public static bool Ignore(Action? operation)
        {
            if (operation == null)
                return false;

            try
            {
                operation.Invoke();
                return true;
            }
            catch
            {
                // ignored
                return false;
            }
        }

        /// <summary>
        /// Runs a function that returns a value and ignores any exceptions that occur.
        /// Returns T or default(T) depending on whether catch was triggered
        /// </summary>
        /// <param name="operation">parameterless lambda that returns a value of T</param>
        /// <param name="defaultValue">Default value returned if the operation fails</param>
        public static T Ignore<T>(Func<T>? operation, T defaultValue = default)
        {
            if (operation == null)
                return defaultValue;

            try
            {
                return operation.Invoke();
            }
            catch
            {
                // ignored
                return defaultValue;
            }
        }

        /// <inheritdoc cref="Ignore"/>
        public static async Task<bool> IgnoreAsync(Func<Task>? operation)
        {
            if (operation == null)
                return false;

            try
            {
                await operation().ConfigureAwait(false);
                return true;
            }
            catch
            {
                // ignored
                return false;
            }
        }
        
        /// <inheritdoc cref="Ignore"/>
        public static async Task<bool> IgnoreAsync<T>(Func<T, Task>? operation, T param)
        {
            if (operation == null)
                return false;

            try
            {
                await operation(param).ConfigureAwait(false);
                return true;
            }
            catch
            {
                // ignored
                return false;
            }
        }

        public static async Task<T> Ignore<T>(Task<T>? operation, T defaultValue = default)
        {
            if (operation == null)
                return defaultValue;

            try
            {
                return await operation.ConfigureAwait(false);
            }
            catch
            {
                // ignored
                return defaultValue;
            }
        }
    }
}
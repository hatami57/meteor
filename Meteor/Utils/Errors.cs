﻿using System;

 namespace Meteor.Utils
{
    public static class Errors
    {
#pragma warning disable CA1303 // Do not pass literals as localized parameters
        public static Error InternalError(object details = null, Exception innerException = null) =>
            new Error(1, "internal_error", details, innerException);
        public static Error NotFound(object details = null) =>
            new Error(2, "not_found", details);
        public static Error AlreadyDone(object details = null) =>
            new Error(3, "already_done", details);
        public static Error InvalidOperation(object details = null) =>
            new Error(4, "invalid_operation", details);
        public static Error DatabaseError(object details = null) =>
            new Error(5, "database_error", details);
        public static Error DuplicateKey(object details = null) =>
            new Error(6, "duplicate_key", details);
        public static Error AccessDenied(object details = null) =>
            new Error(7, "access_denied", details);
        public static Error InvalidInput(object details = null) =>
            new Error(8, "invalid_input", details);
#pragma warning restore CA1303 // Do not pass literals as localized parameters
    }
}

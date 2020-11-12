﻿using System;
using System.Threading.Tasks;
using Meteor.Operation;
using Meteor.Utils;

namespace Meteor.Test.Assets
{
    class SimpleOperation : OperationAsync
    {
        public bool ShouldThrowInPrepareProperties { get; set; }
        public bool ShouldThrowInValidateProperties { get; set; }
        public bool ShouldThrowInValidateBeforeExecution { get; set; }
        public bool ShouldThrowInPrepareExecution { get; set; }
        public bool ShouldThrowInExecution { get; set; }
        public bool ShouldThrowInValidateAfterExecution { get; set; }
        public bool ShouldThrowInFinalize { get; set; }
        public bool ShouldThrowInOnSuccess { get; set; }
        public bool ShouldThrowInOnError { get; set; }

        public bool OnSuccessIsCalled { get; private set; }
        public bool OnErrorIsCalled { get; private set; }
        public bool FinalizeIsCalled { get; private set; }
        public string? ThrowAtMethod { get; private set; }

        protected override Task<OperationAsync> PreparePropertiesAsync()
        {
            if (ShouldThrowInPrepareProperties)
            {
                ThrowAtMethod = nameof(PreparePropertiesAsync);
                throw Errors.InvalidOperation();
            }

            return base.PreparePropertiesAsync();
        }

        protected override Task ValidatePropertiesAsync()
        {
            if (ShouldThrowInValidateProperties)
            {
                ThrowAtMethod = nameof(ValidatePropertiesAsync);
                throw Errors.InvalidOperation();
            }

            return base.ValidatePropertiesAsync();
        }

        protected override Task ValidateBeforeExecutionAsync()
        {
            if (ShouldThrowInValidateBeforeExecution)
            {
                ThrowAtMethod = nameof(ValidateBeforeExecutionAsync);
                throw Errors.InvalidOperation();
            }

            return base.ValidateBeforeExecutionAsync();
        }

        protected override Task PrepareExecutionAsync()
        {
            if (ShouldThrowInPrepareExecution)
            {
                ThrowAtMethod = nameof(PrepareExecutionAsync);
                throw Errors.InvalidOperation();
            }

            return base.PrepareExecutionAsync();
        }

        protected override Task ValidateAfterExecutionAsync()
        {
            if (ShouldThrowInValidateAfterExecution)
            {
                ThrowAtMethod = nameof(ValidateAfterExecutionAsync);
                throw Errors.InvalidOperation();
            }

            return base.ValidateAfterExecutionAsync();
        }

        protected override Task FinalizeAsync()
        {
            FinalizeIsCalled = true;

            if (ShouldThrowInFinalize)
            {
                ThrowAtMethod = nameof(FinalizeAsync);
                throw Errors.InvalidOperation();
            }

            return Task.CompletedTask;
        }

        protected override Task OnSuccessAsync()
        {
            OnSuccessIsCalled = true;

            if (ShouldThrowInOnSuccess)
            {
                ThrowAtMethod = nameof(OnSuccessAsync);
                throw Errors.InvalidOperation();
            }

            return Task.CompletedTask;
        }

        protected override Task OnErrorAsync(Exception e)
        {
            OnErrorIsCalled = true;

            if (ShouldThrowInOnError)
            {
                ThrowAtMethod = nameof(OnErrorAsync);
                throw Errors.InvalidOperation();
            }

            return Task.CompletedTask;
        }

        protected override Task ExecutionAsync()
        {
            if (ShouldThrowInExecution)
            {
                ThrowAtMethod = nameof(ExecutionAsync);
                throw Errors.InvalidOperation();
            }

            return Task.CompletedTask;
        }
    }
}

# Meteor Library

## MessageAsync

You can use `MessageAsync` class for wrapping an operation as a message object that is passing through the following steps in order:

1. `PreparePropertiesAsync`: Preparing input properties
2. `ValidatePropertiesAsync`: Validating input properties
3. `PrepareExecutionAsync`: Preparing for executing the operation
4. `ValidateBeforeExecutionAsync`: Validating environment before executing the operation
5. `ExecuteMessageAsync`: Executing the operation
6. `ValidateAfterExecutionAsync`: Validating environment after executing the operation
7. `FinalizeAsync`: Finalize and release resources

When you derive from `MessageAsync` class, `ExecuteMessageAsync` is an abstract method that should be implemented, however other methods are overridable.

You can stop execution flow in any step by throwing an exception, but
`FinalizeAsync` is always called at the end whether any exception is occured or not.

All steps are logged using `Serilog` library that can be customized.

## Database Operations

There are some predefined classes that you can use to wrap all your database operations.

- `IDbConnectionFactory`: An interface to open database connection.
- `DbConnectionFactory`: Default implementation of `IDbConnectionFactory` that uses `Dapper` library for database operations.
- `LazyDbConnection`: A database connection object that is just opened right before it's needed and can be used through series of messages as a shared connection. Once the connection is opened, it's kept open until the message is finished.
- `DbMessageAsync`: A base class for wrapping database related operations which support `LazyDbConnection` and transactional operations (if the target database support transactions).

`DbConnectionFactory` is already implemented for the following databases:
- PostgreSQL
- SQLite

However virtually any database connection and operation could be implemented using these classes.

## Other useful classes...
that needed to be documented...
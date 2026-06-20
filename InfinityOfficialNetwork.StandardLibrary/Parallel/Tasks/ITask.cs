using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InfinityOfficialNetwork.StandardLibrary.Parallel.Tasks;


public interface IAsyncAction : INotifyCompletion
{
	public IAsyncAction GetAwaiter() => this;

	public bool IsCompleted { get; }
	public void GetResult();
}

public interface IAsyncAction<out TSelf> : IAsyncAction
	where TSelf : IAsyncAction<TSelf>
{
	new public TSelf GetAwaiter();

	IAsyncAction IAsyncAction.GetAwaiter() => GetAwaiter();
}

[AsyncMethodBuilder(typeof(AsyncFuncInterfaceBuilder<>))]
public interface IAsyncFunc<out TArg> : INotifyCompletion
{
	public IAsyncFunc<TArg> GetAwaiter() => this;

	public bool IsCompleted { get; }
	public TArg GetResult();
}

public interface IAsyncFunc<out TSelf,out TArg> : IAsyncFunc<TArg>
	where TSelf : IAsyncFunc<TSelf,TArg>
{
	new public TSelf GetAwaiter();

	IAsyncFunc<TArg> IAsyncFunc<TArg>.GetAwaiter() => GetAwaiter();
}

[AsyncMethodBuilder(typeof(AsyncValueActionMethodBuilder))]
public record struct AsyncValueAction : IAsyncAction<AsyncValueAction>
{
	private ValueTask task;

	public bool IsCompleted => task.GetAwaiter().IsCompleted;

	public void GetResult() => task.GetAwaiter().GetResult();
	public void OnCompleted(Action continuation) => task.GetAwaiter().OnCompleted(continuation);

	public AsyncValueAction GetAwaiter() => this;

	public static implicit operator AsyncValueAction(ValueTask task) => new() { task = task };
}

[AsyncMethodBuilder(typeof(AsyncValueFuncMethodBuilder<>))]
public record struct AsyncValueFunc<TArg> : IAsyncFunc<AsyncValueFunc<TArg>, TArg>
{
	private ValueTask<TArg> task;

	public bool IsCompleted => task.GetAwaiter().IsCompleted;

	public TArg GetResult() => task.GetAwaiter().GetResult();
	public void OnCompleted(Action continuation) => task.GetAwaiter().OnCompleted(continuation);

	public AsyncValueFunc<TArg> GetAwaiter() => this;

	public static implicit operator AsyncValueFunc<TArg>(ValueTask<TArg> task) => new() { task = task };
}

public record struct AsyncValueActionMethodBuilder
{
	// We delegate the hard work to the existing system builder
	private AsyncValueTaskMethodBuilder _methodBuilder;

	// 1. Static Create method
	public static AsyncValueActionMethodBuilder Create() =>
		new() { _methodBuilder = AsyncValueTaskMethodBuilder.Create() };

	// 2. Start the state machine
	public void Start<TStateMachine>(ref TStateMachine stateMachine)
		where TStateMachine : IAsyncStateMachine =>
		_methodBuilder.Start(ref stateMachine);

	// 3. Set the state machine (rarely used in Core but required)
	public void SetStateMachine(IAsyncStateMachine stateMachine) =>
		_methodBuilder.SetStateMachine(stateMachine);

	// 4. Set Result (Void)
	public void SetResult() => _methodBuilder.SetResult();

	// 5. Set Exception
	public void SetException(Exception exception) => _methodBuilder.SetException(exception);

	// 6. The Task property that returns your custom type
	public AsyncValueAction Task => _methodBuilder.Task; // Implicit conversion works here

	// 7. AwaitOnCompleted
	public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
		where TAwaiter : INotifyCompletion
		where TStateMachine : IAsyncStateMachine =>
		_methodBuilder.AwaitOnCompleted(ref awaiter, ref stateMachine);

	// 8. AwaitUnsafeOnCompleted (Optimization for "Unsafe" context switches)
	public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
		where TAwaiter : ICriticalNotifyCompletion
		where TStateMachine : IAsyncStateMachine =>
		_methodBuilder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
}

public record struct AsyncValueFuncMethodBuilder<T>
{
	// Delegate to the generic system builder
	private AsyncValueTaskMethodBuilder<T> _methodBuilder;

	public static AsyncValueFuncMethodBuilder<T> Create() =>
		new() { _methodBuilder = AsyncValueTaskMethodBuilder<T>.Create() };

	public void Start<TStateMachine>(ref TStateMachine stateMachine)
		where TStateMachine : IAsyncStateMachine =>
		_methodBuilder.Start(ref stateMachine);

	public void SetStateMachine(IAsyncStateMachine stateMachine) =>
		_methodBuilder.SetStateMachine(stateMachine);

	// Set Result now takes an argument
	public void SetResult(T result) => _methodBuilder.SetResult(result);

	public void SetException(Exception exception) => _methodBuilder.SetException(exception);

	// The Task property returns your Generic custom type
	public AsyncValueFunc<T> Task => _methodBuilder.Task;

	public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
		where TAwaiter : INotifyCompletion
		where TStateMachine : IAsyncStateMachine =>
		_methodBuilder.AwaitOnCompleted(ref awaiter, ref stateMachine);

	public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
		where TAwaiter : ICriticalNotifyCompletion
		where TStateMachine : IAsyncStateMachine =>
		_methodBuilder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
}

public struct AsyncFuncInterfaceBuilder<T>
{
	// We delegate to the standard ValueTask builder for the heavy lifting
	private AsyncValueTaskMethodBuilder<T> _methodBuilder;

	public static AsyncFuncInterfaceBuilder<T> Create() =>
		new() { _methodBuilder = AsyncValueTaskMethodBuilder<T>.Create() };

	public void Start<TStateMachine>(ref TStateMachine stateMachine)
		where TStateMachine : IAsyncStateMachine =>
		_methodBuilder.Start(ref stateMachine);

	public void SetStateMachine(IAsyncStateMachine stateMachine) =>
		_methodBuilder.SetStateMachine(stateMachine);

	public void SetResult(T result) => _methodBuilder.SetResult(result);

	public void SetException(Exception exception) => _methodBuilder.SetException(exception);

	// CRITICAL PART:
	// The compiler expects this property to return 'IAsyncFunc<T>' 
	// because that is the return type of the method.
	// We create the Struct, and implicit casting BOXES it to the interface.
	public IAsyncFunc<T> Task => (AsyncValueFunc<T>)_methodBuilder.Task;

	public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
		where TAwaiter : INotifyCompletion
		where TStateMachine : IAsyncStateMachine =>
		_methodBuilder.AwaitOnCompleted(ref awaiter, ref stateMachine);

	public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
		where TAwaiter : ICriticalNotifyCompletion
		where TStateMachine : IAsyncStateMachine =>
		_methodBuilder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
}

public class a
{
	async AsyncValueFunc<int> aa()
	{
		IAsyncFunc<int> action = (AsyncValueFunc<int>)ValueTask.FromResult(123);
		await action;

		return 1234;
	}
}
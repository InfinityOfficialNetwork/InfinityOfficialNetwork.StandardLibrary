using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace InfinityOfficialNetwork.StandardLibrary.Contracts;

public enum ContractType
{
	Ensure,
	Precondition,
	Postcondition,
	Invariant,
}

public readonly struct ContractFailureRecord
{
	public readonly required IReadOnlyCollection<string> FailedConditions { get; init; }
	public readonly required ContractType ContractType { get; init; }
}

public class ContractFailureException(ContractFailureRecord record) : Exception("Contract failure occured, see FailureRecord for more information")
{
	public ContractFailureRecord FailureRecord { get; init; } = record;
}

public interface IContractCompliant
{
#if DEBUG
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public PreconditionContract BeginPrecondition() => new();
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public PostconditionContract BeginPostcondition() => new();
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public EnsureContract BeginEnsure() => new();
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public InvariantContract BeginInvariant() => new();
#else
	public static PreconditionContract BeginPrecondition() => null!;
	public static PostconditionContract BeginPostcondition() => null!;
	public static EnsureContract BeginEnsure() => null!;
	public static InvariantContract BeginInvariant() => null!;
#endif

#if DEBUG
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static PreconditionContract BeginStaticPrecondition() => new();
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static PostconditionContract BeginStaticPostcondition() => new();
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static EnsureContract BeginStaticEnsure() => new();
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static InvariantContract BeginStaticInvariant() => new();
#else
	public static PreconditionContract BeginPrecondition() => null!;
	public static PostconditionContract BeginPostcondition() => null!;
	public static EnsureContract BeginEnsure() => null!;
	public static InvariantContract BeginInvariant() => null!;
#endif
}

public sealed class PreconditionContract : Contract
{
	public override ContractType Type => ContractType.Precondition;
}

public sealed class PostconditionContract : Contract
{
	public override ContractType Type => ContractType.Postcondition;
}

public sealed class InvariantContract : Contract
{
	public override ContractType Type => ContractType.Invariant;
}

public sealed class EnsureContract : Contract
{
	public override ContractType Type => ContractType.Ensure;

#if DEBUG
	/// <summary>
	/// In debug builds, evaluates the specified condition and records it for diagnostic purposes.
	/// </summary>
	/// <remarks>This method is only included in builds where the <c>DEBUG</c> symbol is defined. In release builds,
	/// calls to this method are omitted at compile time.</remarks>
	/// <param name="predicate">An expression that represents the condition to be checked. The expression should return <see langword="true"/> if
	/// the condition is met; otherwise, <see langword="false"/>.</param>
	[Conditional("DEBUG")]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Condition(bool condition, [CallerArgumentExpression(nameof(condition))] string? expressionText = "") => base.Condition(condition, expressionText!);
#else
	[Conditional("DEBUG")]
	public void Ensure(bool condition, string? expressionText = null) {}
#endif
}

public abstract class Contract : IDisposable
{
	private static Func<ContractFailureRecord?, bool> handler = (e) => false;
	private static Action exitHandler = () => Environment.Exit(-1);
	private bool disposedValue;

#if DEBUG
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static PreconditionContract BeginPrecondition() => new();
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static PostconditionContract BeginPostcondition() => new();
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static EnsureContract BeginEnsure() => new();
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static InvariantContract BeginInvariant() => new();
#else
	public static PreconditionContract BeginPrecondition() => null!;
	public static PostconditionContract BeginPostcondition() => null!;
	public static EnsureContract BeginEnsure() => null!;
	public static InvariantContract BeginInvariant() => null!;
#endif


	public abstract ContractType Type { get; }

	/// <summary>
	/// Sets the handler called by Panic when contract assertion fails, return value determines whether program exits or not.
	/// </summary>
	/// <param name="handler"></param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetFailureHandler(Func<ContractFailureRecord?, bool> handler) => Contract.handler = handler;
	/// <summary>
	/// Sets the handler called by Panic to terminate the program. This is responsible for terminating the program.
	/// </summary>
	/// <param name="handler"></param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SetExitHandler(Action handler) => exitHandler = handler;

	/// <summary>
	/// Called when contract failure is detected
	/// </summary>
	/// <param name="ex"></param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void Panic(ContractFailureRecord ex)
	{
		Debug.WriteLine("Contract panic detected");
		Debug.WriteLine("Failed conditions:");
		foreach (var condition in ex.FailedConditions)
			Debug.WriteLine("\t{0}", condition);
		Debug.WriteLine("Stacktrace:");
		foreach (var frame in new StackTrace(1, true).GetFrames())
			Debug.WriteLine("\t{0}", frame.ToString());

		if (handler(ex))
		{
			Debug.WriteLine("Handler signaled continuation of execution, ignoring errors...");
			return;
		}
		else
		{
			Debug.WriteLine("Handler failed, crashing program.");
			if (Debugger.IsAttached)
				while (true)
					Debugger.Break();
			else
				exitHandler();
		}
	}

	private readonly List<(bool, string)> pendingConditions = [];

	[Conditional("DEBUG")]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Condition(bool condition, [CallerArgumentExpression(nameof(condition))] string expressionText = "")
	{
		ObjectDisposedException.ThrowIf(disposedValue, this);

		if (!condition)
			pendingConditions.Add((condition, expressionText));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private bool Assert(out List<string>? failedConditions)
	{
		disposedValue = true;
		failedConditions = null;
		foreach (var condition in pendingConditions)
			if (!condition.Item1)
				if (failedConditions == null)
					failedConditions = [condition.Item2];
				else
					failedConditions.Add(condition.Item2);
		return failedConditions == null;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AssertOrPanic()
	{
		ObjectDisposedException.ThrowIf(disposedValue, this);

		if (!Assert(out var failedConditions))
			Panic(new ContractFailureRecord() { FailedConditions = failedConditions!, ContractType = Type });
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AssertOrThrow()
	{
		ObjectDisposedException.ThrowIf(disposedValue, this);

		if (!Assert(out var failedConditions))
			throw new ContractFailureException(new ContractFailureRecord() { FailedConditions = failedConditions!, ContractType = Type });
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			AssertOrPanic();
			disposedValue = true;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize")]
	public void Dispose()
	{
#if DEBUG
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
#endif
	}
}


[AttributeUsage(AttributeTargets.Method)]
public class InvariantMethodAttribute : Attribute
{

}

[AttributeUsage(AttributeTargets.Method)]
public class StaticInvariantMethodAttribute : Attribute
{

}

[AttributeUsage(AttributeTargets.Method)]
public class PreconditionMethodAttribute : Attribute
{

}

[AttributeUsage(AttributeTargets.Method)]
public class StaticPreconditionMethodAttribute : Attribute
{

}

[AttributeUsage(AttributeTargets.Method)]
public class PostconditionMethodAttribute : Attribute
{

}

[AttributeUsage(AttributeTargets.Method)]
public class StaticPostconditionMethodAttribute : Attribute
{

}


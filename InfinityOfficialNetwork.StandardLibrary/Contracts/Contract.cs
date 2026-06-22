using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace InfinityOfficialNetwork.StandardLibrary.Contracts;

[AttributeUsage(AttributeTargets.Method)]
public sealed class InvariantMethodAttribute : Attribute
{ }

[AttributeUsage(AttributeTargets.Method)]
public sealed class StaticInvariantMethodAttribute : Attribute
{ }

[AttributeUsage(AttributeTargets.Method)]
public sealed class PreconditionMethodAttribute : Attribute
{ }

[AttributeUsage(AttributeTargets.Method)]
public sealed class StaticPreconditionMethodAttribute : Attribute
{ }

[AttributeUsage(AttributeTargets.Method)]
public sealed class PostconditionMethodAttribute : Attribute
{ }

[AttributeUsage(AttributeTargets.Method)]
public sealed class ExceptionPostconditionMethodAttribute : Attribute
{ }

[AttributeUsage(AttributeTargets.Method)]
public sealed class StaticPostconditionMethodAttribute : Attribute
{ }

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
public sealed class ContractCompliantAttribute : Attribute
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, AllowMultiple = false, Inherited = true)]
public sealed class ContractCompliantMethodAttribute : Attribute
{ }

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
public sealed class ContractGuaranteedAttribute : Attribute
{ }

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class ContractCompliantIgnoredMethodAttribute : Attribute
{ }

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class ImplementationMethodAttribute : Attribute
{ }

public enum ContractType
{
	Ensure,
	Precondition,
	Postcondition,
	Invariant,
}

public readonly record struct ContractFailureRecord
{
	public readonly required IReadOnlyCollection<string> FailedConditions { get; init; }
	//public readonly required ContractInvocationSource ContractInvocationSource { get; init; }
}

public class ContractFailureException(ContractFailureRecord record) : Exception()
{
	public ContractFailureRecord FailureRecord { get; init; } = record;

	public override string Message
		=> "Contract failure occured, see FailureRecord for more information\nFailure record:\n{0}" + FailureRecord.ToString();
}

[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "<Pending>")]
public struct ContractConditionRecord
{
	public ContractType ConditionType { get; set; }
	public bool Success { get; set; }
	public string Condition { get; set; }
}

[InlineArray(64)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "<Pending>")]
public struct ContractStorage
{
	ContractConditionRecord Records;
}

public ref struct ContractToken
{
	ref ContractStorage storage;
	int storageUsage;

	List<ContractConditionRecord>? extra = null;

	public ContractToken(ref ContractStorage storage)
	{
		this.storage = ref storage;
		storageUsage = 0;
	}

	void AddCondition(ContractType type, bool result, string conditionString)
	{
		if (storageUsage < 64)
		{
			storage[storageUsage++] = new ContractConditionRecord { Condition = conditionString, ConditionType = type, Success = result };
		}
		else
		{
			extra ??= [];
			extra.Add(new ContractConditionRecord { Condition = conditionString, ConditionType = type, Success = result });
		}
	}

	public void Precondition(bool result, [CallerArgumentExpression(nameof(result))] string conditionString = "")
		=> AddCondition(ContractType.Precondition, result, conditionString);
	public void Postcondition(bool result, [CallerArgumentExpression(nameof(result))] string conditionString = "")
	=> AddCondition(ContractType.Postcondition, result, conditionString);
	public void Invariant(bool result, [CallerArgumentExpression(nameof(result))] string conditionString = "")
	=> AddCondition(ContractType.Invariant, result, conditionString);
	public void Ensure(bool result, [CallerArgumentExpression(nameof(result))] string conditionString = "")
	=> AddCondition(ContractType.Ensure, result, conditionString);

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "<Pending>")]
	public readonly void ProcessAllConditions()
	{
		List<string>? failedConditions = null;

		for (int i = 0; i < storageUsage; i++)
			if (!storage[i].Success)
			{
				failedConditions ??= new List<string>();
				failedConditions.Add(storage[i].Condition);
			}

		if (extra != null)
			foreach (var rec in extra)
				if (!rec.Success)
				{
					failedConditions ??= new List<string>();
					failedConditions.Add(rec.Condition);
				}

		if (failedConditions == null)
			return;
		else
			Contract.ReportContractFailure(this, failedConditions);
	}
}

public static class Contract
{
	private static void Panic(ContractFailureRecord ex)
	{
		Debugger.Launch();

		Debug.WriteLine("Contract panic detected");
		Debug.WriteLine("Failed conditions:");
		foreach (var condition in ex.FailedConditions)
			Debug.WriteLine("\t{0}", condition);
		Debug.WriteLine("Stacktrace:");
		foreach (var frame in new StackTrace(1, true).GetFrames())
			Debug.WriteLine("\t{0}", frame.ToString());

		if (Debugger.IsAttached)
			while (true)
				Debugger.Break();
		else
			RequestExit();
	}

	private static void Throw(ContractFailureRecord ex)
	{
		throw new ContractFailureException(ex);
	}

	static Action<ContractFailureRecord> failureHandler = DefaultPanicHandler;
	static Action exitHandler = () => Environment.Exit(1);

	public static void SetFailureHandler(Action<ContractFailureRecord> failureHandler)
		=> Contract.failureHandler = failureHandler;

	public static void SetExitHandler(Action exitHandler)
		=> Contract.exitHandler = exitHandler;

	public static Action<ContractFailureRecord> DefaultPanicHandler => Panic;
	public static Action<ContractFailureRecord> DefaultThrowHandler => Throw;

	public static void ReportContractFailure(ContractToken token, IReadOnlyCollection<string> failedConditions)
	{
		ContractFailureRecord contractFailureRecord = new() { FailedConditions = failedConditions };

		failureHandler(contractFailureRecord);
	}

	public static void RequestExit() => exitHandler();
}
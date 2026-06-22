using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace InfinityOfficialNetwork.StandardLibrary.Utilities;
public static class Assert
{
	[DoesNotReturn]
	public static void Fail(string message,
		[CallerMemberName] string callerMemberName = "",
		[CallerLineNumber] int callerLineNumber = 0,
		[CallerFilePath] string callerFilePath = "")
	{
		Console.WriteLine("======================================================");
		Console.WriteLine("!!! Assertion failed !!!");
		Console.WriteLine("The program is in an exceptional state and cannot continue");
		Console.WriteLine($"Assertion at {callerFilePath}:{callerLineNumber}; {callerMemberName}");
		Console.WriteLine("======================================================");
		Console.WriteLine("CURRENT STACKTRACE:");
		Console.WriteLine(Environment.StackTrace);

		Debugger.Log(5, "ASSERTION_FAILURE", $"An assertion failed:\nStacktrace:\n{Environment.StackTrace}");
		if (Debugger.IsAttached)
			while (true)
				Debugger.Break();
		else
			Environment.Exit(1);
	}

	[Conditional("DEBUG")]
	public static void AssertTrue(bool condition)
	{
		if (!condition)
		{
			Console.WriteLine("Assertion failed");
			Debugger.Log(5, "ASSERTION_FAILURE", $"An assertion failed:\nStacktrace:\n{Environment.StackTrace}");
			if (Debugger.IsAttached)
				while (true)
					Debugger.Break();
			else
				Environment.Exit(1);
		}
	}

	private static readonly Dictionary<Expression<Func<bool>>, Func<bool>> precompiledExpressions = new();

	[Conditional("DEBUG")]
	public static void AssertTrue(Expression<Func<bool>> condition)
	{
		bool val;
		if (precompiledExpressions.TryGetValue(condition, out var result))
		{
			val = result.Invoke();
		}
		else
		{
			var func = condition.Compile(false);
			precompiledExpressions.Add(condition, func);
			val = func.Invoke();
		}

		if (!val)
		{
			Console.WriteLine("Assertion failed");
			Console.WriteLine(condition);
			Debugger.Log(5, "ASSERTION_FAILURE", $"An assertion failed: {condition}\nStacktrace:\n{Environment.StackTrace}");
			if (Debugger.IsAttached)
				while (true)
					Debugger.Break();
			else
				Environment.Exit(1);
		}
	}
}

using InfinityOfficialNetwork.StandardLibrary.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using static InfinityOfficialNetwork.StandardLibrary.Exceptions.Expected;

namespace InfinityOfficialNetwork.StandardLibrary.Contracts;

[ContractGuaranteed]
public interface IContractTest
{
	[InvariantMethod]
	public static void Invariant<T>(T inst, ContractToken c) where T : IContractTest
	{
		c.Invariant(true);
	}

	[StaticInvariantMethod]
	public static void StaticInvariant<T>(ContractToken c) where T : IContractTest
	{
		c.Invariant(true);
	}

	[PreconditionMethod]
	public static void DividePrecondition<T>(T inst, ContractToken c, int dividend, int divisor) where T : IContractTest
	{
		c.Precondition(divisor != 0);
	}

	[PostconditionMethod]
	public static void DividePostcondition<T>(T inst, ContractToken c, int dividend, int divisor, ref readonly int ret) where T : IContractTest
	{
		c.Postcondition(ret <= dividend);
	}

	[ExceptionPostconditionMethod]
	public static void DivideExceptionPostcondition<T>(T inst, ContractToken c, int dividend, int divisor, Exception ex) where T : IContractTest
	{
		c.Postcondition(ex.InnerException == null);
	}

	[ContractCompliantMethod]
	[StrongGuaranteeExceptionSpecification<Exception>]
	public int Divide(int dividend, int divisor);
}

[ContractGuaranteed]
public partial class ContractTest : IContractTest
{
	[ContractCompliantMethod]
	public partial int Divide(int dividend, int divisor);

	public partial int Divide(int dividend, int divisor)
	{
		{
			ContractStorage contractStorage = new();
			ContractToken contractToken = new(ref contractStorage);
			IContractTest.Invariant(this, contractToken);
			IContractTest.StaticInvariant<ContractTest>(contractToken);
			IContractTest.DividePrecondition(this, contractToken, dividend, divisor);
			contractToken.ProcessAllConditions();
		}

		int result;

		try
		{
			result = DivideImpl(dividend, divisor);
		}
		catch (Exception ex)
		{
			ContractStorage contractStorage = new();
			ContractToken contractToken = new(ref contractStorage);
			IContractTest.Invariant(this, contractToken);
			IContractTest.StaticInvariant<ContractTest>(contractToken);
			IContractTest.DivideExceptionPostcondition(this, contractToken, dividend, divisor, ex);
			contractToken.ProcessAllConditions();
			throw;
		}

		{
			ContractStorage contractStorage = new();
			ContractToken contractToken = new(ref contractStorage);
			IContractTest.Invariant(this, contractToken);
			IContractTest.StaticInvariant<ContractTest>(contractToken);
			IContractTest.DividePostcondition(this, contractToken, dividend, divisor, ref result);
			contractToken.ProcessAllConditions();
		}

		return result;
	}

	[ImplementationMethod]
	private int DivideImpl(int dividend, int divisor)
	{
		return dividend / divisor;
	}
}


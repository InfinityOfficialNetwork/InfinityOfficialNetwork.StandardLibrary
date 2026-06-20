using InfinityOfficialNetwork.StandardLibrary.Attributes;
using InfinityOfficialNetwork.StandardLibrary.Parallel.Threading;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace InfinityOfficialNetwork.StandardLibrary.Exceptions;

public static class Expected
{
	public ref struct Result<TArg>
		where TArg : allows ref struct
	{
		public TArg Value;
	}

	public ref struct Exception<TExcept>
		where TExcept : allows ref struct
	{
		public TExcept Value;
	}

	public static Result<TArg> FromResult<TArg>(TArg result)
		=> new() { Value = result };

	public static Exception<TArg> FromException<TArg>(TArg ex)
	=> new() { Value = ex };
}

[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "<Pending>")]
public ref struct Expected<TArg, TExcept1>
	where TArg : allows ref struct
	where TExcept1 : allows ref struct
{
	private ref struct ExpectedStorage
	{
		public TArg result;
		public TExcept1 except1;
	}

	byte index;
	ExpectedStorage storage;

	public static implicit operator Expected<TArg, TExcept1>(TArg result)
		=> new()
		{ index = 0, storage = new ExpectedStorage { result = result } };

	public static implicit operator Expected<TArg, TExcept1>(Expected.Result<TArg> result)
		=> new()
		{ index = 0, storage = new ExpectedStorage { result = result.Value } };

	public static implicit operator Expected<TArg, TExcept1>(Expected.Exception<TExcept1> result)
		=> new()
		{ index = 1, storage = new ExpectedStorage { except1 = result.Value } };

	public static implicit operator TArg(Expected<TArg, TExcept1> result)
		=> result.storage.result;

	[Unverified]
	public readonly TArg Result => storage.result;
	[Unverified]
	public readonly TExcept1 Exception1 => storage.except1;

	public readonly byte Index => index;

	public readonly void Visit(Action<TArg> onResult, Action<TExcept1> onException)
	{
		if (index == 0)
			onResult(storage.result);
		else
			onException(storage.except1);
	}

	public readonly TRet Visit<TRet>(Func<TArg, TRet> onResult, Func<TExcept1, TRet> onException)
		=> index == 0 ? onResult(storage.result) : onException(storage.except1);

	public readonly void Deconstruct(out byte index, out TArg result, out TExcept1 exception)
	{
		index = this.index;
		Unsafe.SkipInit(out result);
		Unsafe.SkipInit(out exception);

		if (index == 0)
			result = storage.result;
		else
			exception = storage.except1;
	}
}
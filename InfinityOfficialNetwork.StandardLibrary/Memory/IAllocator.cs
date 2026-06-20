using InfinityOfficialNetwork.StandardLibrary.Attributes;
using InfinityOfficialNetwork.StandardLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CA1815
#pragma warning disable CA1000
#pragma warning disable SYSLIB0051

namespace InfinityOfficialNetwork.StandardLibrary.Memory;

public class InvalidAllocationException : OutOfMemoryException
{
	public required nuint AllocationSize { get; init; } = 0;
	public override string Message => "Invalid allocation size" + (AllocationSize > 0 ? $" of {AllocationSize}" : "") + " or not enough memory available.";

	public InvalidAllocationException() { }
	public InvalidAllocationException(nuint allocationSize)
	{
		AllocationSize = allocationSize;
	}
	public InvalidAllocationException(nuint allocationSize, Exception innerException) : base("Invalid allocation size or not enough memory available.",innerException)
	{
		AllocationSize = allocationSize;
	}
	public InvalidAllocationException(string message) : base(message) { }
	public InvalidAllocationException(string message, Exception innerException) : base(message, innerException) { }
	protected InvalidAllocationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}

public interface IAllocator
{
	/// <summary>
	/// Allocates memory at least size nuint
	/// <br/>
	/// If the allocator fails, returns nullptr. Does not throw for any reason.
	/// </summary>
	/// <param name="size"></param>
	/// <returns></returns>
	[NothrowGuaranteeExceptionSpecification]
	public unsafe static abstract nuint Allocate(nuint size);
	/// <summary>
	/// Frees memory held by ptr. Undefined behavior if ptr was not allocated by the same instance or was already freed
	/// </summary>
	/// <param name="ptr"></param>
	[NothrowGuaranteeExceptionSpecification]
	public unsafe static abstract void Free(nuint ptr);
}

public struct NativeAllocator : IAllocator
{
	[NothrowGuaranteeExceptionSpecification]
	public static unsafe nuint Allocate(nuint size)
	{
		try
		{
			return (nuint)NativeMemory.Alloc((nuint)size);
		}
		catch (OutOfMemoryException)
		{
			return default;
		}
		catch
		{
			Assert.Fail("NON-SPEC EXCEPTION CAUGHT");
			throw;
		}
	}

	[NothrowGuaranteeExceptionSpecification]
	public static unsafe void Free(nuint ptr) => NativeMemory.Free((void*)ptr);
}

public static class AllocatorUtilities<TAllocator> where TAllocator : IAllocator
{
	[StrongGuaranteeExceptionSpecification<InvalidAllocationException>]
	public unsafe static TArg* AllocateUninitializedObject<TArg>() => (TArg*)TAllocator.Allocate((nuint)sizeof(TArg));
	public unsafe static TArg* AllocateZeroedObject<TArg>()
	{
		var ptr = (TArg*)TAllocator.Allocate((nuint)sizeof(TArg));
		Unsafe.InitBlock(ptr, 0, (uint)sizeof(TArg));
		return ptr;
	}

	public unsafe static ref TArg AllocateUninitializedObjectRef<TArg>() => ref *(TArg*)TAllocator.Allocate((nuint)sizeof(TArg));


	public unsafe static TArg* AllocateUninitializedArray<TArg>(nuint count) => (TArg*)TAllocator.Allocate((nuint)sizeof(TArg) * count);
	public unsafe static TArg* AllocateZeroedArray<TArg>(nuint count)
	{
		var ptr = (TArg*)TAllocator.Allocate((nuint)sizeof(TArg) * count);
		Unsafe.InitBlock(ptr, 0, (uint)sizeof(TArg));
		return ptr;
	}

	public unsafe static TArg** AllocateUninitializedContiguousArray<TArg>(nuint count1, nuint count2)
	{
		TArg** ptr = (TArg**)TAllocator.Allocate((nuint)sizeof(TArg*) * count1 + (nuint)sizeof(TArg) * count1 * count2);
		
		TArg* adjustedPtr = (TArg*)(ptr + count1);
		for (nuint i = 0; i < count1; i++)
		{
			ptr[i] = &(adjustedPtr[i * count2]);
		}

		return ptr;
	}


	public unsafe static void FreeObject<TArg>(TArg* ptr) => TAllocator.Free((nuint)ptr);
	public unsafe static void FreeObjectRef<TArg>(ref TArg ptr) => TAllocator.Free((nuint)Unsafe.AsPointer(ref ptr));
	public unsafe static void FreeArray<TArg>(TArg* ptr) => TAllocator.Free((nuint)ptr);
	public unsafe static void FreeArray<TArg>(TArg** ptr) => TAllocator.Free((nuint)ptr);
	public unsafe static void FreeArray<TArg>(TArg*** ptr) => TAllocator.Free((nuint)ptr);
	public unsafe static void FreeArray<TArg>(TArg**** ptr) => TAllocator.Free((nuint)ptr);
	public unsafe static void FreeArray<TArg>(TArg***** ptr) => TAllocator.Free((nuint)ptr);
	public unsafe static void FreeArray<TArg>(TArg****** ptr) => TAllocator.Free((nuint)ptr);
	public unsafe static void FreeArray<TArg>(TArg******* ptr) => TAllocator.Free((nuint)ptr);
	public unsafe static void FreeArray<TArg>(TArg******** ptr) => TAllocator.Free((nuint)ptr);
}

//public abstract class PolymorphicAllocator
//{
//	/// <summary>
//	/// Allocates memory at least size nuint
//	/// </summary>
//	/// <param name="size"></param>
//	/// <returns></returns>
//	/// <exception cref="InvalidAllocationException"></exception>
//	public unsafe abstract nuint Allocate(nuint size);
//	/// <summary>
//	/// Frees memory held by ptr. Undefined behavior if ptr was not allocated by the same instance or was already freed
//	/// </summary>
//	/// <param name="ptr"></param>
//	public unsafe abstract void Free(nuint ptr);

//	public unsafe THandle* AllocateUninitializedObject<THandle>() => (THandle*)Allocate((nuint)sizeof(THandle));
//	public unsafe THandle* AllocateZeroedObject<THandle>()
//	{
//		var ptr = (THandle*)Allocate((nuint)sizeof(THandle));
//		Unsafe.InitBlock(ptr, 0, (uint)sizeof(THandle));
//		return ptr;
//	}

//	public unsafe ref THandle AllocateUninitializedObjectRef<THandle>() => ref *(THandle*)Allocate((nuint)sizeof(THandle));


//	public unsafe THandle* AllocateUninitializedArray<THandle>(nuint count) => (THandle*)Allocate((nuint)(sizeof(THandle) * count));
//	public unsafe THandle* AllocateZeroedArray<THandle>(nuint count)
//	{
//		var ptr = (THandle*)Allocate((nuint)(sizeof(THandle) * count));
//		Unsafe.InitBlock(ptr, 0, (uint)sizeof(THandle));
//		return ptr;
//	}

//	public unsafe THandle** AllocateUninitializedContiguousArray<THandle>(nuint count1, nuint count2)
//	{
//		THandle** ptr = (THandle**)Allocate((nuint)(sizeof(THandle*) * count1) + (nuint)(sizeof(THandle) * count1 * count2));

//		THandle* adjustedPtr = (THandle*)(ptr + count1);
//		for (nuint i = 0; i < count1; i++)
//		{
//			ptr[i] = &(adjustedPtr[i * count2]);
//		}

//		return ptr;
//	}


//	public unsafe void FreeObject<THandle>(THandle* ptr) => Free((nuint)ptr);
//	public unsafe void FreeObjectRef<THandle>(ref THandle ptr) => Free((nuint)Unsafe.AsPointer(ref ptr));
//	public unsafe void FreeArray<THandle>(THandle* ptr) => Free((nuint)ptr);
//	public unsafe void FreeArray<THandle>(THandle** ptr) => Free((nuint)ptr);
//	public unsafe void FreeArray<THandle>(THandle*** ptr) => Free((nuint)ptr);
//	public unsafe void FreeArray<THandle>(THandle**** ptr) => Free((nuint)ptr);
//	public unsafe void FreeArray<THandle>(THandle***** ptr) => Free((nuint)ptr);
//	public unsafe void FreeArray<THandle>(THandle****** ptr) => Free((nuint)ptr);
//	public unsafe void FreeArray<THandle>(THandle******* ptr) => Free((nuint)ptr);
//	public unsafe void FreeArray<THandle>(THandle******** ptr) => Free((nuint)ptr);
//}

//public class PolymorphicAllocator<TAllocator> : PolymorphicAllocator
//	where TAllocator : IAllocator
//{
//	public override unsafe nuint Allocate(nuint size) => TAllocator.Allocate(size);
//	public override void Free(nuint ptr) => TAllocator.Free(ptr);
//}
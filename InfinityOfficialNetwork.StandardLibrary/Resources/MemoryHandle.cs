using InfinityOfficialNetwork.StandardLibrary.Attributes;
using InfinityOfficialNetwork.StandardLibrary.Memory;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;

namespace InfinityOfficialNetwork.StandardLibrary.Resources;

public unsafe struct MemoryHandle<TArg, TAllocator> : IHandle<MemoryHandle<TArg, TAllocator>>, IMemoryHandle<TArg>
	where TArg : unmanaged
	where TAllocator : IAllocator
{
	TArg* pointer;

	public static MemoryHandle<TArg, TAllocator> AllocateUninitializedObject()
	{
		TArg* ptr = AllocatorUtilities<TAllocator>.AllocateUninitializedObject<TArg>();
		return new MemoryHandle<TArg, TAllocator> { pointer = ptr };
	}

	public static MemoryHandle<TArg, TAllocator> AllocateZeroedObject()
	{
		TArg* ptr = AllocatorUtilities<TAllocator>.AllocateZeroedObject<TArg>();
		return new MemoryHandle<TArg, TAllocator> { pointer = ptr };
	}

	public static MemoryHandle<TArg, TAllocator> AllocateUninitializedArray(nuint size)
	{
		TArg* ptr = AllocatorUtilities<TAllocator>.AllocateUninitializedArray<TArg>(size);
		return new MemoryHandle<TArg, TAllocator> { pointer = ptr };
	}

	public static MemoryHandle<TArg, TAllocator> AllocateZeroedArray(nuint size)
	{
		TArg* ptr = AllocatorUtilities<TAllocator>.AllocateZeroedArray<TArg>(size);
		return new MemoryHandle<TArg, TAllocator> { pointer = ptr };
	}

	public TArg* Pointer => pointer;
	public ref TArg Ref => ref Unsafe.AsRef<TArg>(pointer);

	public bool Active => pointer == default;

	public void Dispose()
	{
		AllocatorUtilities<TAllocator>.FreeObject(pointer);
	}
	public void MoveTo(ref MemoryHandle<TArg, TAllocator> target) => throw new NotImplementedException();


	public MemoryHandle<TArg> ToMemoryHandle() => new MemoryHandle<TArg>(pointer, &TAllocator.Free);
	public bool Equals(MemoryHandle<TArg, TAllocator> other) => throw new NotImplementedException();

	public static implicit operator MemoryHandle<TArg>(MemoryHandle<TArg, TAllocator> target) => target.ToMemoryHandle();

	public static bool operator ==(MemoryHandle<TArg, TAllocator> left, MemoryHandle<TArg, TAllocator> right) => throw new NotImplementedException();
	public static bool operator !=(MemoryHandle<TArg, TAllocator> left, MemoryHandle<TArg, TAllocator> right) => throw new NotImplementedException();
}

public unsafe struct MemoryHandle<TArg> : IHandle<MemoryHandle<TArg>>, IMemoryHandle<TArg>
	where TArg : unmanaged
{
	TArg* pointer;
	delegate* managed<nuint, void> deallocator;

	internal MemoryHandle(TArg* pointer, delegate* managed<nuint, void> deallocator)
	{
		this.pointer = pointer;
		this.deallocator = deallocator;
	}

	[NothrowGuaranteeExceptionSpecification]
	public MemoryHandle(MoveReference<MemoryHandle<TArg>> moveHandle)
	{
		moveHandle.MoveTo(ref this);
	}

	[StrongGuaranteeExceptionSpecification<InvalidAllocationException>]
	public static MemoryHandle<TArg> AllocateUninitializedObject<TAllocator>()
		where TAllocator : IAllocator
	{
		TArg* ptr = AllocatorUtilities<TAllocator>.AllocateUninitializedObject<TArg>();
		return new MemoryHandle<TArg> { pointer = ptr, deallocator = &TAllocator.Free };
	}

	[StrongGuaranteeExceptionSpecification<InvalidAllocationException>]
	public static MemoryHandle<TArg> AllocateZeroedObject<TAllocator>()
		where TAllocator : IAllocator
	{
		TArg* ptr = AllocatorUtilities<TAllocator>.AllocateZeroedObject<TArg>();
		return new MemoryHandle<TArg> { pointer = ptr, deallocator = &TAllocator.Free };
	}

	[StrongGuaranteeExceptionSpecification<InvalidAllocationException>]
	public static MemoryHandle<TArg> AllocateUninitializedArray<TAllocator>(nuint size)
		where TAllocator : IAllocator
	{
		TArg* ptr = AllocatorUtilities<TAllocator>.AllocateUninitializedArray<TArg>(size);
		return new MemoryHandle<TArg> { pointer = ptr, deallocator = &TAllocator.Free };
	}

	[StrongGuaranteeExceptionSpecification<InvalidAllocationException>]
	public static MemoryHandle<TArg> AllocateZeroedArray<TAllocator>(nuint size)
		where TAllocator : IAllocator
	{
		TArg* ptr = AllocatorUtilities<TAllocator>.AllocateZeroedArray<TArg>(size);
		return new MemoryHandle<TArg> { pointer = ptr, deallocator = &TAllocator.Free };
	}

	public TArg* Pointer { [NothrowGuaranteeExceptionSpecification] get => pointer; }

	public ref TArg Ref { [NothrowGuaranteeExceptionSpecification] get => ref Unsafe.AsRef<TArg>(pointer); }


	public bool Active { [NothrowGuaranteeExceptionSpecification] get => pointer == default; }

	[NothrowGuaranteeExceptionSpecification]
	public void Dispose()
	{
		deallocator((nuint)pointer);
	}

	[NothrowGuaranteeExceptionSpecification]
	public void MoveTo(ref MemoryHandle<TArg> target)
	{
		target.pointer = pointer;
		pointer = null;
	}
}
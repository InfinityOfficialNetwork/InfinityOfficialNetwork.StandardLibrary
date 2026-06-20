using InfinityOfficialNetwork.StandardLibrary.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfinityOfficialNetwork.StandardLibrary.Resources;
public unsafe struct SharedHandle<TArg, TAllocator> : ICopyableHandle<SharedHandle<TArg, TAllocator>>
	where TArg : unmanaged, ICopyableHandle<TArg>
	where TAllocator : IAllocator
{
	private struct ControlBlock
	{
		public long RefCount = 0;
		public long ThisRefCount = 0;

		public ControlBlock() { }
	}

	private TArg* data;
	private ControlBlock* controlBlock;

	public SharedHandle(MoveReference<TArg> moveReference)
	{
		data = AllocatorUtilities<TAllocator>.AllocateUninitializedObject<TArg>();
		moveReference.MoveTo(ref *data);

		controlBlock = AllocatorUtilities<TAllocator>.AllocateUninitializedObject<ControlBlock>();
		controlBlock->RefCount = 1;
		controlBlock->ThisRefCount = 1;
	}

	public bool Active => data != null;

	public void CopyTo(ref SharedHandle<TArg, TAllocator> target)
	{
		target.data = data;
		target.controlBlock = controlBlock;
		controlBlock->RefCount++;
		controlBlock->ThisRefCount++;
	}

	public void Dispose()
	{
		controlBlock->RefCount--;
		controlBlock->ThisRefCount--;

		if (controlBlock->RefCount == 0)
		{
			data->Dispose();
			AllocatorUtilities<TAllocator>.FreeObject(data);
		}
		if (controlBlock->ThisRefCount == 0)
		{
			AllocatorUtilities<TAllocator>.FreeObject(controlBlock);
		}
	}

	public void MoveTo(ref SharedHandle<TArg, TAllocator> target)
	{
		target.data = data;
		target.controlBlock = controlBlock;
		data = null;
		controlBlock = null;
	}
}

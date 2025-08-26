using InfinityOfficialNetwork.StandardLibrary.MemoryManagement.Handles;
using InfinityOfficialNetwork.StandardLibrary.MemoryManagement.Memory;

namespace InfinityOfficialNetwork.StandardLibrary.MemoryManagement.Objects.ReferenceTypes
{
	public class NativeReferenceTypeAllocator<TArg> : IObjectAllocator<TArg> where TArg : class
	{
		private class NativeObjectHandle : ObjectHandle<TArg>
		{
			internal unsafe NativeObjectHandle(NativeReferenceTypeAllocator<TArg> nativeMemoryAllocator, TArg* handle)
			{
				_memoryAllocator = nativeMemoryAllocator;
				SetHandle((nint)handle);
			}
			private readonly NativeReferenceTypeAllocator<TArg> _memoryAllocator;

			protected override unsafe bool ReleaseHandle()
			{
				_memoryAllocator.Free((TArg*)handle);
				return true;
			}
		}

		public NativeReferenceTypeAllocator(NativeMemoryAllocator allocator)
		{
			_allocator = allocator;
		}

		private readonly NativeMemoryAllocator _allocator;
		public unsafe TArg* AllocateObject() => (TArg*)_allocator.Allocate(sizeof(TArg));

		public unsafe TArg* AllocateObject(nint count) => (TArg*)_allocator.Allocate(sizeof(TArg) * count);

		public unsafe ObjectHandle<TArg> AllocateObjectHandle()
		{
			TArg* ptr = AllocateObject();
			return new NativeObjectHandle(this, ptr);
		}

		public unsafe ObjectHandle<TArg> AllocateObjectHandle(nint count)
		{
			TArg* ptr = AllocateObject(count);
			return new NativeObjectHandle(this, ptr);
		}

		public unsafe void Free(TArg* ptr)
		{
			_allocator.Free((nint)ptr);
		}
	}
}

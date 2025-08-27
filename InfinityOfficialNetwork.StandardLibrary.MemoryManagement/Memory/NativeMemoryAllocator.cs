using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using InfinityOfficialNetwork.StandardLibrary.MemoryManagement.Handles;

namespace InfinityOfficialNetwork.StandardLibrary.MemoryManagement.Memory
{
	public class NativeMemoryAllocator : IMemoryAllocator
	{
		private class NativeMemoryHandle : MemoryHandle
		{
			internal NativeMemoryHandle(NativeMemoryAllocator nativeMemoryAllocator, nint handle)
			{
				_memoryAllocator = nativeMemoryAllocator;
				SetHandle(handle);
			}
			private readonly NativeMemoryAllocator _memoryAllocator;

			protected override bool ReleaseHandle()
			{
				_memoryAllocator.Free(handle);
				return true;
			}
		}

		public class InvalidFreeException : InvalidOperationException
		{
			public InvalidFreeException(string? message) : base(message)
			{ }
		}
#if DEBUG

		readonly ConcurrentDictionary<nint, nint> allocatedPtrs = new();


		public nint Allocate(nint size)
		{
			try
			{
				nint ptr = Marshal.AllocHGlobal(size);
				GC.AddMemoryPressure(size);
				allocatedPtrs.TryAdd(ptr, size);
				return ptr;
			}
			catch (OutOfMemoryException ex)
			{
				throw new InvalidAllocationException(size, ex);
			}
		}

		public MemoryHandle AllocateHandle(nint size)
		{
			try
			{
				nint ptr = Marshal.AllocHGlobal(size);
				GC.AddMemoryPressure(size);
				allocatedPtrs.TryAdd(ptr, size);
				return new NativeMemoryHandle(this, ptr);
			}
			catch (OutOfMemoryException ex)
			{
				throw new InvalidAllocationException(size, ex);
			}
		}

		public void Free(nint ptr)
		{
			if (allocatedPtrs.TryRemove(ptr, out nint size))
			{
				Marshal.FreeHGlobal(ptr);
				GC.RemoveMemoryPressure(size);
			}
			else
				throw new InvalidFreeException($"Pointer {ptr.ToString("x")} was not allocated by current allocator or was already freed. THIS IS A CRITICAL MEMORY ERROR!");
		}
#else
		public nint Allocate(nint size) => Marshal.AllocHGlobal(size);
		public MemoryHandle AllocateHandle(nint size)
		{
			nint ptr = Marshal.AllocHGlobal(size);
			return new NativeMemoryHandle(this, ptr);
		}
		public void Free(nint ptr) => Marshal.FreeHGlobal(ptr);
#endif


		private class NativeObjectHandle<TArg> : ObjectHandle<TArg>
		{
			internal unsafe NativeObjectHandle(NativeMemoryAllocator nativeMemoryAllocator, TArg* handle)
			{
				_memoryAllocator = nativeMemoryAllocator;
				SetHandle((nint)handle);
			}
			private readonly NativeMemoryAllocator _memoryAllocator;

			protected override unsafe bool ReleaseHandle()
			{
				_memoryAllocator.Free(handle);
				handle = IntPtr.Zero;
				return true;
			}
		}


		public unsafe TArg* AllocateObject<TArg>() => (TArg*)Allocate(sizeof(TArg));

		public unsafe TArg* AllocateObject<TArg>(nint count) => (TArg*)Allocate(sizeof(TArg) * count);

		public unsafe ObjectHandle<TArg> AllocateObjectHandle<TArg>() => new NativeObjectHandle<TArg>(this, AllocateObject<TArg>());

		public unsafe ObjectHandle<TArg> AllocateObjectHandle<TArg>(nint count) => new NativeObjectHandle<TArg>(this, AllocateObject<TArg>(count));

		public unsafe void Free<TArg>(TArg* ptr) => Free((nint)ptr);

		public void Dispose() { }

	}
}
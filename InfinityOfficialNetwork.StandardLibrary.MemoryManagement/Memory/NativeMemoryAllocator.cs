using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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


		public void Dispose() { }

	}
}
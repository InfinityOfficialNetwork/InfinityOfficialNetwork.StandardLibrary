using System.Runtime.InteropServices;

namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Allocators
{
	public class NativeAllocator : IAllocator
	{
		public unsafe void* Allocate(nint size) => NativeMemory.Alloc((nuint)size);

		public unsafe T* Allocate<T>(nint count) where T : struct => (T*)NativeMemory.Alloc((nuint)(count * sizeof(T)));

		public unsafe void Free(void* ptr) => NativeMemory.Free(ptr);

		public unsafe void Free<T>(T* ptr) where T : struct => NativeMemory.Free(ptr);
	}
}

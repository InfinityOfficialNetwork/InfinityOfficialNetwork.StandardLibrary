using InfinityOfficialNetwork.StandardLibrary.MemoryManagement.Handles;

namespace InfinityOfficialNetwork.StandardLibrary.MemoryManagement.Memory
{
	public interface IMemoryAllocator : IDisposable
	{
		/// <summary>
		/// Allocates memory of size aligned to 8-byte boundaries. Throws OutOfMemoryException if the allocator is unable to allocate the memory block.
		/// </summary>
		/// <param name="size">Size of memory block to allocate</param>
		/// <returns></returns>
		/// <exception cref="InvalidAllocationException"></exception>
		public nint Allocate(nint size);
		/// <summary>
		/// Allocates memory of size aligned to 8-byte boundaries. Throws OutOfMemoryException if the allocator is unable to allocate the memory block.
		/// </summary>
		/// <param name="size">Size of memory block to allocate</param>
		/// <returns></returns>
		/// <exception cref="InvalidAllocationException"></exception>
		public MemoryHandle AllocateHandle(nint size);
		/// <summary>
		/// Frees the memory block ptr. The memory block MUST have been allocated by this instance. Exceptions will only be generated in DEBUG mode, it will be up to the implementation to crash the program on release configuration. If ptr is NULL, does nothing.
		/// </summary>
		/// <param name="ptr"></param>
		public void Free(nint ptr);

		public unsafe TArg* AllocateObject<TArg>();
		public unsafe TArg* AllocateObject<TArg>(nint count);
		public ObjectHandle<TArg> AllocateObjectHandle<TArg>();
		public ObjectHandle<TArg> AllocateObjectHandle<TArg>(nint count);
		public unsafe void Free<TArg>(TArg* ptr);

	}

	public class InvalidAllocationException : OutOfMemoryException
	{
		public InvalidAllocationException(nint size) : base($"An allocation of size {size} failed because there was not enough memory or it was an invalid value")
		{ }

		public InvalidAllocationException(nint size, Exception ex) : base($"An allocation of size {size} failed because there was not enough memory or it was an invalid value", ex)
		{ }
	}
}

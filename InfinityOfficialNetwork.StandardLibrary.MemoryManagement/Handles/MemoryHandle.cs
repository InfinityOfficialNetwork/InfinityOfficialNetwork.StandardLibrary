using System.Runtime.InteropServices;

namespace InfinityOfficialNetwork.StandardLibrary.MemoryManagement.Handles
{
	public abstract class MemoryHandle : SafeHandle
	{
		public MemoryHandle() : base(nint.Zero, true)
		{ }

		protected MemoryHandle(nint ptr) : base(nint.Zero, true)
		{
			SetHandle(ptr);
		}

		public override bool IsInvalid => handle == nint.Zero;

		public nint Pointer => handle;
	}
}

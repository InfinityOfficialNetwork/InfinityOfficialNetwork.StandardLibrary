using System.Runtime.InteropServices;

namespace InfinityOfficialNetwork.StandardLibrary.Memory.RpMalloc;

public unsafe partial class RpMallocAllocator : IAllocator
{
	public static unsafe nuint Allocate(nuint size) => throw new NotImplementedException("Not implemented, cannot find a dll for this allocator that has proper exports");
	public static void Free(nuint ptr) => throw new NotImplementedException("Not implemented, cannot find a dll for this allocator that has proper exports");

	[LibraryImport("rpmalloc.dll",EntryPoint ="rpmalloc")]
	[UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
	private static unsafe partial nuint rpmalloc(nuint size);

	[LibraryImport("rpmalloc.dll", EntryPoint ="rpmalloc")]
	[UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
	private static unsafe partial void rpfree(nuint ptr);
}

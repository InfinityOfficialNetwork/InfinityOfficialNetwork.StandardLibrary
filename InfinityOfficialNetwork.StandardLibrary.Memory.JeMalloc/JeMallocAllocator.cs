using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace InfinityOfficialNetwork.StandardLibrary.Memory.JeMalloc;


internal delegate nuint FuncAlloc(nuint size);
internal delegate void FuncFree(nuint ptr);

internal static class JeMallocInternal
{
	[DllImport("jemalloc", EntryPoint = "je_malloc", CallingConvention = CallingConvention.Cdecl)]
	public static unsafe extern nuint je_malloc(nuint size);

	[DllImport("jemalloc", EntryPoint = "je_free", CallingConvention = CallingConvention.Cdecl)]
	public static unsafe extern void je_free(nuint ptr);
}

public class JeMallocAllocator : IAllocator
{
	public static unsafe nuint Allocate(nuint size) => JeMallocInternal.je_malloc(size);
	public static unsafe void Free(nuint ptr) => JeMallocInternal.je_free(ptr);

	//must be init'd by single thread
	static JeMallocAllocator()
	{
		Free(Allocate(1));
	}
}


public class JeMallocAllocator<TFallbackAllocator> : IAllocator
	where TFallbackAllocator : IAllocator
{
	private static readonly bool libraryLoaded;

	public static unsafe nuint Allocate(nuint size)
	{
		if (libraryLoaded)
			return JeMallocInternal.je_malloc(size);
		else
			return TFallbackAllocator.Allocate(size);
	}

	public static unsafe void Free(nuint ptr)
	{
		if (libraryLoaded)
			JeMallocInternal.je_free(ptr);
		else
			TFallbackAllocator.Free(ptr);
	}

	//[ModuleInitializer]
	//internal unsafe static void Initialize()
	static JeMallocAllocator()
	{
		if (NativeLibrary.TryLoad("jemalloc", Assembly.GetCallingAssembly(), DllImportSearchPath.AssemblyDirectory, out nint handle))
		{
			libraryLoaded = true;
			NativeLibrary.Free(handle);
		}
		else
		{
			libraryLoaded = false;
		}
	}

}
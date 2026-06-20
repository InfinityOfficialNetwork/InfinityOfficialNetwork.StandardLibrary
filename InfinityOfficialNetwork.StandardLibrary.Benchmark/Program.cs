using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using InfinityOfficialNetwork.StandardLibrary.Memory;
using InfinityOfficialNetwork.StandardLibrary.Memory.JeMalloc;
using System.Diagnostics.CodeAnalysis;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Runtime.Loader;

namespace InfinityOfficialNetwork.StandardLibrary.Benchmark;

internal class Program
{


	static void Main(string[] args)
	{
		JeMallocAllocator.Allocate(10);

		var config = new DebugInProcessConfig();

		ThreadPool.SetMinThreads(24, 24);
		ThreadPool.SetMaxThreads(240, 240);

		BenchmarkSwitcher
			.FromAssembly(typeof(Program).Assembly) // Or the assembly containing your benchmarks
			.Run(args, config);
	}
}

//public class AllocatorSingleAllocateBenchmark
//{
//	[Benchmark]
//	public unsafe void AllocateJeMallocSmall()
//	{
//		nint ptr = JeMallocAllocator.Allocate(Random.Shared.Next(1, 128));
//		JeMallocAllocator.Free((nint)ptr);
//	}

//	[Benchmark]
//	public unsafe void AllocateJeMallocMedium()
//	{
//		nint ptr = JeMallocAllocator.Allocate(Random.Shared.Next(1024, 4096));
//		JeMallocAllocator.Free((nint)ptr);
//	}

//	[Benchmark]
//	public unsafe void AllocateJeMallocLarge()
//	{
//		nint ptr = JeMallocAllocator.Allocate(Random.Shared.Next(64 * 1024, 256 * 1024));
//		JeMallocAllocator.Free((nint)ptr);
//	}

//	[Benchmark]
//	public unsafe void AllocateJeMallocExtraLarge()
//	{
//		nint ptr = JeMallocAllocator.Allocate(Random.Shared.Next(64 * 1024 * 1024, 256 * 1024 * 1024));
//		JeMallocAllocator.Free((nint)ptr);
//	}

//	[Benchmark]
//	public void AllocateMarshallSmall()
//	{
//		nint ptr = Marshal.AllocHGlobal(Random.Shared.Next(1, 128));
//		Marshal.FreeHGlobal(ptr);
//	}

//	[Benchmark]
//	public void AllocateMarshallMedium()
//	{
//		nint ptr = Marshal.AllocHGlobal(Random.Shared.Next(1024, 4096));
//		Marshal.FreeHGlobal(ptr);
//	}

//	[Benchmark]
//	public void AllocateMarshallLarge()
//	{
//		nint ptr = Marshal.AllocHGlobal(Random.Shared.Next(64 * 1024, 256 * 1024));
//		Marshal.FreeHGlobal(ptr);
//	}

//	[Benchmark]
//	public void AllocateMarshallExtraLarge()
//	{
//		nint ptr = Marshal.AllocHGlobal(Random.Shared.Next(64 * 1024 * 1024, 256 * 1024 * 1024));
//		Marshal.FreeHGlobal(ptr);
//	}

//	[Benchmark]
//	public unsafe void AllocateNativeMemorySmall()
//	{
//		void* ptr = NativeMemory.Alloc((nuint)Random.Shared.Next(1, 128));
//		NativeMemory.Free(ptr);
//	}

//	[Benchmark]
//	public unsafe void AllocateNativeMemoryMedium()
//	{
//		void* ptr = NativeMemory.Alloc((nuint)Random.Shared.Next(1024, 4096));
//		NativeMemory.Free(ptr);
//	}

//	[Benchmark]
//	public unsafe void AllocateNativeMemoryLarge()
//	{
//		void* ptr = NativeMemory.Alloc((nuint)Random.Shared.Next(64 * 1024, 256 * 1024));
//		NativeMemory.Free(ptr);
//	}

//	[Benchmark]
//	public unsafe void AllocateNativeMemoryExtraLarge()
//	{
//		void* ptr = NativeMemory.Alloc((nuint)Random.Shared.Next(64 * 1024 * 1024, 256 * 1024 * 1024));
//		NativeMemory.Free(ptr);
//	}
//}

//public class AllocatorMultipleAllocateBenchmark
//{
//	private const int ConcurrencyLevel = 12; // Define the number of threads

//	// NOTE: This approach measures the overhead of thread spawning,
//	// execution, and joining, which is usually what you want when
//	// testing for concurrent performance.

//	//[Benchmark]
//	//public void AllocateMarshallSmall()
//	//{
//	//	Parallel.For(0, ConcurrencyLevel, _ =>
//	//	{
//	//		unsafe
//	//		{
//	//			nint* ptrs = (nint*)Marshal.AllocHGlobal(sizeof(nint) * 256 * 1024 * 1024);
//	//			ulong cursor = 0;

//	//			for (long i = 0; i < 256 * 1024 * 1024; i++)
//	//			{
//	//				if (cursor > 0 && Random.Shared.Next(0, 5) > 2)
//	//				{
//	//					Marshal.FreeHGlobal(ptrs[--cursor]);
//	//				}
//	//				else
//	//				{
//	//					ptrs[cursor++] = Marshal.AllocHGlobal(Random.Shared.Next(1, 128));
//	//				}
//	//			}

//	//			while (cursor > 0)
//	//				Marshal.FreeHGlobal(ptrs[--cursor]);

//	//			Marshal.FreeHGlobal((nint)ptrs);
//	//		}
//	//	});
//	//}

//	[Benchmark]
//	public unsafe void AllocateJeMallocSmall()
//	{
//		Parallel.For(0, ConcurrencyLevel, _ =>
//		{
//			unsafe
//			{
//				nint* ptrs = (nint*)JeMallocAllocator.Allocate(sizeof(nint) * 16 * 1024 * 1024);
//				ulong cursor = 0;

//				for (long i = 0; i < 16 * 1024 * 1024; i++)
//				{
//					if (cursor > 0 && Random.Shared.Next(0, 5) > 2)
//					{
//						JeMallocAllocator.Free(ptrs[--cursor]);
//					}
//					else
//					{
//						ptrs[cursor++] = JeMallocAllocator.Allocate(Random.Shared.Next(1, 128));
//					}
//				}

//				while (cursor > 0)
//					JeMallocAllocator.Free(ptrs[--cursor]);

//				JeMallocAllocator.Free((nint)ptrs);
//			}
//		});
//	}

//	[Benchmark]
//	public unsafe void AllocateJeMallocFallbackSmall()
//	{
//		Parallel.For(0, ConcurrencyLevel, _ =>
//		{
//			unsafe
//			{
//				nint* ptrs = (nint*)JeMallocAllocator<NativeAllocator>.Allocate(sizeof(nint) * 16 * 1024 * 1024);
//				ulong cursor = 0;

//				for (long i = 0; i < 16 * 1024 * 1024; i++)
//				{
//					if (cursor > 0 && Random.Shared.Next(0, 5) > 2)
//					{
//						JeMallocAllocator<NativeAllocator>.Free(ptrs[--cursor]);
//					}
//					else
//					{
//						ptrs[cursor++] = JeMallocAllocator<NativeAllocator>.Allocate(Random.Shared.Next(1, 128));
//					}
//				}

//				while (cursor > 0)
//					JeMallocAllocator<NativeAllocator>.Free(ptrs[--cursor]);

//				JeMallocAllocator<NativeAllocator>.Free((nint)ptrs);
//			}
//		});
//	}

//	[Benchmark]
//	public void AllocateNativeMemorySmall()
//	{
//		Parallel.For(0, ConcurrencyLevel, _ =>
//		{
//			unsafe
//			{
//				nint* ptrs = (nint*)NativeMemory.Alloc((nuint)sizeof(nint) * 16 * 1024 * 1024);
//				ulong cursor = 0;

//				for (long i = 0; i < 16 * 1024 * 1024; i++)
//				{
//					if (cursor > 0 && Random.Shared.Next(0, 11) > 5)
//					{
//						NativeMemory.Free((void*)ptrs[--cursor]);
//					}
//					else
//					{
//						ptrs[cursor++] = (nint)NativeMemory.Alloc((nuint)Random.Shared.Next(1, 128));
//					}
//				}

//				while (cursor > 0)
//					NativeMemory.Free((void*)ptrs[--cursor]);

//				NativeMemory.Free(ptrs);
//			}
//		});
//	}

//	[Benchmark]
//	public void AllocateGCSmall()
//	{
//		Parallel.For(0, ConcurrencyLevel, _ =>
//		{
//			byte[]?[] ptrs = new byte[16 * 1024 * 1024][];
//			ulong cursor = 0;

//			for (long i = 0; i < 16 * 1024 * 1024; i++)
//			{
//				if (cursor > 0 && Random.Shared.Next(0, 5) > 2)
//				{
//					ptrs[--cursor] = null;
//				}
//				else
//				{
//					ptrs[cursor++] = GC.AllocateArray<byte>(Random.Shared.Next(1, 128));
//				}
//			}

//			while (cursor > 0)
//				ptrs[--cursor] = null;

//			GC.Collect(GC.MaxGeneration, GCCollectionMode.Aggressive, true, true);
//		});
//	}



//	[Benchmark]
//	public void AllocateMarshallMedium()
//	{
//		Parallel.For(0, ConcurrencyLevel, _ =>
//		{
//			unsafe
//			{
//				nint* ptrs = stackalloc nint[64 * 1024];
//				short cursor = 0;

//				for (long i = 0; i < 64 * 1024; i++)
//				{
//					if (cursor > 0 && Random.Shared.Next(0, 5) > 2)
//					{
//						Marshal.FreeHGlobal(ptrs[--cursor]);
//					}
//					else
//					{
//						ptrs[cursor++] = Marshal.AllocHGlobal(Random.Shared.Next(1024, 4096));
//					}
//				}

//				while (cursor > 0)
//					Marshal.FreeHGlobal(ptrs[--cursor]);
//			}
//		});
//	}

//	[Benchmark]
//	public void AllocateNativeMemoryMedium()
//	{
//		Parallel.For(0, ConcurrencyLevel, _ =>
//		{
//			unsafe
//			{
//				nint* ptrs = stackalloc nint[64 * 1024];
//				short cursor = 0;

//				for (long i = 0; i < 64 * 1024; i++)
//				{
//					if (cursor > 0 && Random.Shared.Next(0, 5) > 2)
//					{
//						NativeMemory.Free((void*)ptrs[--cursor]);
//					}
//					else
//					{
//						ptrs[cursor++] = (nint)NativeMemory.Alloc((nuint)Random.Shared.Next(1024, 4096));
//					}
//				}

//				while (cursor > 0)
//					NativeMemory.Free((void*)ptrs[--cursor]);
//			}
//		});
//	}

//	[Benchmark]
//	public void AllocateGCMedium()
//	{
//		Parallel.For(0, ConcurrencyLevel, _ =>
//		{
//			byte[]?[] ptrs = new byte[64 * 1024][];
//			short cursor = 0;

//			for (long i = 0; i < 64 * 1024; i++)
//			{
//				if (cursor > 0 && Random.Shared.Next(0, 5) > 2)
//				{
//					ptrs[--cursor] = null;
//				}
//				else
//				{
//					ptrs[cursor++] = GC.AllocateArray<byte>(Random.Shared.Next(1024, 4096));
//				}
//			}

//			while (cursor > 0)
//				ptrs[--cursor] = null;

//			GC.Collect(GC.MaxGeneration, GCCollectionMode.Aggressive, true, true);
//		});
//	}

//	[Benchmark]
//	public unsafe void AllocateJeMallocMedium()
//	{
//		Parallel.For(0, ConcurrencyLevel, _ =>
//		{
//			unsafe
//			{
//				nint* ptrs = stackalloc nint[64 * 1024];
//				short cursor = 0;

//				for (long i = 0; i < 64 * 1024; i++)
//				{
//					if (cursor > 0 && Random.Shared.Next(0, 5) > 2)
//					{
//						JeMallocAllocator.Free(ptrs[--cursor]);
//					}
//					else
//					{
//						ptrs[cursor++] = (nint)JeMallocAllocator.Allocate(Random.Shared.Next(1024, 4096));
//					}
//				}

//				while (cursor > 0)
//					JeMallocAllocator.Free(ptrs[--cursor]);
//			}
//		});
//	}
//}
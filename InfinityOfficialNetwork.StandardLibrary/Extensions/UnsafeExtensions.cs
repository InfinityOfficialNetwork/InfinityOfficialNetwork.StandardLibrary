using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace InfinityOfficialNetwork.StandardLibrary.Extensions;
public static class UnsafeExtensions
{
	extension (Unsafe)
	{
		public static unsafe TArg* AsPtr<TArg>(scoped ref TArg arg)
		{
			return (TArg*)Unsafe.AsPointer(ref arg);
		}

		public static unsafe ref TArg AsRef<TArg>(TArg* arg)
		{
			return ref Unsafe.AsRef<TArg>((void*)arg);
		}

		public static void InitBlock(ref byte startAddress, byte value, ulong byteCount)
		{
			ref byte ptr = ref startAddress;

			while (byteCount > uint.MaxValue)
			{
				Unsafe.InitBlock(ref ptr, value, uint.MaxValue);

				ptr = ref Unsafe.Add(ref ptr, uint.MaxValue);
				byteCount -= uint.MaxValue;
			}

			if (byteCount > 0)
				Unsafe.InitBlock(ref ptr, value, (uint)byteCount);
		}

		public static void CopyBlock(ref byte destination, ref readonly byte source, ulong byteCount)
		{
			ref byte src = ref Unsafe.AsRef(in source);
			ref byte dst = ref destination;

			while (byteCount > uint.MaxValue)
			{
				Unsafe.CopyBlock(ref dst, ref src, uint.MaxValue);

				dst = ref Unsafe.Add(ref dst, uint.MaxValue);
				src = ref Unsafe.Add(ref src, uint.MaxValue);
				
				byteCount -= uint.MaxValue;
			}

			if (byteCount > 0)
				Unsafe.CopyBlock(ref dst, ref src, (uint)byteCount);
		}
	}
}

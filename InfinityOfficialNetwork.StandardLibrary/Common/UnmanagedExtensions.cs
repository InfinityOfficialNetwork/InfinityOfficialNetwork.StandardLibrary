using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Text;

namespace InfinityOfficialNetwork.StandardLibrary.Common;
public static class UnmanagedExtensions
{
	/// <summary>
	/// Sets all bytes of the specified unmanaged object to zero. Undefined behavior will occur if the object contains managed resources.
	/// <br/>
	/// This should only be used by allocators to set the zero-able validity field to mark it as uninitialized.
	/// </summary>
	/// <typeparam name="TArg">The type of the unmanaged object. Must implement <see cref="IHandle{T}"/>.</typeparam>
	/// <param name="managedObject">A reference to the unmanaged object to be zero-initialized.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static void ZeroInitialize<TArg>(this ref TArg managedObject) where TArg : unmanaged
	{
		Unsafe.InitBlock(Unsafe.AsPointer(ref managedObject), 0, (uint)sizeof(TArg));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe bool IsZeroed<TArg>(this ref TArg source) where TArg : unmanaged
	{
		uint size = (uint)Unsafe.SizeOf<TArg>();
		ref byte current = ref Unsafe.As<TArg, byte>(ref source);

		if (Vector256.IsHardwareAccelerated && size >= 32)
		{
			ref Vector256<byte> currentVector = ref Unsafe.As<byte, Vector256<byte>>(ref current);
			Vector256<byte> zeroVector = Vector256<byte>.Zero;

			while (size >= 32)
			{
				if (currentVector != zeroVector)
					return false;

				currentVector = ref Unsafe.Add(ref currentVector, 1);
				size -= 32;
			}

			current = ref Unsafe.As<Vector256<byte>, byte>(ref currentVector);
		}

		if (size >= 8)
		{
			ref long currentLong = ref Unsafe.As<byte, long>(ref current);

			while (size >= 8)
			{
				if (currentLong != 0)
					return false;

				currentLong = ref Unsafe.Add(ref currentLong, 1);
				size -= 8;
			}

			current = ref Unsafe.As<long, byte>(ref currentLong);
		}

		while (size > 0)
		{
			if (current != 0)
				return false;

			current = ref Unsafe.Add(ref current, 1);
			size--;
		}

		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe void ByteSwap<TArg>(this ref TArg first, ref TArg second) where TArg : unmanaged
	{
		uint size = (uint)Unsafe.SizeOf<TArg>();
		TArg* intermediary = stackalloc TArg[1];

		Unsafe.CopyBlock(ref Unsafe.As<TArg, byte>(ref *intermediary),
			ref Unsafe.As<TArg, byte>(ref first),
			size);

		Unsafe.CopyBlock(ref Unsafe.As<TArg, byte>(ref first),
			ref Unsafe.As<TArg, byte>(ref second),
			size);

		Unsafe.CopyBlock(ref Unsafe.As<TArg, byte>(ref second),
			ref Unsafe.As<TArg, byte>(ref *intermediary),
			size);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe void ByteMove<TArg>(this ref TArg first, ref TArg second) where TArg : unmanaged
	{
		uint size = (uint)Unsafe.SizeOf<TArg>();

		Unsafe.CopyBlock(ref Unsafe.As<TArg, byte>(ref second),
			ref Unsafe.As<TArg, byte>(ref first),
			size);

		Unsafe.InitBlock(ref Unsafe.As<TArg, byte>(ref first), 0, size);
	}
}

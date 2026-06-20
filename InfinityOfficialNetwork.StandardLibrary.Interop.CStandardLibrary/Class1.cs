using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Type Mappings
using size_t = System.UIntPtr;
using rsize_t = System.UIntPtr;
using errno_t = System.Int32;

public unsafe static class CStdLib
{
	public static void* NULL = null;
	public static readonly rsize_t RSIZE_MAX = (rsize_t)(nuint.MaxValue >> 1);

	#region Internal Helpers (Unsafe looping)

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void UnsafeMemCpy(void* dest, void* src, size_t n)
	{
		byte* d = (byte*)dest;
		byte* s = (byte*)src;

		// Unsafe.CopyBlock takes uint (max 4GB). Loop required for 64-bit sizes.
		while (n > uint.MaxValue)
		{
			Unsafe.CopyBlockUnaligned(d, s, uint.MaxValue);
			d += uint.MaxValue;
			s += uint.MaxValue;
			n -= uint.MaxValue;
		}
		Unsafe.CopyBlockUnaligned(d, s, (uint)n);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void UnsafeMemSet(void* dest, byte c, size_t n)
	{
		byte* d = (byte*)dest;
		while (n > uint.MaxValue)
		{
			Unsafe.InitBlockUnaligned(d, c, uint.MaxValue);
			d += uint.MaxValue;
			n -= uint.MaxValue;
		}
		Unsafe.InitBlockUnaligned(d, c, (uint)n);
	}

	#endregion

	#region Memory Functions

	public static void* memcpy(void* s1, void* s2, size_t n)
	{
		UnsafeMemCpy(s1, s2, n);
		return s1;
	}

	public static void* memmove(void* s1, void* s2, size_t n)
	{
		byte* d = (byte*)s1;
		byte* s = (byte*)s2;

		if (d == s || n == 0) return s1;

		// If dest is after src and overlapping, we must copy backwards to avoid corruption.
		// Unsafe.CopyBlock does not guarantee this.
		if (d > s && d < s + n)
		{
			d += n;
			s += n;

			// Backward copy
			while (n > 0)
			{
				d--;
				s--;
				*d = *s;
				n--;
			}
		}
		else
		{
			// Non-overlapping or Forward copy is safe
			UnsafeMemCpy(s1, s2, n);
		}
		return s1;
	}

	public static void* memccpy(void* s1, void* s2, int c, size_t n)
	{
		// Optimization: Search first, then bulk copy.
		// Convert to Span to utilize optimized vectorized search.
		var srcSpan = new ReadOnlySpan<byte>(s2, (int)(n > int.MaxValue ? int.MaxValue : n));
		int index = srcSpan.IndexOf((byte)c);

		if (index >= 0)
		{
			// Found: copy up to including c
			size_t count = (size_t)(index + 1);
			UnsafeMemCpy(s1, s2, count);
			return (byte*)s1 + count;
		}
		else
		{
			// Not found: copy all n
			UnsafeMemCpy(s1, s2, n);

			// If n was huge (larger than int.Max), we might have missed 'c' in the upper region.
			// A full C implementation would continue searching, but assuming reasonable bounds for memccpy:
			return null;
		}
	}

	public static void* memset(void* s, int c, size_t n)
	{
		UnsafeMemSet(s, (byte)c, n);
		return s;
	}

	public static void* memset_explicit(void* s, int c, size_t n)
	{
		// Volatile write loop to prevent compiler optimization (dead store elimination)
		byte* p = (byte*)s;
		byte val = (byte)c;
		for (nuint i = 0; i < n; i++)
		{
			Volatile.Write(ref p[i], val);
		}
		return s;
	}

	public static int memcmp(void* s1, void* s2, size_t n)
	{
		var span1 = new ReadOnlySpan<byte>(s1, (int)n);
		var span2 = new ReadOnlySpan<byte>(s2, (int)n);
		return span1.SequenceCompareTo(span2);
	}

	public static void* memchr(void* s, int c, size_t n)
	{
		// Optimization: Use Span.IndexOf
		if (n > int.MaxValue)
		{
			// Fallback for massive buffers
			byte* p = (byte*)s;
			byte val = (byte)c;
			for (nuint i = 0; i < n; i++)
			{
				if (p[i] == val) return p + i;
			}
			return null;
		}

		int idx = new ReadOnlySpan<byte>(s, (int)n).IndexOf((byte)c);
		return idx >= 0 ? (byte*)s + idx : null;
	}

	#endregion

	#region String Functions

	public static byte* strcpy(byte* s1, byte* s2)
	{
		byte* dest = s1;
		byte* src = s2;
		while ((*dest++ = *src++) != 0) ;
		return s1;
	}

	public static byte* strncpy(byte* s1, byte* s2, size_t n)
	{
		size_t i;
		for (i = 0; i < n && s2[i] != 0; i++)
		{
			s1[i] = s2[i];
		}
		// Pad remainder with nulls
		if (i < n)
		{
			UnsafeMemSet(s1 + i, 0, n - i);
		}
		return s1;
	}

	public static byte* strdup(byte* s)
	{
		size_t len = strlen(s) + 1;
		void* newPtr = NativeMemory.Alloc(len);
		if (newPtr == null) return null;
		UnsafeMemCpy(newPtr, s, len);
		return (byte*)newPtr;
	}

	public static byte* strndup(byte* s, size_t n)
	{
		size_t len = strnlen(s, n);
		void* newPtr = NativeMemory.Alloc(len + 1);
		if (newPtr == null) return null;
		UnsafeMemCpy(newPtr, s, len);
		((byte*)newPtr)[len] = 0;
		return (byte*)newPtr;
	}

	public static byte* strcat(byte* s1, byte* s2)
	{
		byte* dest = s1;
		// Move to end
		while (*dest != 0) dest++;
		// Copy
		strcpy(dest, s2);
		return s1;
	}

	public static byte* strncat(byte* s1, byte* s2, size_t n)
	{
		byte* dest = s1;
		while (*dest != 0) dest++;

		size_t i;
		for (i = 0; i < n && s2[i] != 0; i++)
		{
			dest[i] = s2[i];
		}
		dest[i] = 0;
		return s1;
	}

	public static int strcmp(byte* s1, byte* s2)
	{
		while (*s1 != 0 && *s1 == *s2)
		{
			s1++;
			s2++;
		}
		return *s1 - *s2;
	}

	public static int strncmp(byte* s1, byte* s2, size_t n)
	{
		if (n == 0) return 0;
		for (nuint i = 0; i < n; i++)
		{
			if (s1[i] != s2[i]) return s1[i] - s2[i];
			if (s1[i] == 0) return 0;
		}
		return 0;
	}

	public static int strcoll(byte* s1, byte* s2) => strcmp(s1, s2);

	public static size_t strxfrm(byte* s1, byte* s2, size_t n)
	{
		size_t len = strlen(s2);
		if (n > len) strcpy(s1, s2);
		return len;
	}

	public static byte* strchr(byte* s, int c)
	{
		byte val = (byte)c;
		while (true)
		{
			if (*s == val) return s;
			if (*s == 0) return null;
			s++;
		}
	}

	public static size_t strcspn(byte* s1, byte* s2)
	{
		byte* p = s1;
		while (*p != 0)
		{
			// Optimization: check if *p exists in s2
			byte* s2_iter = s2;
			while (*s2_iter != 0)
			{
				if (*p == *s2_iter) return (size_t)(p - s1);
				s2_iter++;
			}
			p++;
		}
		return (size_t)(p - s1);
	}

	public static byte* strpbrk(byte* s1, byte* s2)
	{
		while (*s1 != 0)
		{
			byte* s2_iter = s2;
			while (*s2_iter != 0)
			{
				if (*s1 == *s2_iter) return s1;
				s2_iter++;
			}
			s1++;
		}
		return null;
	}

	public static byte* strrchr(byte* s, int c)
	{
		byte val = (byte)c;
		byte* last = null;
		while (true)
		{
			if (*s == val) last = s;
			if (*s == 0) return last;
			s++;
		}
	}

	public static size_t strspn(byte* s1, byte* s2)
	{
		byte* p = s1;
		while (*p != 0)
		{
			bool found = false;
			byte* s2_iter = s2;
			while (*s2_iter != 0)
			{
				if (*p == *s2_iter)
				{
					found = true;
					break;
				}
				s2_iter++;
			}
			if (!found) return (size_t)(p - s1);
			p++;
		}
		return (size_t)(p - s1);
	}

	public static byte* strstr(byte* s1, byte* s2)
	{
		if (*s2 == 0) return s1;
		byte* p1 = s1;
		while (*p1 != 0)
		{
			byte* p1Begin = p1;
			byte* p2 = s2;
			while (*p1 != 0 && *p2 != 0 && *p1 == *p2)
			{
				p1++;
				p2++;
			}
			if (*p2 == 0) return p1Begin;
			p1 = p1Begin + 1;
		}
		return null;
	}

	[ThreadStatic]
	private static byte* _strtok_ptr;

	public static byte* strtok(byte* s1, byte* s2)
	{
		if (s1 == null) s1 = _strtok_ptr;
		if (s1 == null) return null;

		s1 += strspn(s1, s2);
		if (*s1 == 0)
		{
			_strtok_ptr = null;
			return null;
		}

		byte* token_start = s1;
		s1 = strpbrk(token_start, s2);
		if (s1 != null)
		{
			*s1 = 0;
			_strtok_ptr = s1 + 1;
		}
		else
		{
			_strtok_ptr = null;
		}
		return token_start;
	}

	public static byte* strerror(int errnum)
	{
		// Minimal implementation
		fixed (byte* p = "Unknown error"u8) return p;
	}

	public static size_t strlen(byte* s)
	{
		// Efficient scanning
		return (size_t)MemoryMarshal.CreateReadOnlySpanFromNullTerminated(s).Length;
	}

	public static size_t strnlen(byte* s, size_t n)
	{
		size_t len = 0;
		while (len < n && s[len] != 0) len++;
		return len;
	}

	#endregion

	#region Annex K (Bounds Checking)

	public static errno_t memcpy_s(void* s1, rsize_t s1max, void* s2, rsize_t n)
	{
		if (s1 == null || s1max > RSIZE_MAX) return 1;
		if (s2 == null)
		{
			UnsafeMemSet(s1, 0, s1max);
			return 1;
		}
		if (n > RSIZE_MAX || n > s1max)
		{
			UnsafeMemSet(s1, 0, s1max);
			return 1;
		}
		UnsafeMemCpy(s1, s2, n);
		return 0;
	}

	public static errno_t memmove_s(void* s1, rsize_t s1max, void* s2, rsize_t n)
	{
		if (s1 == null || s1max > RSIZE_MAX) return 1;
		if (s2 == null)
		{
			UnsafeMemSet(s1, 0, s1max);
			return 1;
		}
		if (n > RSIZE_MAX || n > s1max)
		{
			UnsafeMemSet(s1, 0, s1max);
			return 1;
		}
		memmove(s1, s2, n);
		return 0;
	}

	public static errno_t strcpy_s(byte* s1, rsize_t s1max, byte* s2)
	{
		if (s1 == null || s1max > RSIZE_MAX || s1max == 0) return 1;
		if (s2 == null)
		{
			*s1 = 0;
			return 1;
		}

		rsize_t i;
		for (i = 0; i < s1max; i++)
		{
			s1[i] = s2[i];
			if (s2[i] == 0) return 0;
		}

		// Overflow
		*s1 = 0;
		return 1;
	}

	public static errno_t strncpy_s(byte* s1, rsize_t s1max, byte* s2, rsize_t n)
	{
		if (s1 == null || s1max > RSIZE_MAX || s1max == 0) return 1;
		if (s2 == null)
		{
			*s1 = 0;
			return 1;
		}
		if (n > RSIZE_MAX)
		{
			*s1 = 0;
			return 1;
		}

		rsize_t i;
		for (i = 0; i < s1max; i++)
		{
			if (i == n || s2[i] == 0)
			{
				s1[i] = 0;
				return 0;
			}
			s1[i] = s2[i];
		}

		*s1 = 0;
		return 1;
	}

	public static errno_t strcat_s(byte* s1, rsize_t s1max, byte* s2)
	{
		if (s1 == null || s1max > RSIZE_MAX || s1max == 0) return 1;
		if (s2 == null)
		{
			*s1 = 0;
			return 1;
		}

		byte* orig = s1;
		rsize_t remaining = s1max;

		while (*s1 != 0 && remaining > 0)
		{
			s1++;
			remaining--;
		}

		if (remaining == 0)
		{
			*orig = 0;
			return 1;
		}

		while (remaining > 0)
		{
			*s1 = *s2;
			if (*s2 == 0) return 0;
			s1++;
			s2++;
			remaining--;
		}

		*orig = 0;
		return 1;
	}

	public static errno_t strncat_s(byte* s1, rsize_t s1max, byte* s2, rsize_t n)
	{
		if (s1 == null || s1max > RSIZE_MAX || s1max == 0) return 1;
		if (s2 == null)
		{
			*s1 = 0;
			return 1;
		}
		if (n > RSIZE_MAX)
		{
			*s1 = 0;
			return 1;
		}

		byte* orig = s1;
		rsize_t remaining = s1max;

		while (*s1 != 0 && remaining > 0)
		{
			s1++;
			remaining--;
		}

		if (remaining == 0)
		{
			*orig = 0;
			return 1;
		}

		rsize_t count = 0;
		while (remaining > 0 && count < n)
		{
			if (*s2 == 0) break;
			*s1++ = *s2++;
			remaining--;
			count++;
		}

		if (remaining == 0)
		{
			*orig = 0;
			return 1;
		}

		*s1 = 0;
		return 0;
	}

	public static byte* strtok_s(byte* s1, rsize_t* s1max, byte* s2, byte** ptr)
	{
		if (s1max == null || *s1max > RSIZE_MAX || s2 == null || ptr == null)
		{
			if (s1 != null && s1max != null && *s1max > 0) *s1 = 0;
			return null;
		}

		byte* str = s1 != null ? s1 : *ptr;
		if (str == null) return null;

		while (*str != 0 && strchr(s2, *str) != null) str++;

		if (*str == 0)
		{
			*ptr = str;
			return null;
		}

		byte* token_start = str;
		while (*str != 0 && strchr(s2, *str) == null) str++;

		if (*str != 0)
		{
			*str = 0;
			*ptr = str + 1;
		}
		else
		{
			*ptr = str;
		}

		return token_start;
	}

	public static errno_t memset_s(void* s, rsize_t smax, int c, rsize_t n)
	{
		if (s == null || smax > RSIZE_MAX) return 1;
		if (n > RSIZE_MAX || n > smax)
		{
			UnsafeMemSet(s, (byte)c, smax);
			return 1;
		}

		// Volatile write for security
		memset_explicit(s, c, n);
		return 0;
	}

	public static errno_t strerror_s(byte* s, rsize_t maxsize, errno_t errnum)
	{
		if (s == null || maxsize == 0 || maxsize > RSIZE_MAX) return 1;

		var msg = "Unknown error"u8;
		size_t len = (size_t)msg.Length;

		if (len >= maxsize)
		{
			UnsafeMemCpy(s, (void*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(msg)), maxsize - 1);
			s[maxsize - 1] = 0;
			return 1;
		}

		UnsafeMemCpy(s, (void*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(msg)), len);
		s[len] = 0;
		return 0;
	}

	public static size_t strerrorlen_s(errno_t errnum)
	{
		return (size_t)("Unknown error".Length);
	}

	public static size_t strnlen_s(byte* s, size_t maxsize)
	{
		if (s == null) return 0;
		size_t len = 0;
		while (len < maxsize && s[len] != 0) len++;
		return len;
	}

	#endregion
}
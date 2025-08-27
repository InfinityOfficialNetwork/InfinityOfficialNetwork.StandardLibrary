using InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators;
using InfinityOfficialNetwork.StandardLibrary.MemoryManagement.Memory;
using System.Runtime.CompilerServices;

namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Containers.Primary;

public struct VectorDefaultParameters : IVectorParameters
{
	public static int InitialReserve => 4;

	public static int ReserveMultiplierNumerator => 2;

	public static int ReserveMultiplierDenominator => 1;
}

public unsafe sealed class Vector<TArg, TAllocator> : Vector<TArg, TAllocator, VectorDefaultParameters> where TArg : unmanaged where TAllocator : IMemoryAllocator, new()
{
	public Vector() { }
	public Vector(TArg value) : base(value) { }
	public Vector(int count, TArg value) : base(count, value) { }
	public Vector(int count, Func<TArg> factory) : base(count, factory) { }
	public Vector(int count, Func<int, TArg> factory) : base(count, factory) { }
	public Vector(IInputIterator<TArg> first, IInputIterator<TArg> last) : base(first, last) { }
	public Vector(IStreamingInputIterator<TArg> first, IStreamingIterator<TArg> last) : base(first, last) { }
	public Vector(IForwardIterator<TArg> first, IForwardIterator<TArg> last) : base(first, last) { }
	public Vector(IBidirectionalIterator<TArg> first, IBidirectionalIterator<TArg> last) : base(first, last) { }
	public Vector(IRandomAccessIterator<TArg> first, IRandomAccessIterator<TArg> last) : base(first, last) { }
	public Vector(IContiguousIterator<TArg> first, IContiguousIterator<TArg> last) : base(first, last) { }
	public Vector(IInputIteratable<TArg> items) : base(items) { }
	public Vector(IStreamingInputIteratable<TArg> items) : base(items) { }
	public Vector(IForwardIteratable<TArg> items) : base(items) { }
	public Vector(IBidirectionalIteratable<TArg> items) : base(items) { }
	public Vector(IRandomAccessIteratable<TArg> items) : base(items) { }
	public Vector(IContiguousIteratable<TArg> items) : base(items) { }
}

public unsafe sealed class Vector<TArg> : Vector<TArg, NativeMemoryAllocator, VectorDefaultParameters> where TArg : unmanaged
{
	public Vector() { }
	public Vector(TArg value) : base(value) { }
	public Vector(int count, TArg value) : base(count, value) { }
	public Vector(int count, Func<TArg> factory) : base(count, factory) { }
	public Vector(int count, Func<int, TArg> factory) : base(count, factory) { }
	public Vector(IInputIterator<TArg> first, IInputIterator<TArg> last) : base(first, last) { }
	public Vector(IStreamingInputIterator<TArg> first, IStreamingIterator<TArg> last) : base(first, last) { }
	public Vector(IForwardIterator<TArg> first, IForwardIterator<TArg> last) : base(first, last) { }
	public Vector(IBidirectionalIterator<TArg> first, IBidirectionalIterator<TArg> last) : base(first, last) { }
	public Vector(IRandomAccessIterator<TArg> first, IRandomAccessIterator<TArg> last) : base(first, last) { }
	public Vector(IContiguousIterator<TArg> first, IContiguousIterator<TArg> last) : base(first, last) { }
	public Vector(IInputIteratable<TArg> items) : base(items) { }
	public Vector(IStreamingInputIteratable<TArg> items) : base(items) { }
	public Vector(IForwardIteratable<TArg> items) : base(items) { }
	public Vector(IBidirectionalIteratable<TArg> items) : base(items) { }
	public Vector(IRandomAccessIteratable<TArg> items) : base(items) { }
	public Vector(IContiguousIteratable<TArg> items) : base(items) { }
}

public unsafe class Vector<TArg, TAllocator, TVectorParameters> : IVector<TArg> where TArg : unmanaged where TAllocator : IMemoryAllocator, new() where TVectorParameters : IVectorParameters
{
	private static int initialReserve = TVectorParameters.InitialReserve;
	private static int reserveMultiplierNumerator = TVectorParameters.ReserveMultiplierNumerator;
	private static int reserveMultiplierDenominator = TVectorParameters.ReserveMultiplierDenominator;

	private TArg* data;
	private int size, reserve;
	static private TAllocator allocator = new TAllocator();

	public Vector()
	{
		reserve = initialReserve;
		data = allocator.AllocateObject<TArg>(reserve);
		size = 0;
	}

	public Vector(TArg value)
	{
		reserve = initialReserve;
		data = allocator.AllocateObject<TArg>(reserve);
		size = 1;
		data[0] = value;
	}


	public Vector(int count, TArg value)
	{
		reserve = FindMinimumReserve(count);
		data = allocator.AllocateObject<TArg>(reserve);
		for (int i = 0; i < count; ++i)
			data[size++] = value;
	}

	public Vector(int count, Func<TArg> factory)
	{
		reserve = FindMinimumReserve(count);
		data = allocator.AllocateObject<TArg>(reserve);
		for (int i = 0; i < count; ++i)
			data[size++] = factory();
	}

	public Vector(int count, Func<int, TArg> factory)
	{
		reserve = FindMinimumReserve(count);
		data = allocator.AllocateObject<TArg>(reserve);
		for (int i = 0; i < count; ++i)
			data[size++] = factory(i);
	}

	public Vector(IInputIterator<TArg> first, IInputIterator<TArg> last)
	{
		TempStoragePage* firstPage = stackalloc TempStoragePage[1];
		TempStoragePage* currentPage = firstPage;

		int pageCount = 0, pageIndex = 0;
		for (IInputIterator<TArg> i = first, e = last; !i.Equals(e); i.MoveNext())
		{
			if (pageIndex == TempStoragePage.PAGE_SIZE)
			{
				TempStoragePage* newPage;

				if (pageCount < 16)
				{
					TempStoragePage* page = stackalloc TempStoragePage[1];
					newPage = page;
					newPage->next = null;
				}
				else
				{
					newPage = allocator.AllocateObject<TempStoragePage>();
					newPage->next = null;
				}

				currentPage->next = newPage;
				currentPage = newPage;
				pageIndex = 0;
				pageCount++;
			}
			currentPage->items[pageIndex++] = i.Current;
		}

		currentPage = firstPage;
		int count = (pageCount * TempStoragePage.PAGE_SIZE) + pageIndex;

		reserve = FindMinimumReserve(pageCount * TempStoragePage.PAGE_SIZE + pageIndex);
		data = allocator.AllocateObject<TArg>(reserve);

		pageIndex = 0;
		currentPage = firstPage;
		for (int i = 0; i < count; i++)
		{
			if (pageIndex == TempStoragePage.PAGE_SIZE)
			{
				currentPage = currentPage->next;
				pageIndex = 0;
			}
			data[size++] = currentPage->items[pageIndex++];
		}

		TempStoragePage* pageToFree = firstPage;
		for (int i = 0; i < 16 + 1; i++)
		{
			if (pageToFree == null) break;
			pageToFree = pageToFree->next;
		}

		while (pageToFree != null)
		{
			TempStoragePage* next = pageToFree->next;
			allocator.Free(pageToFree);
			pageToFree = next;
		}
	}
	public Vector(IStreamingInputIterator<TArg> first, IStreamingIterator<TArg> last) : this((IInputIterator<TArg>)first, (IInputIterator<TArg>)last) { }

	public Vector(IForwardIterator<TArg> first, IForwardIterator<TArg> last) : this((IInputIterator<TArg>)first, (IInputIterator<TArg>)last) { }
	public Vector(IBidirectionalIterator<TArg> first, IBidirectionalIterator<TArg> last) : this((IInputIterator<TArg>)first, (IInputIterator<TArg>)last) { }
	public Vector(IRandomAccessIterator<TArg> first, IRandomAccessIterator<TArg> last) : this((IInputIterator<TArg>)first, (IInputIterator<TArg>)last) { }
	public Vector(IContiguousIterator<TArg> first, IContiguousIterator<TArg> last) : this((IInputIterator<TArg>)first, (IInputIterator<TArg>)last) { }

	public Vector(IInputIteratable<TArg> items) : this(items.Begin, items.End) { }
	public Vector(IStreamingInputIteratable<TArg> items) : this(items.Begin, items.End) { }
	public Vector(IForwardIteratable<TArg> items) : this(items.Begin, items.End) { }
	public Vector(IBidirectionalIteratable<TArg> items) : this(items.Begin, items.End) { }
	public Vector(IRandomAccessIteratable<TArg> items) : this(items.Begin, items.End) { }
	public Vector(IContiguousIteratable<TArg> items) : this(items.Begin, items.End) { }

	public int MaxSize => int.MaxValue;

	public ref TArg Back => ref data[size - 1];
	public ref TArg Front => ref data[0];
	public unsafe TArg* Data => data;
	public bool IsEmpty => size == 0;
	public int Size => size;
	public int ReserveSize => reserve;

	public IContiguousIterator<TArg> Begin => new VectorIterator(data);
	public IContiguousIterator<TArg> End => new VectorIterator(data + size);
	//public IStreamingIterator<TArg> Begin => throw new NotImplementedException();
	//public IStreamingIterator<TArg> End => throw new NotImplementedException();

	public void AddBack(TArg value)
	{
		if (reserve >= size + 1)
		{
			data[size++] = value;
		}
		else
		{
			int newReserve = (reserve * reserveMultiplierNumerator) / reserveMultiplierDenominator;
			TArg* newData = allocator.AllocateObject<TArg>(newReserve);
			for (int i = 0; i < size; i++)
				newData[i] = data[i];
			reserve = newReserve;
			allocator.Free(data);
			data = newData;
			data[size++] = value;
		}
	}

	public void AddBack(int count, TArg value)
	{
		if (reserve >= size + count)
			for (int i = 0; i < count; i++)
				data[size++] = value;
		else
		{
			int newReserve = FindMinimumReserve(size + count);
			TArg* newData = allocator.AllocateObject<TArg>(newReserve);
			for (int i = 0; i < size; i++)
				newData[i] = data[i];
			reserve = newReserve;
			allocator.Free(data);
			data = newData;
			for (int i = 0; i < count; i++)
				data[size++] = value;
		}
	}

	public void AddBack(int count, Func<TArg> factory)
	{
		if (reserve >= size + count)
			for (int i = 0; i < count; i++)
				data[size++] = factory();
		else
		{
			int newReserve = FindMinimumReserve(size + count);
			TArg* newData = allocator.AllocateObject<TArg>(newReserve);
			for (int i = 0; i < size; i++)
				newData[i] = data[i];
			reserve = newReserve;
			allocator.Free(data);
			data = newData;
			for (int i = 0; i < count; i++)
				data[size++] = factory();
		}
	}

	public void AddBack(int count, Func<int, TArg> factory)
	{
		if (reserve >= size + count)
			for (int i = 0; i < count; i++)
				data[size++] = factory(i);
		else
		{
			int newReserve = FindMinimumReserve(size + count);
			TArg* newData = allocator.AllocateObject<TArg>(newReserve);
			for (int i = 0; i < size; i++)
				newData[i] = data[i];
			reserve = newReserve;
			allocator.Free(data);
			data = newData;
			for (int i = 0; i < count; i++)
				data[size++] = factory(i);
		}
	}

	public void AddBackRange(IInputIterator<TArg> first, IInputIterator<TArg> last)
	{
		TempStoragePage* firstPage = stackalloc TempStoragePage[1];
		TempStoragePage* currentPage = firstPage;

		int pageCount = 0, pageIndex = 0;
		for (IInputIterator<TArg> i = first, e = last; !i.Equals(e); i.MoveNext())
		{
			if (pageIndex == TempStoragePage.PAGE_SIZE)
			{
				TempStoragePage* newPage;

				if (pageCount < 16)
				{
					TempStoragePage* page = stackalloc TempStoragePage[1];
					newPage = page;
					newPage->next = null;
				}
				else
				{
					newPage = allocator.AllocateObject<TempStoragePage>();
					newPage->next = null;
				}

				currentPage->next = newPage;
				currentPage = newPage;
				pageIndex = 0;
				pageCount++;
			}
			currentPage->items[pageIndex++] = i.Current;
		}

		AddBackPages(firstPage, pageCount, pageIndex);

		TempStoragePage* pageToFree = firstPage;
		for (int i = 0; i < 16 + 1; i++)
		{
			if (pageToFree == null) break;
			pageToFree = pageToFree->next;
		}

		while (pageToFree != null)
		{
			TempStoragePage* next = pageToFree->next;
			allocator.Free(pageToFree);
			pageToFree = next;
		}

	}

	public void Assign(int count, TArg value)
	{
		if (reserve >= count)
		{
			size = 0;
			for (int i = 0; i < count; i++)
				data[size++] = value;
		}
		else
		{
			allocator.Free(data);
			reserve = FindMinimumReserve(count);
			size = 0;
			data = allocator.AllocateObject<TArg>(reserve);
			for (int i = 0; i < count; i++)
				data[size++] = value;
		}
	}

	public void Assign(int count, Func<int, TArg> factory)
	{
		allocator.Free(data);
		reserve = FindMinimumReserve(count);
		size = 0;
		data = allocator.AllocateObject<TArg>(reserve);
		for (int i = 0; i < count; i++)
			data[size++] = factory(i);
	}

	public void AssignRange(IInputIterator<TArg> first, IInputIterator<TArg> last)
	{
		TempStoragePage* firstPage = stackalloc TempStoragePage[1];
		TempStoragePage* currentPage = firstPage;

		int pageCount = 0, pageIndex = 0;
		for (IInputIterator<TArg> i = first, e = last; !i.Equals(e); i.MoveNext())
		{
			if (pageIndex == TempStoragePage.PAGE_SIZE)
			{
				TempStoragePage* newPage;

				if (pageCount < 16)
				{
					TempStoragePage* page = stackalloc TempStoragePage[1];
					newPage = page;
					newPage->next = null;
				}
				else
				{
					newPage = allocator.AllocateObject<TempStoragePage>();
					newPage->next = null;
				}

				currentPage->next = newPage;
				currentPage = newPage;
				pageIndex = 0;
				pageCount++;
			}
			currentPage->items[pageIndex++] = i.Current;
		}

		AssignPages(firstPage, pageCount, pageIndex);

		TempStoragePage* pageToFree = firstPage;
		for (int i = 0; i < 16 + 1; i++)
		{
			if (pageToFree == null) break;
			pageToFree = pageToFree->next;
		}

		while (pageToFree != null)
		{
			TempStoragePage* next = pageToFree->next;
			allocator.Free(pageToFree);
			pageToFree = next;
		}
	}

	public ref TArg At(int index) => ref data[index];

	~Vector()
	{
		Dispose();
	}

	public void Dispose()
	{
		if (data != null)
			allocator.Free(data);
		GC.SuppressFinalize(this);
	}

	public void RemoveBack()
	{
		if ((reserve * reserveMultiplierNumerator) / reserveMultiplierDenominator >= size && reserve > initialReserve)
		{
			int newReserve = reserve * reserveMultiplierDenominator / reserveMultiplierNumerator;
			TArg* newData = allocator.AllocateObject<TArg>(newReserve);
			for (int i = 0; i < size - 1; i++)
				newData[i] = data[i];
			allocator.Free(data);
			data = newData;
		}
		size--;
	}

	public void RemoveBack(int count)
	{
		if ((reserve * reserveMultiplierNumerator) / reserveMultiplierDenominator >= size - count && reserve > initialReserve)
		{
			int newReserve = reserve * reserveMultiplierDenominator / reserveMultiplierNumerator;
			TArg* newData = allocator.AllocateObject<TArg>(newReserve);
			for (int i = 0; i < size - count; i++)
				newData[i] = data[i];
			allocator.Free(data);
			data = newData;
		}
		size -= count;
	}

	public void Reserve(int size)
	{
		if (size > reserve)
		{
			reserve = size;
			TArg* newData = allocator.AllocateObject<TArg>(reserve);
			for (int i = 0; i < this.size; i++)
				newData[i] = data[i];
			allocator.Free(data);
			data = newData;
		}
	}

	public void Resize(int size)
	{
		if (size == this.size)
			return;
		else if (size > this.size)
		{
			reserve = size;
			TArg* newData = allocator.AllocateObject<TArg>(reserve);
			for (int i = 0; i < this.size; i++)
				newData[i] = data[i];
			allocator.Free(data);
			data = newData;
		}
		else
		{
			RemoveBack(this.size - size);
		}
	}

	public void Clear()
	{
		allocator.Free(data);
		reserve = initialReserve;
		size = 0;
		data = allocator.AllocateObject<TArg>(reserve);
	}

	public void ShrinkToFit()
	{
		if (reserve > size)
		{
			reserve = size;
			TArg* newData = allocator.AllocateObject<TArg>(reserve);
			for (int i = 0; i < this.size; i++)
				newData[i] = data[i];
			allocator.Free(data);
			data = newData;
		}
	}

	//

	private int FindMinimumReserve(int minReserve)
	{
		int res = initialReserve;
		while (res < minReserve)
			res = (res * reserveMultiplierNumerator) / reserveMultiplierDenominator;
		return res;
	}

	private struct TempStoragePage
	{
		public const int PAGE_SIZE = 64;

		[InlineArray(PAGE_SIZE)]
		public struct Item { public TArg Value; }

		public Item items;
		public TempStoragePage* next;
	}

	private void AddBackPages(TempStoragePage* firstPage, int pageCount, int pageIndex)
	{
		TempStoragePage* currentPage = firstPage;
		int iCount = (pageCount * TempStoragePage.PAGE_SIZE) + pageIndex;

		if (reserve >= size + iCount)
		{
			pageIndex = 0;
			currentPage = firstPage;
			for (int i = 0; i < iCount; i++)
			{
				if (pageIndex == TempStoragePage.PAGE_SIZE)
				{
					currentPage = currentPage->next;
					pageIndex = 0;
				}
				data[size++] = currentPage->items[pageIndex++];
			}
		}
		else
		{
			int newReserve = FindMinimumReserve(size + iCount);
			TArg* newData = allocator.AllocateObject<TArg>(newReserve);
			for (int i = 0; i < size; i++)
				newData[i] = data[i];
			reserve = newReserve;
			allocator.Free(data);
			data = newData;
			pageIndex = 0;
			currentPage = firstPage;
			for (int i = 0; i < iCount; i++)
			{
				if (pageIndex == TempStoragePage.PAGE_SIZE)
				{
					currentPage = currentPage->next;
					pageIndex = 0;
				}
				data[size++] = currentPage->items[pageIndex++];
			}
		}
	}

	private void AssignPages(TempStoragePage* firstPage, int pageCount, int pageIndex)
	{
		TempStoragePage* currentPage = firstPage;
		int count = (pageCount * TempStoragePage.PAGE_SIZE) + pageIndex;

		size = 0;
		allocator.Free(data);
		reserve = FindMinimumReserve(pageCount * TempStoragePage.PAGE_SIZE + pageIndex);
		data = allocator.AllocateObject<TArg>(reserve);

		pageIndex = 0;
		currentPage = firstPage;
		for (int i = 0; i < count; i++)
		{
			if (pageIndex == TempStoragePage.PAGE_SIZE)
			{
				currentPage = currentPage->next;
				pageIndex = 0;
			}
			data[size++] = currentPage->items[pageIndex++];
		}
	}

	public unsafe struct VectorIterator : IContiguousIterator<TArg>
	{
		TArg* ptr;

		public VectorIterator(TArg* ptr)
		{ this.ptr = ptr; }

		public ref TArg Current => ref *ptr;

		public object Clone()
		{
			return new VectorIterator(ptr);
		}

		public void Dispose()
		{ }

		public bool Equals(IIterator<TArg>? other) => ptr == ((VectorIterator)other).ptr;

		public void MoveNext(int count) => ptr += count;

		public void MoveNext() => ptr++;

		public void MovePrevious(int count) => ptr -= count;

		public void MovePrevious() => ptr--;
	}
}

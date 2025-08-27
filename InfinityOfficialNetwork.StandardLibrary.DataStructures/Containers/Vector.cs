using System.Runtime.CompilerServices;
using InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators.Generic;
using InfinityOfficialNetwork.StandardLibrary.MemoryManagement.Memory;

namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Containers
{
	public unsafe class Vector<TArg, TAllocator> : IRandomAccessContainer<TArg> where TArg : unmanaged where TAllocator : IMemoryAllocator, new()
	{
		public struct VectorIterator : IRandomAccessIterator<TArg>
		{
			private unsafe TArg* ptr;

			public unsafe VectorIterator(TArg* ptr)
			{
				this.ptr = ptr;
			}

			public unsafe TArg this[int index] { get => ptr[index]; set => ptr[index] = value; }
			public unsafe TArg Current { get => *ptr; set => *ptr = value; }
			public void Dispose() { }
			public unsafe bool NotEquals(IIterator<TArg> other) => ptr != ((VectorIterator)other).ptr;
			public unsafe void MoveNext(int count) => ptr += count;
			public unsafe void MoveNext() => ptr++;
			public unsafe void MovePrevious(int count) => ptr -= count;
			public unsafe void MovePrevious() => ptr--;
		}

		public struct VectorReverseIterator : IRandomAccessIterator<TArg>
		{
			private unsafe TArg* ptr;

			public unsafe VectorReverseIterator(TArg* ptr)
			{
				this.ptr = ptr;
			}

			public unsafe TArg this[int index] { get => ptr[index]; set => ptr[index] = value; }
			public unsafe TArg Current { get => *ptr; set => *ptr = value; }
			public void Dispose() { }
			public unsafe bool NotEquals(IIterator<TArg> other) => ptr != ((VectorReverseIterator)other).ptr;
			public unsafe void MoveNext(int count) => ptr -= count;
			public unsafe void MoveNext() => ptr--;
			public unsafe void MovePrevious(int count) => ptr += count;
			public unsafe void MovePrevious() => ptr++;
		}

		private const int INITIAL_RESERVE = 4, RESERVE_MULTIPLIER = 2;
		private static TAllocator allocator = new TAllocator();
		private unsafe TArg* data;
		private int count, reserve;

		public unsafe TArg this[int index] { get => data[index]; set => data[index] = value; }

		public unsafe IRandomAccessIterator<TArg> Begin => new VectorIterator(data);
		public unsafe IRandomAccessIterator<TArg> End => new VectorIterator(data + count + 1);
		public unsafe IRandomAccessIterator<TArg> ReverseBegin => new VectorReverseIterator(data + count);
		public unsafe IRandomAccessIterator<TArg> ReverseEnd => new VectorReverseIterator(data - 1);

		public unsafe TArg Back { get => data[count - 1]; set => data[count - 1] = value; }
		public unsafe TArg Front { get => *data; set => *data = value; }

		public int Count => count;

		public int MaxSize => int.MaxValue;

		public int ReserveSize => reserve;

		public bool IsEmpty => count == 0;

		public unsafe void AddBack(TArg item)
		{
			if (reserve > count)
			{
				data[count++] = item;
			}
			else
			{
				int newReserve = reserve * RESERVE_MULTIPLIER;
				TArg* newData = allocator.AllocateObject<TArg>(newReserve);
				for (int i = 0; i < count; i++)
					newData[i] = data[i];
				reserve = newReserve;
				allocator.Free(data);
				data = newData;
				data[count++] = item;
			}
		}

		public void AddBackRange(IRandomAccessContainer<TArg> items)
		{
			if (reserve >= count + items.Count)
			{
				for (int i = 0; i < items.Count; i++)
					data[count++] = items[i];
			}
			else
			{
				int requiredReserve = count + items.Count;
				int newReserve = reserve;
				if (newReserve == 0) newReserve = INITIAL_RESERVE;
				while (newReserve < requiredReserve)
				{
					newReserve *= RESERVE_MULTIPLIER;
				}
				TArg* newData = allocator.AllocateObject<TArg>(newReserve);
				for (int i = 0; i < count; i++)
					newData[i] = data[i];
				reserve = newReserve;
				allocator.Free(data);
				data = newData;
				for (int i = 0; i < items.Count; i++)
					data[count++] = items[i];
			}
		}

		private struct TempStoragePage
		{
			public const int PAGE_SIZE = 64;

			[InlineArray(PAGE_SIZE)]
			public struct Item { public TArg Value; }

			public Item items;
			public TempStoragePage* next;
		}

		public unsafe void AddBackRange(IInputIteratable<TArg> items) => AddBackRange(items.Begin, items.End);

		private void AddBackRangeFromPages(TempStoragePage* firstPage, int pageCount, int pageIndex)
		{
			TempStoragePage* currentPage = firstPage;
			int iCount = (pageCount * TempStoragePage.PAGE_SIZE) + pageIndex;

			if (reserve >= count + iCount)
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
					data[count++] = currentPage->items[pageIndex++];
				}
			}
			else
			{
				int requiredReserve = count + iCount;
				int newReserve = reserve;
				if (newReserve == 0) newReserve = INITIAL_RESERVE;
				while (newReserve < requiredReserve)
				{
					newReserve *= RESERVE_MULTIPLIER;
				}
				TArg* newData = allocator.AllocateObject<TArg>(newReserve);
				for (int i = 0; i < count; i++)
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
					data[count++] = currentPage->items[pageIndex++];
				}
			}
		}

		public void AddBackRange(IInputIterator<TArg> begin, IInputIterator<TArg> end)
		{
			TempStoragePage* firstPage = stackalloc TempStoragePage[1];
			TempStoragePage* currentPage = firstPage;

			int pageCount = 0, pageIndex = 0;
			for (IInputIterator<TArg> i = begin, e = end; i.LessThan(e); i.MoveNext())
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

			AddBackRangeFromPages(firstPage, pageCount, pageIndex);

			TempStoragePage* pageToFree = firstPage;
			for (int i = 0; i < 16; i++)
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

		public void AddBackRange(TArg* items, int iCount)
		{
			if (reserve >= count + iCount)
			{
				for (int i = 0; i < iCount; i++)
					data[count++] = items[i];
			}
			else
			{
				int requiredReserve = count + iCount;
				int newReserve = reserve;
				if (newReserve == 0) newReserve = INITIAL_RESERVE;
				while (newReserve < requiredReserve)
				{
					newReserve *= RESERVE_MULTIPLIER;
				}
				TArg* newData = allocator.AllocateObject<TArg>(newReserve);
				for (int i = 0; i < count; i++)
					newData[i] = data[i];
				reserve = newReserve;
				allocator.Free(data);
				data = newData;
				for (int i = 0; i < iCount; i++)
					data[count++] = items[i];
			}
		}

		public unsafe void AddBackRange(IEnumerable<TArg> items)
		{
			TempStoragePage* firstPage = stackalloc TempStoragePage[1];
			TempStoragePage* currentPage = firstPage;

			int pageCount = 0, pageIndex = 0;
			foreach (var item in items)
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
				currentPage->items[pageIndex++] = item;
			}

			AddBackRangeFromPages(firstPage, pageCount, pageIndex);

			TempStoragePage* pageToFree = firstPage;
			for (int i = 0; i < 16; i++)
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

		public void AddBackRange(ICollection<TArg> items)
		{
			if (reserve >= count + items.Count)
			{
				foreach (var item in items)
					data[count++] = item;
			}
			else
			{
				int requiredReserve = count + items.Count;
				int newReserve = reserve;
				if (newReserve == 0) newReserve = INITIAL_RESERVE;
				while (newReserve < requiredReserve)
				{
					newReserve *= RESERVE_MULTIPLIER;
				}
				TArg* newData = allocator.AllocateObject<TArg>(newReserve);
				for (int i = 0; i < count; i++)
					newData[i] = data[i];
				reserve = newReserve;
				allocator.Free(data);
				data = newData;
				foreach (var item in items)
					data[count++] = item;
			}
		}

		public unsafe void AddFront(TArg item)
		{
			throw new NotImplementedException();
		}

		public void AddFrontRange(IRandomAccessContainer<TArg> items)
		{
			throw new NotImplementedException();
		}

		public void AddFrontRange(IInputIteratable<TArg> items)
		{
			throw new NotImplementedException();
		}

		public void AddFrontRange(IInputIterator<TArg> begin, IInputIterator<TArg> end)
		{
			throw new NotImplementedException();
		}

		public void AddFrontRange(IEnumerable<TArg> items)
		{
			throw new NotImplementedException();
		}

		public void AddFrontRange(ICollection<TArg> items)
		{
			throw new NotImplementedException();
		}

		private void AssignRangeFromPages(TempStoragePage* firstPage, int pageCount, int pageIndex)
		{
			TempStoragePage* currentPage = firstPage;
			int count = (pageCount * TempStoragePage.PAGE_SIZE) + pageIndex;

			allocator.Free(data);
			reserve = Math.Max(this.count = count, INITIAL_RESERVE);
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
				data[this.count++] = currentPage->items[pageIndex++];
			}
		}

		public void Assign(int count, TArg seed)
		{
			allocator.Free(data);
			reserve = Math.Max(this.count = count, INITIAL_RESERVE);
			data = allocator.AllocateObject<TArg>(reserve);
			for (int i = 0; i < count; i++)
				data[i] = seed;
		}

		public void Assign(int count, Func<TArg> factory)
		{
			allocator.Free(data);
			reserve = Math.Max(this.count = count, INITIAL_RESERVE);
			data = allocator.AllocateObject<TArg>(reserve);
			for (int i = 0; i < count; i++)
				data[i] = factory();
		}

		public void Assign(int count, Func<int, TArg> factory)
		{
			allocator.Free(data);
			reserve = Math.Max(this.count = count, INITIAL_RESERVE);
			data = allocator.AllocateObject<TArg>(reserve);
			for (int i = 0; i < count; i++)
				data[i] = factory(i);
		}

		public void AssignRange(IRandomAccessContainer<TArg> items)
		{
			allocator.Free(data);
			int requiredReserve = count + items.Count;
			reserve = INITIAL_RESERVE;
			while (reserve < requiredReserve)
				reserve *= RESERVE_MULTIPLIER;

			data = allocator.AllocateObject<TArg>(reserve);
			for (int i = 0; i < items.Count; i++)
				data[count++] = items[i];
		}

		public void AssignRange(IInputIteratable<TArg> items)
		{
			TempStoragePage* firstPage = stackalloc TempStoragePage[1];
			TempStoragePage* currentPage = firstPage;

			int pageCount = 0, pageIndex = 0;
			for (IInputIterator<TArg> i = items.Begin, e = items.End; i.LessThan(e); i.MoveNext())
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

			AssignRangeFromPages(firstPage, pageCount, pageIndex);

			TempStoragePage* pageToFree = firstPage;
			for (int i = 0; i < 16; i++)
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

		public void AssignRange(IInputIterator<TArg> begin, IInputIterator<TArg> end)
		{
			TempStoragePage* firstPage = stackalloc TempStoragePage[1];
			TempStoragePage* currentPage = firstPage;

			int pageCount = 0, pageIndex = 0;
			for (IInputIterator<TArg> i = begin, e = end; i.LessThan(e); i.MoveNext())
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

			AssignRangeFromPages(firstPage, pageCount, pageIndex);

			TempStoragePage* pageToFree = firstPage;
			for (int i = 0; i < 16; i++)
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

		public void AssignRange(IEnumerable<TArg> items)
		{
			TempStoragePage* firstPage = stackalloc TempStoragePage[1];
			TempStoragePage* currentPage = firstPage;

			int pageCount = 0, pageIndex = 0;
			foreach (var item in items)
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
				currentPage->items[pageIndex++] = item;
			}

			AssignRangeFromPages(firstPage, pageCount, pageIndex);

			TempStoragePage* pageToFree = firstPage;
			for (int i = 0; i < 16; i++)
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

		public void AssignRange(ICollection<TArg> items)
		{
			allocator.Free(data);
			int requiredReserve = count + items.Count;
			reserve = INITIAL_RESERVE;
			while (reserve < requiredReserve)
				reserve *= RESERVE_MULTIPLIER;

			data = allocator.AllocateObject<TArg>(reserve);
			foreach (var item in items)
				data[count++] = item;
		}

		public ref TArg At(int index)
		{
			return ref data[index];
		}

		public void Clear()
		{
			count = reserve = 0;
			allocator.Free(data);
			data = null;
		}

		public void Dispose()
		{
			if (data != null)
			{
				allocator.Free(data);
				data = null;
			}
		}

		public void EmplaceAfter(IForwardIterator<TArg> pos, Func<TArg> factory)
		{
			throw new NotImplementedException();
		}

		public void EmplaceAfter(IForwardIterator<TArg> pos, int count, Func<TArg> factory)
		{
			throw new NotImplementedException();
		}

		public void EmplaceAfter(IForwardIterator<TArg> pos, int count, Func<int, TArg> factory)
		{
			throw new NotImplementedException();
		}

		public void EmplaceBack(Func<TArg> factory)
		{
			throw new NotImplementedException();
		}

		public void EmplaceBack(int count, Func<TArg> factory)
		{
			throw new NotImplementedException();
		}

		public void EmplaceBack(int count, Func<int, TArg> factory)
		{
			throw new NotImplementedException();
		}

		public void EmplaceFront(Func<TArg> factory)
		{
			throw new NotImplementedException();
		}

		public void EmplaceFront(int count, Func<TArg> factory)
		{
			throw new NotImplementedException();
		}

		public void EmplaceFront(int count, Func<int, TArg> factory)
		{
			throw new NotImplementedException();
		}

		public void EraseAfter(IForwardIterator<TArg> pos)
		{
			throw new NotImplementedException();
		}

		public void EraseAfter(IForwardIterator<TArg> begin, IForwardIterator<TArg> end)
		{
			throw new NotImplementedException();
		}

		public void InsertAfter(IForwardIterator<TArg> pos, TArg item)
		{
			throw new NotImplementedException();
		}

		public void InsertRangeAfter(IForwardIterator<TArg> pos, IRandomAccessContainer<TArg> items)
		{
			throw new NotImplementedException();
		}

		public void InsertRangeAfter(IForwardIterator<TArg> pos, IInputIteratable<TArg> items)
		{
			throw new NotImplementedException();
		}

		public void InsertRangeAfter(IForwardIterator<TArg> pos, IInputIterator<TArg> begin, IInputIterator<TArg> end)
		{
			throw new NotImplementedException();
		}

		public void InsertRangeAfter(IForwardIterator<TArg> pos, IEnumerable<TArg> items)
		{
			throw new NotImplementedException();
		}

		public void InsertRangeAfter(IForwardIterator<TArg> pos, ICollection<TArg> items)
		{
			throw new NotImplementedException();
		}

		public void RemoveBack()
		{
			throw new NotImplementedException();
		}

		public void RemoveBack(int count)
		{
			throw new NotImplementedException();
		}

		public void RemoveFront()
		{
			throw new NotImplementedException();
		}

		public void RemoveFront(int count)
		{
			throw new NotImplementedException();
		}

		public void Reserve(int size)
		{
			throw new NotImplementedException();
		}
	}
}

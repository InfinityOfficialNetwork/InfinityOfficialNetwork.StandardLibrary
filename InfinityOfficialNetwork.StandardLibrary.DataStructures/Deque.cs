using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using InfinityOfficialNetwork.StandardLibrary.DataStructures.Allocators;
using InfinityOfficialNetwork.StandardLibrary.DataStructures.Containers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InfinityOfficialNetwork.StandardLibrary.DataStructures
{
	public unsafe class Deque<T> : IList<T>, IDisposable, ICloneable, IEquatable<IVector<T>> where T : struct
	{
		const int PAGE_SIZE = 16;

		private T** data;
		private int pagesCount, startPageIndex, endPageIndex;

		public Deque()
		{
			data = (T**)NativeMemory.Alloc((nuint)sizeof(T*));
			data[0] = (T*)NativeMemory.Alloc((nuint)sizeof(T));
			pagesCount = 1;
			startPageIndex = 0;
			endPageIndex = 0;
		}


		public T this[int index]
		{
			get
			{
				int pageIndex = (index - startPageIndex) / PAGE_SIZE;
				int inPageIndex = (index - startPageIndex) % PAGE_SIZE;
				return data[pageIndex][inPageIndex];
			}
			set
			{
				int pageIndex = (index - startPageIndex) / PAGE_SIZE;
				int inPageIndex = (index - startPageIndex) % PAGE_SIZE;
				data[pageIndex][inPageIndex] = value;
			}
		}

		public int Count => pagesCount * PAGE_SIZE - startPageIndex - (PAGE_SIZE - endPageIndex);

		public bool IsReadOnly => false;

		private void AddPageFront()
		{
			int newPagesCount = pagesCount + 1;
			T** newData = (T**)NativeMemory.Alloc((nuint)(sizeof(T*)*newPagesCount));
			Buffer.MemoryCopy(data, newData + 1, (nuint)(sizeof(T*) * newPagesCount), (nuint)(sizeof(T*) * pagesCount));
			newData[0] = (T*)NativeMemory.Alloc((nuint)sizeof(T));
			NativeMemory.Free(data);
			data = newData;
			pagesCount = newPagesCount;

		}

		public void Add(T item)
		{
			throw new NotImplementedException();
		}

		public void AddFront(T item)
		{

		}

		public void AddBack(T item)
		{
			if (endPageIndex < PAGE_SIZE - 1)
			{
				data[pagesCount - 1][endPageIndex++] = item;
			}
			else
			{
				int newPagesCount = pagesCount + 1;
				T** newData = (T**)NativeMemory.Alloc((nuint)(sizeof(T*) * newPagesCount));
				Buffer.MemoryCopy(data, newData, (nuint)(sizeof(T*) * newPagesCount), (nuint)(sizeof(T*) * pagesCount));
				newData[pagesCount] = (T*)NativeMemory.Alloc((nuint)sizeof(T));
				NativeMemory.Free(data);
				data = newData;
				pagesCount = newPagesCount;

				endPageIndex = 0;
				data[pagesCount - 1][0] = item;
			}
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public bool Contains(T item)
		{
			throw new NotImplementedException();
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public IEnumerator<T> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		public int IndexOf(T item)
		{
			throw new NotImplementedException();
		}

		public void Insert(int index, T item)
		{
			throw new NotImplementedException();
		}

		public bool Remove(T item)
		{
			throw new NotImplementedException();
		}

		public void RemoveAt(int index)
		{
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (data != null)
			{

				data = null;
			}
		}

		~Deque()
		{
			Dispose(disposing: false);
		}

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		public object Clone()
		{
			throw new NotImplementedException();
		}

		public bool Equals(IVector<T>? other)
		{
			throw new NotImplementedException();
		}
	}
}

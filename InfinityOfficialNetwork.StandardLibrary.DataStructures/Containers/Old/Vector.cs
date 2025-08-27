using System.Collections;
using System.Runtime.InteropServices;

#pragma warning disable CS8500

namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Containers.Old
{
	public unsafe class Vector<TArg> : IVector<TArg> where TArg : struct
	{
		static Vector()
		{
			var type = typeof(TArg);
			var fields = type.GetFields();
			foreach (var field in fields)
			{
				if (!field.FieldType.IsValueType)
					throw new InvalidOperationException("Vector cannot store structs with references.");
			}
		}

		const int INITIAL_RESERVE = 4;

		private class VectorEnumerator : IEnumerator<TArg>, IEnumerator
		{
			private Vector<TArg> instance;
			private int position = -1;

			public VectorEnumerator(Vector<TArg> instance)
			{
				this.instance = instance;
			}

			public TArg Current => instance != null ? instance[position] : throw new ObjectDisposedException(nameof(Vector<TArg>));

			object IEnumerator.Current => Current;

			public void Dispose()
			{
			}

			public bool MoveNext()
			{
				position++;
				return position < instance.count;
			}

			public void Reset()
			{
				position = -1;
			}
		}

		private TArg* data;
		private int count, reserve;

		public Vector()
		{
			reserve = INITIAL_RESERVE;
			data = (TArg*)NativeMemory.Alloc((nuint)(sizeof(TArg) * reserve));
			count = 0;
		}

		public Vector(TArg[] values)
		{
			reserve = values.Length;
			data = (TArg*)NativeMemory.Alloc((nuint)(sizeof(TArg) * reserve));
			count = reserve;
			for (int i = 0; i < values.Length; i++)
			{
				data[i] = values[i];
			}
		}

		public Vector(IEnumerable<TArg> values)
		{
			reserve = INITIAL_RESERVE;
			data = (TArg*)NativeMemory.Alloc((nuint)(sizeof(TArg) * reserve));
			count = 0;
			foreach (var v in values)
			{
				Add(v);
			}
		}

		public Vector(Vector<TArg> values)
		{
			reserve = values.reserve;
			count = values.count;
			data = (TArg*)NativeMemory.Alloc((nuint)(sizeof(TArg) * reserve));
			Buffer.MemoryCopy(values.data, data, reserve * sizeof(TArg), count * sizeof(TArg));
		}

		public TArg this[int index] { get => data[index]; set => data[index] = value; }

		public int Count => count;
		public bool IsReadOnly => false;

		public unsafe TArg* Data => data;

		public int Reserve => reserve;

		public int Size => count;

		public int MaxSize => int.MaxValue;

		public bool IsEmpty => count == 0;

		public TArg Front { get => data[0]; set => data[0] = value; }
		public TArg Back { get => data[count - 1]; set => data[count - 1] = value; }

		public void Add(TArg item)
		{
			if (reserve > count)
			{
				data[count++] = item;
			}
			else
			{
				int newReserve = reserve * 2;
				TArg* newData = (TArg*)NativeMemory.Alloc((nuint)(sizeof(TArg) * newReserve));
				Buffer.MemoryCopy(data, newData, newReserve * sizeof(TArg), count * sizeof(TArg));
				reserve = newReserve;
				NativeMemory.Free(data);
				data = newData;
				data[count++] = item;
			}
		}

		public void Clear()
		{
			NativeMemory.Free(data);
			data = (TArg*)NativeMemory.Alloc((nuint)(sizeof(TArg) * INITIAL_RESERVE));
			count = 0;
			reserve = INITIAL_RESERVE;
		}

		public bool Contains(TArg item)
		{
			for (int i = 0; i < count; i++)
			{
				if (data[i].Equals(item))
					return true;
			}
			return false;
		}

		public void CopyTo(TArg[] array, int arrayIndex)
		{
			for (int i = 0; i < count; ++i)
			{
				array[arrayIndex + i] = data[i];
			}
		}

		public IEnumerator<TArg> GetEnumerator()
		{
			return new VectorEnumerator(this);
		}

		public int IndexOf(TArg item)
		{
			for (int i = 0; i < count; i++)
			{
				if (data[i].Equals(item))
					return i;
			}
			return -1;
		}

		public void Insert(int index, TArg item)
		{
			if (reserve > count)
			{
				for (long i = count; i > index; --i)
				{
					data[i] = data[i - 1];
				}
				data[index] = item;
				count++;
			}
			else
			{
				int newReserve = reserve * 2;
				TArg* newData = (TArg*)NativeMemory.Alloc((nuint)(sizeof(TArg) * newReserve));
				Buffer.MemoryCopy(data, newData, newReserve * sizeof(TArg), count * sizeof(TArg));
				NativeMemory.Free(data);
				data = newData;
				reserve = newReserve;
				for (long i = count; i > index; --i)
				{
					data[i] = data[i - 1];
				}
				data[index] = item;
				count++;
			}
		}

		public bool Remove(TArg item)
		{
			for (int i = 0; i < count; i++)
			{
				if (data[i].Equals(item))
				{
					RemoveAt(i);
					return true;
				}
			}
			return false;
		}


		public void RemoveAt(int index)
		{
			if (!(count * 2 < reserve && reserve > INITIAL_RESERVE))
			{
				for (int j = index; j < count - 1; j++)
				{
					data[j] = data[j + 1];
				}
			}
			else
			{
				int newReserve = reserve / 2;
				TArg* newData = (TArg*)NativeMemory.Alloc((nuint)(sizeof(TArg) * newReserve));
				Buffer.MemoryCopy(data, newData, (nuint)(index * sizeof(TArg)), (nuint)(index * sizeof(TArg)));
				nuint trailingBytes = (nuint)((count - index - 1) * sizeof(TArg));
				Buffer.MemoryCopy(data + index + 1, newData + index, trailingBytes, trailingBytes);
				NativeMemory.Free(data);
				data = newData;
				reserve = newReserve;
			}
			count--;
		}

		public void AddBackRange(int index, int count)
		{
			if (!(this.count * 2 < reserve && reserve > INITIAL_RESERVE))
			{
				for (int j = index; j < this.count - count; j++)
				{
					data[j] = data[j + count];
				}
			}
			else
			{
				int newReserve = reserve / 2;
				TArg* newData = (TArg*)NativeMemory.Alloc((nuint)(sizeof(TArg) * newReserve));
				Buffer.MemoryCopy(data, newData, (nuint)(index * sizeof(TArg)), (nuint)(index * sizeof(TArg)));
				nuint trailingBytes = (nuint)((this.count - index - count) * sizeof(TArg));
				Buffer.MemoryCopy(data + index + count, newData + index, trailingBytes, trailingBytes);
				NativeMemory.Free(data);
				data = newData;
				reserve = newReserve;
			}
			this.count -= count;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (data != null)
			{
				NativeMemory.Free(data);
				data = null;
			}
		}

		~Vector()
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
			return new Vector<TArg>(this);
		}

		public void RemoveEnd() => RemoveAt(count - 1);

		public void AddReserve(int newReserve)
		{
			if (newReserve > reserve)
			{
				TArg* newData = (TArg*)NativeMemory.Alloc((nuint)(sizeof(TArg) * newReserve));
				Buffer.MemoryCopy(data, newData, newReserve * sizeof(TArg), count * sizeof(TArg));
				NativeMemory.Free(data);
				data = newData;
				reserve = newReserve;
			}
		}

		public bool Equals(IVector<TArg>? other)
		{
			if (other != null && count == other.Count)
			{
				for (int i = 0; i < count; i++)
					if (!this[i].Equals(other[i]))
						return false;
				return true;
			}
			else return false;
		}

		public void AddEndRange(IEnumerable<TArg> collection)
		{
			foreach (var item in collection) { Add(item); }
		}

		public void AddEndRange(TArg[] collection)
		{
			if (reserve > count + collection.Length)
			{
				foreach (var item in collection)
					data[count++] = item;
			}
			else
			{
				int newReserve = Math.Max(reserve * 2, reserve + collection.Length + 4);
				TArg* newData = (TArg*)NativeMemory.Alloc((nuint)(sizeof(TArg) * newReserve));
				Buffer.MemoryCopy(data, newData, newReserve * sizeof(TArg), count * sizeof(TArg));
				reserve = newReserve;
				NativeMemory.Free(data);
				data = newData;
				foreach (var item in collection)
					data[count++] = item;
			}
		}

		public void AddEndRange(ICollection<TArg> collection)
		{
			if (reserve > count + collection.Count)
			{
				foreach (var item in collection)
					data[count++] = item;
			}
			else
			{
				int newReserve = Math.Max(reserve * 2, reserve + collection.Count + 4);
				TArg* newData = (TArg*)NativeMemory.Alloc((nuint)(sizeof(TArg) * newReserve));
				Buffer.MemoryCopy(data, newData, newReserve * sizeof(TArg), count * sizeof(TArg));
				reserve = newReserve;
				NativeMemory.Free(data);
				data = newData;
				foreach (var item in collection)
					data[count++] = item;
			}
		}

		public ref TArg At(int index)
		{
			throw new NotImplementedException();
		}

		public void Assign(int count, TArg item)
		{
			throw new NotImplementedException();
		}

		public void AssignRange(TArg[] items)
		{
			throw new NotImplementedException();
		}

		public void AssignRange(IEnumerable<TArg> items)
		{
			throw new NotImplementedException();
		}

		public void AssignRange(ICollection<TArg> items)
		{
			throw new NotImplementedException();
		}

		public int IndexOfFirst(TArg item)
		{
			throw new NotImplementedException();
		}

		public int IndexOfLast(TArg item)
		{
			throw new NotImplementedException();
		}

		int[] IVector<TArg>.IndexOf(TArg item)
		{
			throw new NotImplementedException();
		}

		public bool ContainsAllRange(TArg[] items)
		{
			throw new NotImplementedException();
		}

		public bool ContainsAllRange(IEnumerable<TArg> items)
		{
			throw new NotImplementedException();
		}

		public bool ContainsAllRange(ICollection<TArg> items)
		{
			throw new NotImplementedException();
		}

		public bool ContainsAnyRange(TArg[] items)
		{
			throw new NotImplementedException();
		}

		public bool ContainsAnyRange(IEnumerable<TArg> items)
		{
			throw new NotImplementedException();
		}

		public bool ContainsAnyRange(ICollection<TArg> items)
		{
			throw new NotImplementedException();
		}

		public void ShrinkToFit()
		{
			throw new NotImplementedException();
		}

		public void InsertRange(int index, TArg[] items)
		{
			throw new NotImplementedException();
		}

		public void InsertRange(int index, IEnumerable<TArg> items)
		{
			throw new NotImplementedException();
		}

		public void InsertRange(int index, ICollection<TArg> items)
		{
			throw new NotImplementedException();
		}

		public void AddEnd(TArg item)
		{
			throw new NotImplementedException();
		}

		public void EmplaceEnd()
		{
			throw new NotImplementedException();
		}

		public void EmplaceEndRange(int count)
		{
			throw new NotImplementedException();
		}

		bool IVector<TArg>.RemoveEnd()
		{
			throw new NotImplementedException();
		}

		public bool RemoveEndRange(int count)
		{
			throw new NotImplementedException();
		}

		bool IVector<TArg>.RemoveAt(int index)
		{
			throw new NotImplementedException();
		}

		public bool RemoveAtRange(int index, int count)
		{
			throw new NotImplementedException();
		}
	}
}

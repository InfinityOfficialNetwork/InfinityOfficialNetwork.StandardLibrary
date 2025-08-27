using System.Collections;

namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Containers.Old;

public interface IVector<T> : IList<T>, IDisposable, ICloneable, IEquatable<IVector<T>> where T : struct
{
	//Element access

	new public T this[int index] { get; set; }
	new public ref T At(int index);
	new public T Front { get; set; }
	new public T Back { get; set; }
	new public unsafe T* Data { get; }

	new public void Assign(int count, T item);
	new public void AssignRange(T[] items);
	new public void AssignRange(IEnumerable<T> items);
	new public void AssignRange(ICollection<T> items);

	new public int IndexOfFirst(T item);
	new public int IndexOfLast(T item);
	new public int[] IndexOf(T item);
	new public bool Contains(T item);
	new public bool ContainsAllRange(T[] items);
	new public bool ContainsAllRange(IEnumerable<T> items);
	new public bool ContainsAllRange(ICollection<T> items);
	new public bool ContainsAnyRange(T[] items);
	new public bool ContainsAnyRange(IEnumerable<T> items);
	new public bool ContainsAnyRange(ICollection<T> items);

	//Capacity

	new public int Reserve { get; }
	new public int Size { get; }
	new public int MaxSize { get; }
	new public bool IsEmpty { get; }
	new public void AddReserve(int reserve);
	new public void ShrinkToFit();

	//Modifiers
	new public void Clear();
	new public void Insert(int index, T item);
	new public void InsertRange(int index, T[] items);
	new public void InsertRange(int index, IEnumerable<T> items);
	new public void InsertRange(int index, ICollection<T> items);
	new public void AddEnd(T item);
	new public void AddEndRange(T[] items);
	new public void AddEndRange(IEnumerable<T> items);
	new public void AddEndRange(ICollection<T> items);
	new public void EmplaceEnd();
	new public void EmplaceEndRange(int count);
	new public bool RemoveEnd();
	new public bool RemoveEndRange(int count);
	new public bool RemoveAt(int index);
	new public bool RemoveAtRange(int index, int count);

	//Explicit interface decls

	int IList<T>.IndexOf(T item) => IndexOfFirst(item);

	void IList<T>.Insert(int index, T item) => Insert(index, item);

	void IList<T>.RemoveAt(int index) => RemoveAt(index);

	void ICollection<T>.Add(T item) => AddEnd(item);

	void ICollection<T>.Clear() => Clear();

	bool ICollection<T>.Contains(T item) => Contains(item);

	void ICollection<T>.CopyTo(T[] array, int arrayIndex)
	{
		throw new NotImplementedException();
	}

	bool ICollection<T>.Remove(T item) => Remove(item);

	//IEnumerator<T> IEnumerable<T>.GetEnumerator()
	//{
	//	throw new NotImplementedException();
	//}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


	int ICollection<T>.Count => Count;

	bool ICollection<T>.IsReadOnly => false;

	T IList<T>.this[int index] { get => this[index]; set => this[index] = value; }
}



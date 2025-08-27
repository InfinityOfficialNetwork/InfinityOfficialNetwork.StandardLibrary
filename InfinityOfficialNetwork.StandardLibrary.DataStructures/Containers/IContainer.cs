using InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators.Generic;

namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Containers;

public interface IInputContainer<T> : IInputIteratable<T>
{

}

public interface IOutputContainer<T> : IOutputIteratable<T>
{

}

public interface IForwardContainer<T> : IInputContainer<T>, IOutputContainer<T>, IForwardIteratable<T>
{
	new public void Assign(int count, T seed);
	new public void Assign(int count, Func<T> factory);
	new public void Assign(int count, Func<int, T> factory);
	new public void AssignRange(IRandomAccessContainer<T> items);
	new public void AssignRange(IInputIteratable<T> items);
	new public void AssignRange(IInputIterator<T> begin, IInputIterator<T> end);
	new public void AssignRange(IEnumerable<T> items);
	new public void AssignRange(ICollection<T> items);

	new public T Front { get; set; }
	new public int Count { get; }
	new public int MaxSize { get; }
	new public int ReserveSize { get; }
	new public bool IsEmpty { get; }

	new public void Reserve(int size);

	new public void Clear();
	new public void InsertAfter(IForwardIterator<T> pos, T item);
	new public void EmplaceAfter(IForwardIterator<T> pos, Func<T> factory);
	new public void EmplaceAfter(IForwardIterator<T> pos, int count, Func<T> factory);
	new public void EmplaceAfter(IForwardIterator<T> pos, int count, Func<int, T> factory);
	new public void InsertRangeAfter(IForwardIterator<T> pos, IRandomAccessContainer<T> items);
	new public void InsertRangeAfter(IForwardIterator<T> pos, IInputIteratable<T> items);
	new public void InsertRangeAfter(IForwardIterator<T> pos, IInputIterator<T> begin, IInputIterator<T> end);
	new public void InsertRangeAfter(IForwardIterator<T> pos, IEnumerable<T> items);
	new public void InsertRangeAfter(IForwardIterator<T> pos, ICollection<T> items);
	new public void EraseAfter(IForwardIterator<T> pos);
	new public void EraseAfter(IForwardIterator<T> begin, IForwardIterator<T> end);
	new public void AddFront(T item);
	new public void AddFrontRange(IRandomAccessContainer<T> items);
	new public void AddFrontRange(IInputIteratable<T> items);
	new public void AddFrontRange(IInputIterator<T> begin, IInputIterator<T> end);
	new public void AddFrontRange(IEnumerable<T> items);
	new public void AddFrontRange(ICollection<T> items);
	new public void EmplaceFront(Func<T> factory);
	new public void EmplaceFront(int count, Func<T> factory);
	new public void EmplaceFront(int count, Func<int, T> factory);
	new public void RemoveFront();
	new public void RemoveFront(int count);
}

public interface IBidirectionalContainer<T> : IForwardContainer<T>, IBidirectionalIteratable<T>
{
	new public T Back { get; set; }

	new public void AddBack(T item);
	new public void AddBackRange(IRandomAccessContainer<T> items);
	new public void AddBackRange(IInputIteratable<T> items);
	new public void AddBackRange(IInputIterator<T> begin, IInputIterator<T> end);
	new public void AddBackRange(IEnumerable<T> items);
	new public void AddBackRange(ICollection<T> items);
	new public void EmplaceBack(Func<T> factory);
	new public void EmplaceBack(int count, Func<T> factory);
	new public void EmplaceBack(int count, Func<int, T> factory);
	new public void RemoveBack();
	new public void RemoveBack(int count);
}

public interface IRandomAccessContainer<T> : IRandomAccessIteratable<T>, IBidirectionalContainer<T>
{
	new public T this[int index] { get; set; }
	new public ref T At(int index);
}

public interface IArrayContainer<T> : IRandomAccessContainer<T>
{
}

public interface IVectorContainer<T> : IRandomAccessContainer<T>
{
}

public interface IDequeContainer<T> : IRandomAccessContainer<T>
{
}

public interface ISinglyLinkedListContainer<T> : IForwardContainer<T>
{

}

public interface IDoublyLinkedListContainer<T> : IBidirectionalContainer<T>
{
}

public interface ISetContainer<T> : IBidirectionalContainer<T>
{ }

public interface IMapContainer<T> : IBidirectionalContainer<T>
{ }

using InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators;

namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Containers;

public interface IContainer<TArg> : IDisposable
{
	public int MaxSize { get; }
}

public interface IContainerAssign<TArg>
{
	public void Assign(TArg value) => Assign(1, value);
	public void Assign(int count, TArg value);
	public void Assign(int count, Func<TArg> factory) => Assign(count, (int i) => factory());
	public void Assign(int count, Func<int, TArg> factory);

	public void AssignRange(IInputIterator<TArg> first, IInputIterator<TArg> last);
	public void AssignRange(IStreamingInputIterator<TArg> first, IStreamingIterator<TArg> last) => AssignRange((IInputIterator<TArg>)first, (IInputIterator<TArg>)last);
	public void AssignRange(IForwardIterator<TArg> first, IForwardIterator<TArg> last) => AssignRange((IStreamingInputIterator<TArg>)first, (IStreamingInputIterator<TArg>)last);
	public void AssignRange(IBidirectionalIterator<TArg> first, IBidirectionalIterator<TArg> last) => AssignRange((IForwardIterator<TArg>)first, (IForwardIterator<TArg>)last);
	public void AssignRange(IRandomAccessIterator<TArg> first, IRandomAccessIterator<TArg> last) => AssignRange((IBidirectionalIterator<TArg>)first, (IBidirectionalIterator<TArg>)last);
	public void AssignRange(IContiguousIterator<TArg> first, IContiguousIterator<TArg> last) => AssignRange((IRandomAccessIterator<TArg>)first, (IRandomAccessIterator<TArg>)last);

	public void AssignRange(IInputIteratable<TArg> items) => AssignRange(items.Begin, items.End);
	public void AssignRange(IStreamingInputIteratable<TArg> items) => AssignRange(items.Begin, items.End);
	public void AssignRange(IForwardIteratable<TArg> items) => AssignRange(items.Begin, items.End);
	public void AssignRange(IBidirectionalIteratable<TArg> items) => AssignRange(items.Begin, items.End);
	public void AssignRange(IRandomAccessIteratable<TArg> items) => AssignRange(items.Begin, items.End);
	public void AssignRange(IContiguousIteratable<TArg> items) => AssignRange(items.Begin, items.End);
}

public interface IContainerElementRandomAccess<TArg>
{
	public ref TArg this[int index] { get => ref At(index); }
	public ref TArg At(int index);
}

public interface IContainerElementFrontAccess<TArg>
{
	public ref TArg Front { get; }
}

public interface IContainerElementBackAccess<TArg>
{
	public ref TArg Back { get; }
}

public interface IContainerElementNativeArrayAccess<TArg>
{
	public unsafe TArg* Data { get; }
}

public interface IContainerCapacityIsEmpty<TArg>
{
	public bool IsEmpty { get; }
}

public interface IContainerCapacitySize<TArg>
{
	public int Size { get; }
}

public interface IContainerCapacityResize<TArg>
{
	public void Resize(int size);
}

public interface IContainerCapacityReserve<TArg>
{
	public int ReserveSize { get; }
	public void Reserve(int size);
	public void ShrinkToFit();
}

public interface IContainerClear<TArg>
{
	public void Clear();
}

public interface IContainerInsertAtIndex<TArg>
{
	public void Insert(int index, TArg value);
	public void Insert(int index, int count, TArg value);
	public void Insert(int index, int count, Func<TArg> factory);
	public void Insert(int index, int count, Func<int, TArg> factory);

	public void InsertRange(int index, IInputIterator<TArg> first, IInputIterator<TArg> last);
	public void InsertRange(int index, IStreamingInputIterator<TArg> first, IStreamingIterator<TArg> last) => InsertRange(index, (IInputIterator<TArg>)first, (IInputIterator<TArg>)last);
	public void InsertRange(int index, IForwardIterator<TArg> first, IForwardIterator<TArg> last) => InsertRange(index, (IStreamingInputIterator<TArg>)first, (IStreamingInputIterator<TArg>)last);
	public void InsertRange(int index, IBidirectionalIterator<TArg> first, IBidirectionalIterator<TArg> last) => InsertRange(index, (IForwardIterator<TArg>)first, (IForwardIterator<TArg>)last);
	public void InsertRange(int index, IRandomAccessIterator<TArg> first, IRandomAccessIterator<TArg> last) => InsertRange(index, (IBidirectionalIterator<TArg>)first, (IBidirectionalIterator<TArg>)last);
	public void InsertRange(int index, IContiguousIterator<TArg> first, IContiguousIterator<TArg> last) => InsertRange(index, (IRandomAccessIterator<TArg>)first, (IRandomAccessIterator<TArg>)last);

	public void InsertRange(int index, IInputIteratable<TArg> items) => InsertRange(index, items.Begin, items.End);
	public void InsertRange(int index, IStreamingInputIteratable<TArg> items) => InsertRange(index, items.Begin, items.End);
	public void InsertRange(int index, IForwardIteratable<TArg> items) => InsertRange(index, items.Begin, items.End);
	public void InsertRange(int index, IBidirectionalIteratable<TArg> items) => InsertRange(index, items.Begin, items.End);
	public void InsertRange(int index, IRandomAccessIteratable<TArg> items) => InsertRange(index, items.Begin, items.End);
	public void InsertRange(int index, IContiguousIteratable<TArg> items) => InsertRange(index, items.Begin, items.End);
}

public interface IContainerInsertAtIterator<TArg>
{
	public void Insert(IIterator<TArg> it, TArg value);
	public void Insert(IIterator<TArg> it, int count, TArg value);
	public void Insert(IIterator<TArg> it, int count, Func<TArg> factory);
	public void Insert(IIterator<TArg> it, int count, Func<int, TArg> factory);

	public void InsertRange(IIterator<TArg> it, IInputIterator<TArg> first, IInputIterator<TArg> last);
	public void InsertRange(IIterator<TArg> it, IStreamingInputIterator<TArg> first, IStreamingIterator<TArg> last) => InsertRange(it, (IInputIterator<TArg>)first, (IInputIterator<TArg>)last);
	public void InsertRange(IIterator<TArg> it, IForwardIterator<TArg> first, IForwardIterator<TArg> last) => InsertRange(it, (IStreamingInputIterator<TArg>)first, (IStreamingInputIterator<TArg>)last);
	public void InsertRange(IIterator<TArg> it, IBidirectionalIterator<TArg> first, IBidirectionalIterator<TArg> last) => InsertRange(it, (IForwardIterator<TArg>)first, (IForwardIterator<TArg>)last);
	public void InsertRange(IIterator<TArg> it, IRandomAccessIterator<TArg> first, IRandomAccessIterator<TArg> last) => InsertRange(it, (IBidirectionalIterator<TArg>)first, (IBidirectionalIterator<TArg>)last);
	public void InsertRange(IIterator<TArg> it, IContiguousIterator<TArg> first, IContiguousIterator<TArg> last) => InsertRange(it, (IRandomAccessIterator<TArg>)first, (IRandomAccessIterator<TArg>)last);

	public void InsertRange(IIterator<TArg> it, IInputIteratable<TArg> items) => InsertRange(it, items.Begin, items.End);
	public void InsertRange(IIterator<TArg> it, IStreamingInputIteratable<TArg> items) => InsertRange(it, items.Begin, items.End);
	public void InsertRange(IIterator<TArg> it, IForwardIteratable<TArg> items) => InsertRange(it, items.Begin, items.End);
	public void InsertRange(IIterator<TArg> it, IBidirectionalIteratable<TArg> items) => InsertRange(it, items.Begin, items.End);
	public void InsertRange(IIterator<TArg> it, IRandomAccessIteratable<TArg> items) => InsertRange(it, items.Begin, items.End);
	public void InsertRange(IIterator<TArg> it, IContiguousIteratable<TArg> items) => InsertRange(it, items.Begin, items.End);
}

public interface IContainerRemoveAtIndex<TArg>
{
	public void Remove(int index);
	public void Remove(int index, int count);
}

public interface IContainerRemoveAtIterator<TArg, TIterator> where TIterator : IIterator<TArg>
{
	public void Remove(TIterator it);
	public void Remove(TIterator first, TIterator last);
}

public interface IContainerAddFront<TArg>
{
	public void AddFront(TArg value);
	public void AddFront(int count, TArg value);
	public void AddFront(int count, Func<TArg> factory);
	public void AddFront(int count, Func<int, TArg> factory);

	public void AddFrontRange(IInputIterator<TArg> first, IInputIterator<TArg> last);
	public void AddFrontRange(IStreamingInputIterator<TArg> first, IStreamingIterator<TArg> last) => AddFrontRange((IInputIterator<TArg>)first, (IInputIterator<TArg>)last);
	public void AddFrontRange(IForwardIterator<TArg> first, IForwardIterator<TArg> last) => AddFrontRange((IStreamingInputIterator<TArg>)first, (IStreamingInputIterator<TArg>)last);
	public void AddFrontRange(IBidirectionalIterator<TArg> first, IBidirectionalIterator<TArg> last) => AddFrontRange((IForwardIterator<TArg>)first, (IForwardIterator<TArg>)last);
	public void AddFrontRange(IRandomAccessIterator<TArg> first, IRandomAccessIterator<TArg> last) => AddFrontRange((IBidirectionalIterator<TArg>)first, (IBidirectionalIterator<TArg>)last);
	public void AddFrontRange(IContiguousIterator<TArg> first, IContiguousIterator<TArg> last) => AddFrontRange((IRandomAccessIterator<TArg>)first, (IRandomAccessIterator<TArg>)last);

	public void AddFrontRange(IInputIteratable<TArg> items) => AddFrontRange(items.Begin, items.End);
	public void AddFrontRange(IStreamingInputIteratable<TArg> items) => AddFrontRange(items.Begin, items.End);
	public void AddFrontRange(IForwardIteratable<TArg> items) => AddFrontRange(items.Begin, items.End);
	public void AddFrontRange(IBidirectionalIteratable<TArg> items) => AddFrontRange(items.Begin, items.End);
	public void AddFrontRange(IRandomAccessIteratable<TArg> items) => AddFrontRange(items.Begin, items.End);
	public void AddFrontRange(IContiguousIteratable<TArg> items) => AddFrontRange(items.Begin, items.End);
}

public interface IContainerAddBack<TArg>
{
	public void AddBack(TArg value);
	public void AddBack(int count, TArg value);
	public void AddBack(int count, Func<TArg> factory);
	public void AddBack(int count, Func<int, TArg> factory);

	public void AddBackRange(IInputIterator<TArg> first, IInputIterator<TArg> last);
	public void AddBackRange(IStreamingInputIterator<TArg> first, IStreamingIterator<TArg> last) => AddBackRange((IInputIterator<TArg>)first, (IInputIterator<TArg>)last);
	public void AddBackRange(IForwardIterator<TArg> first, IForwardIterator<TArg> last) => AddBackRange((IStreamingInputIterator<TArg>)first, (IStreamingInputIterator<TArg>)last);
	public void AddBackRange(IBidirectionalIterator<TArg> first, IBidirectionalIterator<TArg> last) => AddBackRange((IForwardIterator<TArg>)first, (IForwardIterator<TArg>)last);
	public void AddBackRange(IRandomAccessIterator<TArg> first, IRandomAccessIterator<TArg> last) => AddBackRange((IBidirectionalIterator<TArg>)first, (IBidirectionalIterator<TArg>)last);
	public void AddBackRange(IContiguousIterator<TArg> first, IContiguousIterator<TArg> last) => AddBackRange((IRandomAccessIterator<TArg>)first, (IRandomAccessIterator<TArg>)last);

	public void AddBackRange(IInputIteratable<TArg> items) => AddBackRange(items.Begin, items.End);
	public void AddBackRange(IStreamingInputIteratable<TArg> items) => AddBackRange(items.Begin, items.End);
	public void AddBackRange(IForwardIteratable<TArg> items) => AddBackRange(items.Begin, items.End);
	public void AddBackRange(IBidirectionalIteratable<TArg> items) => AddBackRange(items.Begin, items.End);
	public void AddBackRange(IRandomAccessIteratable<TArg> items) => AddBackRange(items.Begin, items.End);
	public void AddBackRange(IContiguousIteratable<TArg> items) => AddBackRange(items.Begin, items.End);
}

public interface IContainerRemoveFront<TArg>
{
	public void RemoveFront();
	public void RemoveFront(int count);
}

public interface IContainerRemoveBack<TArg>
{
	public void RemoveBack();
	public void RemoveBack(int count);
}
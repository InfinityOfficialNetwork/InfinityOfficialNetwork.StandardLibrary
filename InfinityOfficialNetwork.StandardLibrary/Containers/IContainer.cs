using InfinityOfficialNetwork.StandardLibrary.Iterators;
using InfinityOfficialNetwork.StandardLibrary.Iterators.Tiers.Levels;
using InfinityOfficialNetwork.StandardLibrary.Parallel.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfinityOfficialNetwork.StandardLibrary.Containers;

public interface IContainer
{ }

public interface IAsyncIteratable<out TIteratorTier, out TIterator, out TAwaitable>
	where TIteratorTier : IIterator
	where TIterator : TIteratorTier
	where TAwaitable : IAsyncFunc<TAwaitable, TIterator>
{
	public TAwaitable GetBeginAsync(CancellationToken cancellationToken = default);
	public TAwaitable GetEndAsync(CancellationToken cancellationToken = default);
}

public interface IIteratable<out TIteratorTier, out TIterator>
	where TIteratorTier : IIterator
	where TIterator : TIteratorTier, allows ref struct
{
	public TIterator Begin { get; }
	public TIterator End { get; }
}


//container size operations 


public interface IContainerMaxSize : IContainer
{
	/// <summary>
	/// The implementation defined maximum number of elements that can be stored in a single instance of the container
	/// </summary>
	public static abstract ulong MaxSize { get; }
}

public interface IContainerSize : IContainer
{
	/// <summary>
	/// The current number of elements in the container
	/// </summary>
	public ulong Size { get; }
}

public interface IContainerEmpty : IContainer
{
	public bool IsEmpty { get; }
}

public interface IContainerResize
{
	/// <summary>
	/// Resizes the container to the new size, if the resize operation increases the size, the new elements will be zero-initialized, if the operation decreases the size, the overflow elements will be dropped
	/// </summary>
	/// <param name="newSize"></param>
	public void Resize(ulong newSize);
}

public interface IContainerReserveCapacity
{
	/// <summary>
	/// Maximum number of elements a container can be resized to without triggering a reallocation and iterator invalidation
	/// </summary>
	public ulong MaxReserveCapacity { get; }
	/// <summary>
	/// Minimum number of elements a container can be resized to without triggering a reallocation and iterator invalidation
	/// </summary>
	public ulong MinReserveCapacity { get; }
}

public interface IContainerReserve
{
	/// <summary>
	/// Ensures the container has enough reserve reserve capacity for `reserveCapacity` number of elements, does nothing if `reserveCapacity` is less than MaxReserveCapacity
	/// </summary>
	/// <param name="reserveCapacity"></param>
	public void Reserve(ulong reserveCapacity);
	/// <summary>
	/// Shrinks the containers allocated reserve capacity to the current Size
	/// </summary>
	public void Shrink();
}

public interface IContainerClear
{
	/// <summary>
	/// Clears all elements from the container and sets Size to zero; may involve reallocation
	/// </summary>
	public void Clear();
}


//container element operations
//all invalid index operations are undefined behavior, unless implementation protected


public interface IReadOnlyContainerAtIndex<out TArg>
	where TArg : allows ref struct
{
	public TArg GetAt(ulong index);
	public TArg? TryGetAt(ulong index);
}

public interface IWriteOnlyContainerAtIndex<in TArg>
	where TArg : allows ref struct
{
	public void SetAt(ulong index, TArg value);
	public bool TrySetAt(ulong index, TArg value);
}

public interface IContainerAtIndex<TArg> : IReadOnlyContainerAtIndex<TArg>, IWriteOnlyContainerAtIndex<TArg>
	where TArg : allows ref struct
{
	public TArg this[ulong index] { get; set; }
}

public interface IRefContainerAtIndex<TArg> : IContainerAtIndex<TArg>
	where TArg : allows ref struct
{
	public ref TArg At(ulong index);

	new public ref TArg this[ulong index] { get; }
}


////
///

public interface IContainerAddBack<in TArg>
{
	public void AddBack(TArg value);
	public void AddBackRange<TInputIterator>(TInputIterator first, TInputIterator last) where TInputIterator : IReadOnlyStreamIterator<TArg>, allows ref struct;
}

public interface IContainerAddFront<in TArg>
{
	public void AddFront(TArg value);
	public void AddFrontRange<TInputIterator>(TInputIterator first, TInputIterator last) where TInputIterator : IReadOnlyStreamIterator<TArg>, allows ref struct;
}

public interface IContainerAddAtIndex<in TArg>
{
	public void AddAt(ulong index, TArg value);
	public void AddAtRange<TInputIterator>(ulong index, TInputIterator first, TInputIterator last) where TInputIterator : IReadOnlyStreamIterator<TArg>, allows ref struct;
}

public interface IContainerAddAtIterator<in TArg, in TContainerIterator>
{
	public void AddAt(TContainerIterator index, TArg value);
	public void AddAtRange<TInputIterator>(TContainerIterator index, TInputIterator first, TInputIterator last) where TInputIterator : IReadOnlyStreamIterator<TArg>, allows ref struct;
}


public interface IContainerRemoveBack
{
	public void RemoveBack();
	public void RemoveBackRange(ulong count);

	public bool TryRemoveBack();
	public ulong TryRemoveBackRange(ulong count);
}

public interface IContainerRemoveFront
{
	public void RemoveFront();
	public void RemoveFrontRange(ulong count);

	public bool TryRemoveFront();
	public ulong TryRemoveFrontRange(ulong count);
}

public interface IContainerRemoveAtIndex
{
	public void RemoveAtIndex(ulong index);
	public void RemoveAtIndexRange(ulong index, ulong count);

	public bool TryRemoveAtIndex(ulong index);
	public ulong TryRemoveAtIndexRange(ulong index, ulong count);
}

public interface IContainerRemoveAtIterator<in TIterator>
{
	public void RemoveAtIterator(TIterator first);
	public void RemoveAtIteratorRange(TIterator first, TIterator last);

	public bool TryRemoveAtIterator(TIterator first);
	public ulong TryRemoveAtIteratorRange(TIterator first, TIterator last);
}


public interface IContainerTakeBack<out TArg>
{
	public TArg TakeBack<TOutputIterator>(TOutputIterator first, TOutputIterator last) where TOutputIterator : IWriteOnlyStreamIterator<TArg>, allows ref struct;
	public void TakeBackRange<TOutputIterator>(ulong count, TOutputIterator first, TOutputIterator last) where TOutputIterator : IWriteOnlyStreamIterator<TArg>, allows ref struct;

	public TArg? TryTakeBack<TOutputIterator>(TOutputIterator first, TOutputIterator last) where TOutputIterator : IWriteOnlyStreamIterator<TArg>, allows ref struct;
	public ulong TryTakeBackRange<TOutputIterator>(ulong count, TOutputIterator first, TOutputIterator last) where TOutputIterator : IWriteOnlyStreamIterator<TArg>, allows ref struct;
}

public interface IContainerTakeFront<out TArg>
{
	public TArg TakeFront<TOutputIterator>(TOutputIterator first, TOutputIterator last) where TOutputIterator : IWriteOnlyStreamIterator<TArg>, allows ref struct;
	public void TakeFrontRange<TOutputIterator>(ulong count, TOutputIterator first, TOutputIterator last) where TOutputIterator : IWriteOnlyStreamIterator<TArg>, allows ref struct;

	public TArg? TryTakeFront<TOutputIterator>(TOutputIterator first, TOutputIterator last) where TOutputIterator : IWriteOnlyStreamIterator<TArg>, allows ref struct;
	public ulong TryTakeFrontRange<TOutputIterator>(ulong count, TOutputIterator first, TOutputIterator last) where TOutputIterator : IWriteOnlyStreamIterator<TArg>, allows ref struct;
}

public interface IContainerTakeAtIndex<out TArg>
{
	public TArg TakeAtIndex<TOutputIterator>(ulong index, TOutputIterator first, TOutputIterator last) where TOutputIterator : IWriteOnlyStreamIterator<TArg>, allows ref struct;
	public void TakeAtIndexRange<TOutputIterator>(ulong index, ulong count, TOutputIterator first, TOutputIterator last) where TOutputIterator : IWriteOnlyStreamIterator<TArg>, allows ref struct;

	public TArg? TryTakeAtIndex<TOutputIterator>(ulong index, TOutputIterator first, TOutputIterator last) where TOutputIterator : IWriteOnlyStreamIterator<TArg>, allows ref struct;
	public ulong TryTakeAtIndexRange<TOutputIterator>(ulong index, ulong count, TOutputIterator first, TOutputIterator last) where TOutputIterator : IWriteOnlyStreamIterator<TArg>, allows ref struct;
}

public interface IContainerTakeAtIterator<out TArg, in TIterator>
{
	public TArg TakeAtIterator<TOutputIterator>(TIterator begin, TOutputIterator first, TOutputIterator last) where TOutputIterator : IWriteOnlyStreamIterator<TArg>, allows ref struct;
	public void TakeAtIteratorRange<TOutputIterator>(TIterator begin, TIterator end, TOutputIterator first, TOutputIterator last) where TOutputIterator : IWriteOnlyStreamIterator<TArg>, allows ref struct;

	public TArg? TryTakeAtIterator<TOutputIterator>(TIterator begin, TOutputIterator first, TOutputIterator last) where TOutputIterator : IWriteOnlyStreamIterator<TArg>, allows ref struct;
	public ulong TryTakeAtIteratorRange<TOutputIterator>(TIterator begin, TIterator end, TOutputIterator first, TOutputIterator last) where TOutputIterator : IWriteOnlyStreamIterator<TArg>, allows ref struct;
}
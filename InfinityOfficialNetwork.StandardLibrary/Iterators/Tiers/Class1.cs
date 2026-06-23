using InfinityOfficialNetwork.StandardLibrary.Iterators.Operations;
using InfinityOfficialNetwork.StandardLibrary.Iterators.Tiers.Levels;

namespace InfinityOfficialNetwork.StandardLibrary.Iterators.Tiers;
//ctrp iterators

public interface IReadOnlyStreamIterator<TSelf, out TArg>
	: IReadOnlyStreamIterator<TArg>, IIteratorEquatable<TSelf>
	where TSelf : IReadOnlyStreamIterator<TSelf, TArg>, allows ref struct
	where TArg : allows ref struct
{ }
public interface IWriteOnlyStreamIterator<TSelf, in TArg>
	: IWriteOnlyStreamIterator<TArg>, IIteratorEquatable<TSelf>
	where TSelf : IWriteOnlyStreamIterator<TSelf, TArg>, allows ref struct
	where TArg : allows ref struct
{ }
public interface IStreamIterator<TSelf, TArg>
	: IStreamIterator<TArg>, IReadOnlyStreamIterator<TArg>, IWriteOnlyStreamIterator<TArg>
	where TSelf : IStreamIterator<TSelf, TArg>, allows ref struct
	where TArg : allows ref struct
{ }
public interface IRefStreamIterator<TSelf, TArg>
	: IRefStreamIterator<TArg>, IStreamIterator<TSelf, TArg>
	where TSelf : IRefStreamIterator<TSelf, TArg>, allows ref struct
	where TArg : allows ref struct
{ }

public interface IReadOnlyForwardIterator<TSelf, out TArg>
	: IReadOnlyForwardIterator<TArg>, IReadOnlyStreamIterator<TSelf, TArg>, IIteratorCloneable<TSelf>
	where TSelf : IReadOnlyForwardIterator<TSelf, TArg>, allows ref struct
	where TArg : allows ref struct
{ }
public interface IWriteOnlyForwardIterator<TSelf, in TArg>
	: IWriteOnlyForwardIterator<TArg>, IWriteOnlyStreamIterator<TSelf, TArg>, IIteratorCloneable<TSelf>
	where TSelf : IWriteOnlyForwardIterator<TSelf, TArg>, allows ref struct
	where TArg : allows ref struct
{ }
public interface IForwardIterator<TSelf, TArg>
	: IForwardIterator<TArg>, IReadOnlyForwardIterator<TSelf, TArg>, IWriteOnlyForwardIterator<TSelf, TArg>, IStreamIterator<TSelf, TArg>
	where TSelf : IForwardIterator<TSelf, TArg>, allows ref struct
	where TArg : allows ref struct
{ }
public interface IRefForwardIterator<TSelf, TArg>
	: IRefForwardIterator<TArg>, IForwardIterator<TSelf, TArg>, IRefStreamIterator<TSelf, TArg>
	where TSelf : IRefForwardIterator<TSelf, TArg>, allows ref struct
	where TArg : allows ref struct
{ }

public interface IReadOnlyBidirectionalIterator<TSelf, out TArg>
	: IReadOnlyBidirectionalIterator<TArg>, IReadOnlyForwardIterator<TSelf, TArg>
	where TSelf : IReadOnlyBidirectionalIterator<TSelf, TArg>, allows ref struct
	where TArg : allows ref struct
{ }
public interface IWriteOnlyBidirectionalIterator<TSelf, in TArg>
	: IWriteOnlyBidirectionalIterator<TArg>, IWriteOnlyForwardIterator<TSelf, TArg>
	where TSelf : IWriteOnlyBidirectionalIterator<TSelf, TArg>, allows ref struct
	where TArg : allows ref struct
{ }
public interface IBidirectionalIterator<TSelf, TArg>
	: IBidirectionalIterator<TArg>, IReadOnlyBidirectionalIterator<TSelf, TArg>, IWriteOnlyBidirectionalIterator<TSelf, TArg>, IForwardIterator<TSelf, TArg>
	where TSelf : IBidirectionalIterator<TSelf, TArg>, allows ref struct
	where TArg : allows ref struct
{ }
public interface IRefBidirectionalIterator<TSelf, TArg>
	: IRefBidirectionalIterator<TArg>, IBidirectionalIterator<TSelf, TArg>, IRefForwardIterator<TSelf, TArg>
	where TSelf : IRefBidirectionalIterator<TSelf, TArg>, allows ref struct
	where TArg : allows ref struct
{ }

public interface IReadOnlyRandomAccessIterator<TSelf, out TArg>
	: IReadOnlyRandomAccessIterator<TArg>, IReadOnlyBidirectionalIterator<TSelf, TArg>, IIteratorComparable<TSelf>
	where TSelf : IReadOnlyRandomAccessIterator<TSelf, TArg>, allows ref struct
	where TArg : allows ref struct
{ }
public interface IWriteOnlyRandomAccessIterator<TSelf, in TArg>
	: IWriteOnlyRandomAccessIterator<TArg>, IWriteOnlyBidirectionalIterator<TSelf, TArg>, IIteratorComparable<TSelf>
	where TSelf : IWriteOnlyRandomAccessIterator<TSelf, TArg>, allows ref struct
	where TArg : allows ref struct
{ }
public interface IRandomAccessIterator<TSelf, TArg>
	: IRandomAccessIterator<TArg>, IReadOnlyRandomAccessIterator<TSelf, TArg>, IWriteOnlyRandomAccessIterator<TSelf, TArg>, IBidirectionalIterator<TSelf, TArg>
	where TSelf : IRandomAccessIterator<TSelf, TArg>, allows ref struct
	where TArg : allows ref struct
{ }
public interface IRefRandomAccessIterator<TSelf, TArg>
	: IRefRandomAccessIterator<TArg>, IRandomAccessIterator<TSelf, TArg>, IRefBidirectionalIterator<TSelf, TArg>
	where TSelf : IRefRandomAccessIterator<TSelf, TArg>, allows ref struct
	where TArg : allows ref struct
{ }

//implies the refs from Value between 2 iterators from the same container span a contiguous range of elements
public interface IContiguousIterator<TSelf, TArg>
	: IContiguousIterator<TArg>, IRefRandomAccessIterator<TSelf, TArg>
	where TSelf : IContiguousIterator<TSelf, TArg>, allows ref struct
	where TArg : allows ref struct
{ }

//allows taking a raw pointer address
public interface IAddressableIterator<TSelf, TArg>
	: IAddressableIterator<TArg>, IContiguousIterator<TSelf, TArg>
	where TSelf : IAddressableIterator<TSelf, TArg>, allows ref struct
	where TArg : allows ref struct
{ }
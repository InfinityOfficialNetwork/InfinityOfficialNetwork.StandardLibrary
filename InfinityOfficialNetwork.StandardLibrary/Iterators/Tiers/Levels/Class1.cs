using InfinityOfficialNetwork.StandardLibrary.Iterators.Operations;
using InfinityOfficialNetwork.StandardLibrary.Iterators.Tiers.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfinityOfficialNetwork.StandardLibrary.Iterators.Tiers.Levels;
//base iterator types
public interface IReadOnlyStreamIterator<out TArg>
	: IReadOnlyIterator<TArg>, IIteratorMoveNext, IIteratorValueGet<TArg>
	where TArg : allows ref struct
{ }
public interface IWriteOnlyStreamIterator<in TArg>
	: IWriteOnlyIterator<TArg>, IIteratorMoveNext, IIteratorValueSet<TArg>
	where TArg : allows ref struct
{ }
public interface IStreamIterator<TArg>
	: IIterator<TArg>, IIteratorValueGetSet<TArg>, IReadOnlyStreamIterator<TArg>, IWriteOnlyStreamIterator<TArg>
	where TArg : allows ref struct
{ }
public interface IRefStreamIterator<TArg>
	: IRefIterator<TArg>, IIteratorValueGetRef<TArg>, IStreamIterator<TArg>
	where TArg : allows ref struct
{ }

public interface IReadOnlyForwardIterator<out TArg>
	: IReadOnlyStreamIterator<TArg>
	where TArg : allows ref struct
{ }
public interface IWriteOnlyForwardIterator<in TArg>
	: IWriteOnlyStreamIterator<TArg>
	where TArg : allows ref struct
{ }
public interface IForwardIterator<TArg>
	: IReadOnlyForwardIterator<TArg>, IWriteOnlyForwardIterator<TArg>, IStreamIterator<TArg>
	where TArg : allows ref struct
{ }
public interface IRefForwardIterator<TArg>
	: IForwardIterator<TArg>, IRefStreamIterator<TArg>
	where TArg : allows ref struct
{ }

public interface IReadOnlyBidirectionalIterator<out TArg>
	: IIteratorMoveBack, IReadOnlyForwardIterator<TArg>
	where TArg : allows ref struct
{ }
public interface IWriteOnlyBidirectionalIterator<in TArg>
	: IIteratorMoveBack, IWriteOnlyForwardIterator<TArg>
	where TArg : allows ref struct
{ }
public interface IBidirectionalIterator<TArg>
	: IReadOnlyBidirectionalIterator<TArg>, IWriteOnlyBidirectionalIterator<TArg>, IForwardIterator<TArg>
	where TArg : allows ref struct
{ }
public interface IRefBidirectionalIterator<TArg>
	: IBidirectionalIterator<TArg>, IRefForwardIterator<TArg>
	where TArg : allows ref struct
{ }

public interface IReadOnlyRandomAccessIterator<out TArg>
	: IIteratorMoveNextMany, IIteratorMoveBackMany, IReadOnlyBidirectionalIterator<TArg>
	where TArg : allows ref struct
{ }
public interface IWriteOnlyRandomAccessIterator<in TArg>
	: IIteratorMoveNextMany, IIteratorMoveBackMany, IWriteOnlyBidirectionalIterator<TArg>
	where TArg : allows ref struct
{ }
public interface IRandomAccessIterator<TArg>
	: IReadOnlyRandomAccessIterator<TArg>, IWriteOnlyRandomAccessIterator<TArg>, IBidirectionalIterator<TArg>
	where TArg : allows ref struct
{ }
public interface IRefRandomAccessIterator<TArg>
	: IRandomAccessIterator<TArg>, IRefBidirectionalIterator<TArg>
	where TArg : allows ref struct
{ }

//implies the refs from Value between 2 iterators from the same container span a contiguous range of elements
public interface IContiguousIterator<TArg>
	: IRefRandomAccessIterator<TArg>
	where TArg : allows ref struct
{ }

//allows taking a raw pointer address
public interface IAddressableIterator<TArg>
	: IIteratorValueGetPtr<TArg>, IContiguousIterator<TArg>
	where TArg : allows ref struct
{ }
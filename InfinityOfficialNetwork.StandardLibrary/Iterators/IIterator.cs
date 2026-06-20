using InfinityOfficialNetwork.StandardLibrary.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfinityOfficialNetwork.StandardLibrary.Iterators;

public interface IIterator { }

public interface IIteratorMoveNext
{
	public void MoveNext();
}

public interface IIteratorMoveNextMany : IIteratorMoveNext
{
	public void MoveNext(ulong count);
}

public interface IIteratorMoveBack
{
	public void MoveBack();
}

public interface IIteratorMoveBackMany : IIteratorMoveBack
{
	public void MoveBack(ulong count);
}

public interface IIteratorValueGet<out TArg>
	where TArg : allows ref struct
{
	public TArg Value { get; }
}

public interface IIteratorValueSet<in TArg>
	where TArg : allows ref struct
{
	public TArg Value { set; }
}

public interface IIteratorValueGetSet<TArg> : IIteratorValueGet<TArg>, IIteratorValueSet<TArg>
	where TArg : allows ref struct
{
	new public TArg Value { get; set; }
	//TArg IIteratorValueGet<TArg>.Value => Value;
	//TArg IIteratorValueSet<TArg>.Value { set => Value = value; }
}

public interface IIteratorValueGetRef<TArg> : IIteratorValueGetSet<TArg>
{
	public ref TArg ValueRef { get; }
	//TArg IIteratorValueGet<TArg>.Value => Value;
	//TArg IIteratorValueSet<TArg>.Value { set => Value = value; }
	//TArg IIteratorValueGetSet<TArg>.Value { get => Value; set => Value = value; }
}

public interface IIteratorValueGetPtr<TArg>
	where TArg : unmanaged
{
	public unsafe TArg* Ptr { get; }
}

public interface IIteratorCloneable<TSelf>
	where TSelf : IIteratorCloneable<TSelf>, allows ref struct
{
	public TSelf Clone();
}

public interface IIteratorEquatable<TSelf>
	where TSelf : IIteratorEquatable<TSelf>, allows ref struct
{
	public bool Equals(TSelf other);
	public static virtual bool operator ==(TSelf lhs, TSelf rhs) => lhs.Equals(rhs);
	public static virtual bool operator !=(TSelf lhs, TSelf rhs) => !lhs.Equals(rhs);
}

public interface IIteratorComparable<TSelf> : IIteratorEquatable<TSelf>
	where TSelf : IIteratorComparable<TSelf>, allows ref struct
{
	//returns the number of elements between this and other, or negative if this preceeds other
	public long Compare(TSelf other);
	public static virtual bool operator >(TSelf lhs, TSelf rhs) => lhs.Compare(rhs) > 0;
	public static virtual bool operator <(TSelf lhs, TSelf rhs) => lhs.Compare(rhs) < 0;
	public static virtual bool operator <=(TSelf lhs, TSelf rhs) => lhs.Compare(rhs) <= 0;
	public static virtual bool operator >=(TSelf lhs, TSelf rhs) => lhs.Compare(rhs) >= 0;
}


//base iterator types
public interface IReadOnlyStreamIterator<out TArg>
	: IIterator, IIteratorMoveNext, IIteratorValueGet<TArg>
	where TArg : allows ref struct
{ }
public interface IWriteOnlyStreamIterator<in TArg>
	: IIterator, IIteratorMoveNext, IIteratorValueSet<TArg>
	where TArg : allows ref struct
{ }
public interface IStreamIterator<TArg>
	: IIteratorValueGetSet<TArg>, IReadOnlyStreamIterator<TArg>, IWriteOnlyStreamIterator<TArg>
	where TArg : allows ref struct
{ }
public interface IRefStreamIterator<TArg>
	: IIteratorValueGetRef<TArg>, IStreamIterator<TArg>
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
{ }

//implies the refs from Value between 2 iterators from the same container span a contiguous range of elements
public interface IContiguousIterator<TArg>
	: IRefRandomAccessIterator<TArg>
{ }

//allows taking a raw pointer address
public interface IAddressableIterator<TArg>
	: IIteratorValueGetPtr<TArg>, IContiguousIterator<TArg>
	where TArg : unmanaged
{ }


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
{ }

//implies the refs from Value between 2 iterators from the same container span a contiguous range of elements
public interface IContiguousIterator<TSelf, TArg>
	: IContiguousIterator<TArg>, IRefRandomAccessIterator<TSelf, TArg>
	where TSelf : IContiguousIterator<TSelf, TArg>, allows ref struct
{ }

//allows taking a raw pointer address
public interface IAddressableIterator<TSelf, TArg>
	: IAddressableIterator<TArg>, IContiguousIterator<TSelf, TArg>
	where TSelf : IAddressableIterator<TSelf, TArg>, allows ref struct
	where TArg : unmanaged
{ }
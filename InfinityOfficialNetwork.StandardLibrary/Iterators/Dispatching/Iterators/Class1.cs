using InfinityOfficialNetwork.StandardLibrary.Iterators.Tiers.Levels;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfinityOfficialNetwork.StandardLibrary.Iterators.Dispatching.Iterators;
public interface IReadOnlyIteratorVisitor<in TArg>
	where TArg : allows ref struct
{
	public void VisitReadOnlyStreamIterator<TIterator>(TIterator iterator) where TIterator : IReadOnlyStreamIterator<TArg>, allows ref struct;
	public void VisitReadOnlyForwardIterator<TIterator>(TIterator iterator) where TIterator : IReadOnlyForwardIterator<TArg>, allows ref struct;
	public void VisitReadOnlyBidirectionalIterator<TIterator>(TIterator iterator) where TIterator : IReadOnlyBidirectionalIterator<TArg>, allows ref struct;
	public void VisitReadOnlyRandomAccessIterator<TIterator>(TIterator iterator) where TIterator : IReadOnlyRandomAccessIterator<TArg>, allows ref struct;
}

public interface IReadOnlyIteratorVisitor<in TArg, out TRet>
	where TArg : allows ref struct
	where TRet : allows ref struct
{
	public TRet VisitReadOnlyStreamIterator<TIterator>(TIterator iterator) where TIterator : IReadOnlyStreamIterator<TArg>, allows ref struct;
	public TRet VisitReadOnlyForwardIterator<TIterator>(TIterator iterator) where TIterator : IReadOnlyForwardIterator<TArg>, allows ref struct;
	public TRet VisitReadOnlyBidirectionalIterator<TIterator>(TIterator iterator) where TIterator : IReadOnlyBidirectionalIterator<TArg>, allows ref struct;
	public TRet VisitReadOnlyRandomAccessIterator<TIterator>(TIterator iterator) where TIterator : IReadOnlyRandomAccessIterator<TArg>, allows ref struct;
}

public interface IWriteOnlyIteratorVisitor<out TArg>
	where TArg : allows ref struct
{
	public void VisitWriteOnlyStreamIterator<TIterator>(TIterator iterator) where TIterator : IWriteOnlyStreamIterator<TArg>, allows ref struct;
	public void VisitWriteOnlyForwardIterator<TIterator>(TIterator iterator) where TIterator : IWriteOnlyForwardIterator<TArg>, allows ref struct;
	public void VisitWriteOnlyBidirectionalIterator<TIterator>(TIterator iterator) where TIterator : IWriteOnlyBidirectionalIterator<TArg>, allows ref struct;
	public void VisitWriteOnlyRandomAccessIterator<TIterator>(TIterator iterator) where TIterator : IWriteOnlyRandomAccessIterator<TArg>, allows ref struct;
}

public interface IWriteOnlyIteratorVisitor<out TArg, out TRet>
	where TArg : allows ref struct
	where TRet : allows ref struct
{
	public TRet VisitWriteOnlyStreamIterator<TIterator>(TIterator iterator) where TIterator : IWriteOnlyStreamIterator<TArg>, allows ref struct;
	public TRet VisitWriteOnlyForwardIterator<TIterator>(TIterator iterator) where TIterator : IWriteOnlyForwardIterator<TArg>, allows ref struct;
	public TRet VisitWriteOnlyBidirectionalIterator<TIterator>(TIterator iterator) where TIterator : IWriteOnlyBidirectionalIterator<TArg>, allows ref struct;
	public TRet VisitWriteOnlyRandomAccessIterator<TIterator>(TIterator iterator) where TIterator : IWriteOnlyRandomAccessIterator<TArg>, allows ref struct;
}

public interface IIteratorVisitor<TArg>
	where TArg : allows ref struct
{
	public void VisitStreamIterator<TIterator>(TIterator iterator) where TIterator : IStreamIterator<TArg>, allows ref struct;
	public void VisitForwardIterator<TIterator>(TIterator iterator) where TIterator : IForwardIterator<TArg>, allows ref struct;
	public void VisitBidirectionalIterator<TIterator>(TIterator iterator) where TIterator : IBidirectionalIterator<TArg>, allows ref struct;
	public void VisitRandomAccessIterator<TIterator>(TIterator iterator) where TIterator : IRandomAccessIterator<TArg>, allows ref struct;
}

public interface IIteratorVisitor<TArg, out TRet>
	where TArg : allows ref struct
	where TRet : allows ref struct
{
	public TRet VisitStreamIterator<TIterator>(TIterator iterator) where TIterator : IStreamIterator<TArg>, allows ref struct;
	public TRet VisitForwardIterator<TIterator>(TIterator iterator) where TIterator : IForwardIterator<TArg>, allows ref struct;
	public TRet VisitBidirectionalIterator<TIterator>(TIterator iterator) where TIterator : IBidirectionalIterator<TArg>, allows ref struct;
	public TRet VisitRandomAccessIterator<TIterator>(TIterator iterator) where TIterator : IRandomAccessIterator<TArg>, allows ref struct;
}

public interface IRefIteratorVisitor<TArg>
	where TArg : allows ref struct
{
	public void VisitRefStreamIterator<TIterator>(TIterator iterator) where TIterator : IRefStreamIterator<TArg>, allows ref struct;
	public void VisitRefForwardIterator<TIterator>(TIterator iterator) where TIterator : IRefForwardIterator<TArg>, allows ref struct;
	public void VisitRefBidirectionalIterator<TIterator>(TIterator iterator) where TIterator : IRefBidirectionalIterator<TArg>, allows ref struct;
	public void VisitRefRandomAccessIterator<TIterator>(TIterator iterator) where TIterator : IRefRandomAccessIterator<TArg>, allows ref struct;
	public void VisitContiguousIterator<TIterator>(TIterator iterator) where TIterator : IContiguousIterator<TArg>, allows ref struct;
	public void VisitAddressableIterator<TIterator>(TIterator iterator) where TIterator : IAddressableIterator<TArg>, allows ref struct;
}

public interface IRefIteratorVisitor<TArg, out TRet>
	where TArg : allows ref struct
	where TRet : allows ref struct
{
	public TRet VisitRefStreamIterator<TIterator>(TIterator iterator) where TIterator : IRefStreamIterator<TArg>, allows ref struct;
	public TRet VisitRefForwardIterator<TIterator>(TIterator iterator) where TIterator : IRefForwardIterator<TArg>, allows ref struct;
	public TRet VisitRefBidirectionalIterator<TIterator>(TIterator iterator) where TIterator : IRefBidirectionalIterator<TArg>, allows ref struct;
	public TRet VisitRefRandomAccessIterator<TIterator>(TIterator iterator) where TIterator : IRefRandomAccessIterator<TArg>, allows ref struct;
	public TRet VisitContiguousIterator<TIterator>(TIterator iterator) where TIterator : IContiguousIterator<TArg>, allows ref struct;
	public TRet VisitAddressableIterator<TIterator>(TIterator iterator) where TIterator : IAddressableIterator<TArg>, allows ref struct;
}

public interface IStaticReadOnlyIteratorVisitor<in TArg>
	where TArg : allows ref struct
{
	public static abstract void VisitReadOnlyStreamIterator<TIterator>(TIterator iterator) where TIterator : IReadOnlyStreamIterator<TArg>, allows ref struct;
	public static abstract void VisitReadOnlyForwardIterator<TIterator>(TIterator iterator) where TIterator : IReadOnlyForwardIterator<TArg>, allows ref struct;
	public static abstract void VisitReadOnlyBidirectionalIterator<TIterator>(TIterator iterator) where TIterator : IReadOnlyBidirectionalIterator<TArg>, allows ref struct;
	public static abstract void VisitReadOnlyRandomAccessIterator<TIterator>(TIterator iterator) where TIterator : IReadOnlyRandomAccessIterator<TArg>, allows ref struct;
}

public interface IStaticReadOnlyIteratorVisitor<in TArg, out TRet>
	where TArg : allows ref struct
	where TRet : allows ref struct
{
	public static abstract TRet VisitReadOnlyStreamIterator<TIterator>(TIterator iterator) where TIterator : IReadOnlyStreamIterator<TArg>, allows ref struct;
	public static abstract TRet VisitReadOnlyForwardIterator<TIterator>(TIterator iterator) where TIterator : IReadOnlyForwardIterator<TArg>, allows ref struct;
	public static abstract TRet VisitReadOnlyBidirectionalIterator<TIterator>(TIterator iterator) where TIterator : IReadOnlyBidirectionalIterator<TArg>, allows ref struct;
	public static abstract TRet VisitReadOnlyRandomAccessIterator<TIterator>(TIterator iterator) where TIterator : IReadOnlyRandomAccessIterator<TArg>, allows ref struct;
}

public interface IStaticWriteOnlyIteratorVisitor<out TArg>
	where TArg : allows ref struct
{
	public static abstract void VisitWriteOnlyStreamIterator<TIterator>(TIterator iterator) where TIterator : IWriteOnlyStreamIterator<TArg>, allows ref struct;
	public static abstract void VisitWriteOnlyForwardIterator<TIterator>(TIterator iterator) where TIterator : IWriteOnlyForwardIterator<TArg>, allows ref struct;
	public static abstract void VisitWriteOnlyBidirectionalIterator<TIterator>(TIterator iterator) where TIterator : IWriteOnlyBidirectionalIterator<TArg>, allows ref struct;
	public static abstract void VisitWriteOnlyRandomAccessIterator<TIterator>(TIterator iterator) where TIterator : IWriteOnlyRandomAccessIterator<TArg>, allows ref struct;
}

public interface IStaticWriteOnlyIteratorVisitor<out TArg, out TRet>
	where TArg : allows ref struct
	where TRet : allows ref struct
{
	public static abstract TRet VisitWriteOnlyStreamIterator<TIterator>(TIterator iterator) where TIterator : IWriteOnlyStreamIterator<TArg>, allows ref struct;
	public static abstract TRet VisitWriteOnlyForwardIterator<TIterator>(TIterator iterator) where TIterator : IWriteOnlyForwardIterator<TArg>, allows ref struct;
	public static abstract TRet VisitWriteOnlyBidirectionalIterator<TIterator>(TIterator iterator) where TIterator : IWriteOnlyBidirectionalIterator<TArg>, allows ref struct;
	public static abstract TRet VisitWriteOnlyRandomAccessIterator<TIterator>(TIterator iterator) where TIterator : IWriteOnlyRandomAccessIterator<TArg>, allows ref struct;
}

public interface IStaticIteratorVisitor<TArg>
	where TArg : allows ref struct
{
	public static abstract void VisitStreamIterator<TIterator>(TIterator iterator) where TIterator : IStreamIterator<TArg>, allows ref struct;
	public static abstract void VisitForwardIterator<TIterator>(TIterator iterator) where TIterator : IForwardIterator<TArg>, allows ref struct;
	public static abstract void VisitBidirectionalIterator<TIterator>(TIterator iterator) where TIterator : IBidirectionalIterator<TArg>, allows ref struct;
	public static abstract void VisitRandomAccessIterator<TIterator>(TIterator iterator) where TIterator : IRandomAccessIterator<TArg>, allows ref struct;
}

public interface IStaticIteratorVisitor<TArg, out TRet>
	where TArg : allows ref struct
	where TRet : allows ref struct
{
	public static abstract TRet VisitStreamIterator<TIterator>(TIterator iterator) where TIterator : IStreamIterator<TArg>, allows ref struct;
	public static abstract TRet VisitForwardIterator<TIterator>(TIterator iterator) where TIterator : IForwardIterator<TArg>, allows ref struct;
	public static abstract TRet VisitBidirectionalIterator<TIterator>(TIterator iterator) where TIterator : IBidirectionalIterator<TArg>, allows ref struct;
	public static abstract TRet VisitRandomAccessIterator<TIterator>(TIterator iterator) where TIterator : IRandomAccessIterator<TArg>, allows ref struct;
}

public interface IStaticRefIteratorVisitor<TArg>
	where TArg : allows ref struct
{
	public static abstract void VisitRefStreamIterator<TIterator>(TIterator iterator) where TIterator : IRefStreamIterator<TArg>, allows ref struct;
	public static abstract void VisitRefForwardIterator<TIterator>(TIterator iterator) where TIterator : IRefForwardIterator<TArg>, allows ref struct;
	public static abstract void VisitRefBidirectionalIterator<TIterator>(TIterator iterator) where TIterator : IRefBidirectionalIterator<TArg>, allows ref struct;
	public static abstract void VisitRefRandomAccessIterator<TIterator>(TIterator iterator) where TIterator : IRefRandomAccessIterator<TArg>, allows ref struct;
	public static abstract void VisitContiguousIterator<TIterator>(TIterator iterator) where TIterator : IContiguousIterator<TArg>, allows ref struct;
	public static abstract void VisitAddressableIterator<TIterator>(TIterator iterator) where TIterator : IAddressableIterator<TArg>, allows ref struct;
}

public interface IStaticRefIteratorVisitor<TArg, out TRet>
	where TArg : allows ref struct
	where TRet : allows ref struct
{
	public static abstract TRet VisitRefStreamIterator<TIterator>(TIterator iterator) where TIterator : IRefStreamIterator<TArg>, allows ref struct;
	public static abstract TRet VisitRefForwardIterator<TIterator>(TIterator iterator) where TIterator : IRefForwardIterator<TArg>, allows ref struct;
	public static abstract TRet VisitRefBidirectionalIterator<TIterator>(TIterator iterator) where TIterator : IRefBidirectionalIterator<TArg>, allows ref struct;
	public static abstract TRet VisitRefRandomAccessIterator<TIterator>(TIterator iterator) where TIterator : IRefRandomAccessIterator<TArg>, allows ref struct;
	public static abstract TRet VisitContiguousIterator<TIterator>(TIterator iterator) where TIterator : IContiguousIterator<TArg>, allows ref struct;
	public static abstract TRet VisitAddressableIterator<TIterator>(TIterator iterator) where TIterator : IAddressableIterator<TArg>, allows ref struct;
}

using InfinityOfficialNetwork.StandardLibrary.Iterators.Tiers.Levels;

namespace InfinityOfficialNetwork.StandardLibrary.Iterators.Dispatching.Iterations;
public interface IReadOnlyIterationVisitor<in TArg>
	where TArg : allows ref struct
{
	public void VisitReadOnlyStreamIteration<TIterator>(TIterator first, TIterator last) where TIterator : IReadOnlyStreamIterator<TArg>, allows ref struct;
	public void VisitReadOnlyForwardIteration<TIterator>(TIterator first, TIterator last) where TIterator : IReadOnlyForwardIterator<TArg>, allows ref struct;
	public void VisitReadOnlyBidirectionalIteration<TIterator>(TIterator first, TIterator last) where TIterator : IReadOnlyBidirectionalIterator<TArg>, allows ref struct;
	public void VisitReadOnlyRandomAccessIteration<TIterator>(TIterator first, TIterator last) where TIterator : IReadOnlyRandomAccessIterator<TArg>, allows ref struct;
}

public interface IReadOnlyIterationVisitor<in TArg, out TRet>
	where TArg : allows ref struct
	where TRet : allows ref struct
{
	public TRet VisitReadOnlyStreamIteration<TIterator>(TIterator first, TIterator last) where TIterator : IReadOnlyStreamIterator<TArg>, allows ref struct;
	public TRet VisitReadOnlyForwardIteration<TIterator>(TIterator first, TIterator last) where TIterator : IReadOnlyForwardIterator<TArg>, allows ref struct;
	public TRet VisitReadOnlyBidirectionalIteration<TIterator>(TIterator first, TIterator last) where TIterator : IReadOnlyBidirectionalIterator<TArg>, allows ref struct;
	public TRet VisitReadOnlyRandomAccessIteration<TIterator>(TIterator first, TIterator last) where TIterator : IReadOnlyRandomAccessIterator<TArg>, allows ref struct;
}

public interface IWriteOnlyIterationVisitor<out TArg>
	where TArg : allows ref struct
{
	public void VisitWriteOnlyStreamIteration<TIterator>(TIterator first, TIterator last) where TIterator : IWriteOnlyStreamIterator<TArg>, allows ref struct;
	public void VisitWriteOnlyForwardIteration<TIterator>(TIterator first, TIterator last) where TIterator : IWriteOnlyForwardIterator<TArg>, allows ref struct;
	public void VisitWriteOnlyBidirectionalIteration<TIterator>(TIterator first, TIterator last) where TIterator : IWriteOnlyBidirectionalIterator<TArg>, allows ref struct;
	public void VisitWriteOnlyRandomAccessIteration<TIterator>(TIterator first, TIterator last) where TIterator : IWriteOnlyRandomAccessIterator<TArg>, allows ref struct;
}

public interface IWriteOnlyIterationVisitor<out TArg, out TRet>
	where TArg : allows ref struct
	where TRet : allows ref struct
{
	public TRet VisitWriteOnlyStreamIteration<TIterator>(TIterator first, TIterator last) where TIterator : IWriteOnlyStreamIterator<TArg>, allows ref struct;
	public TRet VisitWriteOnlyForwardIteration<TIterator>(TIterator first, TIterator last) where TIterator : IWriteOnlyForwardIterator<TArg>, allows ref struct;
	public TRet VisitWriteOnlyBidirectionalIteration<TIterator>(TIterator first, TIterator last) where TIterator : IWriteOnlyBidirectionalIterator<TArg>, allows ref struct;
	public TRet VisitWriteOnlyRandomAccessIteration<TIterator>(TIterator first, TIterator last) where TIterator : IWriteOnlyRandomAccessIterator<TArg>, allows ref struct;
}

public interface IIterationVisitor<TArg>
	where TArg : allows ref struct
{
	public void VisitStreamIteration<TIterator>(TIterator first, TIterator last) where TIterator : IStreamIterator<TArg>, allows ref struct;
	public void VisitForwardIteration<TIterator>(TIterator first, TIterator last) where TIterator : IForwardIterator<TArg>, allows ref struct;
	public void VisitBidirectionalIteration<TIterator>(TIterator first, TIterator last) where TIterator : IBidirectionalIterator<TArg>, allows ref struct;
	public void VisitRandomAccessIteration<TIterator>(TIterator first, TIterator last) where TIterator : IRandomAccessIterator<TArg>, allows ref struct;
}

public interface IIterationVisitor<TArg, out TRet>
	where TArg : allows ref struct
	where TRet : allows ref struct
{
	public TRet VisitStreamIteration<TIterator>(TIterator first, TIterator last) where TIterator : IStreamIterator<TArg>, allows ref struct;
	public TRet VisitForwardIteration<TIterator>(TIterator first, TIterator last) where TIterator : IForwardIterator<TArg>, allows ref struct;
	public TRet VisitBidirectionalIteration<TIterator>(TIterator first, TIterator last) where TIterator : IBidirectionalIterator<TArg>, allows ref struct;
	public TRet VisitRandomAccessIteration<TIterator>(TIterator first, TIterator last) where TIterator : IRandomAccessIterator<TArg>, allows ref struct;
}

public interface IRefIterationVisitor<TArg>
	where TArg : allows ref struct
{
	public void VisitRefStreamIteration<TIterator>(TIterator first, TIterator last) where TIterator : IRefStreamIterator<TArg>, allows ref struct;
	public void VisitRefForwardIteration<TIterator>(TIterator first, TIterator last) where TIterator : IRefForwardIterator<TArg>, allows ref struct;
	public void VisitRefBidirectionalIteration<TIterator>(TIterator first, TIterator last) where TIterator : IRefBidirectionalIterator<TArg>, allows ref struct;
	public void VisitRefRandomAccessIteration<TIterator>(TIterator first, TIterator last) where TIterator : IRefRandomAccessIterator<TArg>, allows ref struct;
	public void VisitContiguousIteration<TIterator>(TIterator first, TIterator last) where TIterator : IContiguousIterator<TArg>, allows ref struct;
	public void VisitAddressableIteration<TIterator>(TIterator first, TIterator last) where TIterator : IAddressableIterator<TArg>, allows ref struct;
}

public interface IRefIterationVisitor<TArg, out TRet>
	where TArg : allows ref struct
	where TRet : allows ref struct
{
	public TRet VisitRefStreamIteration<TIterator>(TIterator first, TIterator last) where TIterator : IRefStreamIterator<TArg>, allows ref struct;
	public TRet VisitRefForwardIteration<TIterator>(TIterator first, TIterator last) where TIterator : IRefForwardIterator<TArg>, allows ref struct;
	public TRet VisitRefBidirectionalIteration<TIterator>(TIterator first, TIterator last) where TIterator : IRefBidirectionalIterator<TArg>, allows ref struct;
	public TRet VisitRefRandomAccessIteration<TIterator>(TIterator first, TIterator last) where TIterator : IRefRandomAccessIterator<TArg>, allows ref struct;
	public TRet VisitContiguousIteration<TIterator>(TIterator first, TIterator last) where TIterator : IContiguousIterator<TArg>, allows ref struct;
	public TRet VisitAddressableIteration<TIterator>(TIterator first, TIterator last) where TIterator : IAddressableIterator<TArg>, allows ref struct;
}
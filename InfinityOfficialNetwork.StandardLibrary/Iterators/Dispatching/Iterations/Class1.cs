using InfinityOfficialNetwork.StandardLibrary.Iterators.Tiers.Levels;

namespace InfinityOfficialNetwork.StandardLibrary.Iterators.Dispatching.Iterations;
public interface IReadOnlyIterationVisitor<in TArg>
	where TArg : allows ref struct
{
	public void VisitReadOnlyStreamIterator<TIterator>(TIterator first, TIterator last) where TIterator : IReadOnlyStreamIterator<TArg>, allows ref struct;
	public void VisitReadOnlyForwardIterator<TIterator>(TIterator first, TIterator last) where TIterator : IReadOnlyForwardIterator<TArg>, allows ref struct;
	public void VisitReadOnlyBidirectionalIterator<TIterator>(TIterator first, TIterator last) where TIterator : IReadOnlyBidirectionalIterator<TArg>, allows ref struct;
	public void VisitReadOnlyRandomAccessIterator<TIterator>(TIterator first, TIterator last) where TIterator : IReadOnlyRandomAccessIterator<TArg>, allows ref struct;
}
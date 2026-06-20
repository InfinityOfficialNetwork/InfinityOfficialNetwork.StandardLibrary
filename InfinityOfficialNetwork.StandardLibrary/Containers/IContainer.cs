using InfinityOfficialNetwork.StandardLibrary.Iterators;
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
	where TIterator : TIteratorTier, allows ref struct
{
	public TIterator Begin { get; }
	public TIterator End { get; }
}


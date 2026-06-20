using InfinityOfficialNetwork.StandardLibrary.DataStructures.Containers;
using InfinityOfficialNetwork.StandardLibrary.DataStructures.Containers.Basic;
using InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfinityOfficialNetwork.StandardLibrary.Implementations.Managed.DataStructures.Containers.Basic;

public struct Span<TArg> : ISpan<TArg>, IContainerContiguousIteratable<TArg, Span<TArg>.SpanIterator>
{
	private Memory<TArg> _memory;

	public ref TArg this[long index] => throw new NotImplementedException();

	public bool IsEmpty => throw new NotImplementedException();

	public long Size => throw new NotImplementedException();

	public ref TArg Back => throw new NotImplementedException();

	public ref TArg Front => throw new NotImplementedException();

	SpanIterator IContainerContiguousIteratable<TArg, SpanIterator>.Begin => throw new NotImplementedException();
	SpanIterator IContainerContiguousIteratable<TArg, SpanIterator>.End => throw new NotImplementedException();


	public void Assign(long count, TArg value) => throw new NotImplementedException();
	public void AssignRange<TIterator>(TIterator first, TIterator last) where TIterator : IReadOnlyStreamingIterator<TArg> => throw new NotImplementedException();
	public ValueTask AssignRangeAsync<TIterator>(TIterator first, TIterator last) where TIterator : IReadOnlyAsyncStreamingIterator<TArg> => throw new NotImplementedException();
	public void Dispose() => throw new NotImplementedException();
	public ValueTask DisposeAsync() => throw new NotImplementedException();





	public unsafe struct SpanIterator : IContiguousIterator<TArg>
	{
		public ref TArg Current => throw new NotImplementedException();

		public unsafe TArg* Pointer => throw new NotImplementedException();

		public object Clone() => throw new NotImplementedException();
		public void MoveBack() => throw new NotImplementedException();
		public void MoveBack(long count) => throw new NotImplementedException();
		public void MoveNext() => throw new NotImplementedException();
		public void MoveNext(long count) => throw new NotImplementedException();
	}
}

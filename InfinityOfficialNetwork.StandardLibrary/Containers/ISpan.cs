using InfinityOfficialNetwork.StandardLibrary.Iterators.Dispatching.Iterators;
using InfinityOfficialNetwork.StandardLibrary.Iterators.Tiers;
using InfinityOfficialNetwork.StandardLibrary.Iterators.Tiers.Levels;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfinityOfficialNetwork.StandardLibrary.Containers;

public interface ISpan<TArg> : IContainer, IContainerAssign<TArg>, IContainerFront<TArg>, IContainerBack<TArg>, IRefContainerAtIndex<TArg>, IContainerSize, IContainerMaxSize, IContainerEmpty
{}

public ref struct Span<TArg> : ISpan<TArg>, IIteratable<IContiguousIterator<TArg>,Span<TArg>.SpanIterator>
	where TArg : unmanaged
{
	public ref struct SpanIterator : IContiguousIterator<SpanIterator, TArg>
	{
		public ref TArg ValueRef => throw new NotImplementedException();

		public TArg Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public void Accept<TIteratorVisitor>(TIteratorVisitor visitor) where TIteratorVisitor : IIteratorVisitor<TArg>, allows ref struct => throw new NotImplementedException();
		public TRet Accept<TIteratorVisitor, TRet>(TIteratorVisitor visitor) where TIteratorVisitor : IIteratorVisitor<TArg, TRet>, allows ref struct => throw new NotImplementedException();
		public SpanIterator Clone() => throw new NotImplementedException();
		public long Compare(SpanIterator other) => throw new NotImplementedException();
		public bool Equals(SpanIterator other) => throw new NotImplementedException();
		public void MoveBack(ulong count) => throw new NotImplementedException();
		public void MoveBack() => throw new NotImplementedException();
		public void MoveNext(ulong count) => throw new NotImplementedException();
		public void MoveNext() => throw new NotImplementedException();
		public void ReadOnlyAccept<TIteratorVisitor>(TIteratorVisitor visitor) where TIteratorVisitor : IReadOnlyIteratorVisitor<TArg>, allows ref struct => throw new NotImplementedException();
		public TRet ReadOnlyAccept<TIteratorVisitor, TRet>(TIteratorVisitor visitor) where TIteratorVisitor : IReadOnlyIteratorVisitor<TArg, TRet>, allows ref struct => throw new NotImplementedException();
		public void RefAccept<TIteratorVisitor>(TIteratorVisitor visitor) where TIteratorVisitor : IRefIteratorVisitor<TArg>, allows ref struct => throw new NotImplementedException();
		public TRet RefAccept<TIteratorVisitor, TRet>(TIteratorVisitor visitor) where TIteratorVisitor : IRefIteratorVisitor<TArg, TRet>, allows ref struct => throw new NotImplementedException();
		public void WriteOnlyAccept<TIteratorVisitor>(TIteratorVisitor visitor) where TIteratorVisitor : IWriteOnlyIteratorVisitor<TArg>, allows ref struct => throw new NotImplementedException();
		public TRet WriteOnlyAccept<TIteratorVisitor, TRet>(TIteratorVisitor visitor) where TIteratorVisitor : IWriteOnlyIteratorVisitor<TArg, TRet>, allows ref struct => throw new NotImplementedException();
	}


	public ref TArg this[ulong index] => throw new NotImplementedException();

	TArg IContainerAtIndex<TArg>.this[ulong index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

	public static ulong MaxSize => throw new NotImplementedException();

	public TArg Front { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
	public TArg Back { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

	public ulong Size => throw new NotImplementedException();

	public bool IsEmpty => throw new NotImplementedException();

	public SpanIterator Begin => throw new NotImplementedException();

	public SpanIterator End => throw new NotImplementedException();

	public void Assign(TArg value, ulong count) => throw new NotImplementedException();
	public void Assign<TInputIterator>(TInputIterator first, TInputIterator last) where TInputIterator : IReadOnlyStreamIterator<TArg> => throw new NotImplementedException();
	public ref TArg At(ulong index) => throw new NotImplementedException();
	public TArg GetAt(ulong index) => throw new NotImplementedException();
	public TArg GetBack() => throw new NotImplementedException();
	public TArg GetFront() => throw new NotImplementedException();
	public void SetAt(ulong index, TArg value) => throw new NotImplementedException();
	public void SetBack(TArg value) => throw new NotImplementedException();
	public void SetFront(TArg value) => throw new NotImplementedException();
	public bool TryAssign(TArg value, ulong count) => throw new NotImplementedException();
	public ulong TryAssign<TInputIterator>(TInputIterator first, TInputIterator last) where TInputIterator : IReadOnlyStreamIterator<TArg> => throw new NotImplementedException();
	public TArg TryGetAt(ulong index) => throw new NotImplementedException();
	public TArg TryGetBack() => throw new NotImplementedException();
	public TArg TryGetFront() => throw new NotImplementedException();
	public bool TrySetAt(ulong index, TArg value) => throw new NotImplementedException();
	public bool TrySetBack(TArg value) => throw new NotImplementedException();
	public bool TrySetFront(TArg value) => throw new NotImplementedException();

}
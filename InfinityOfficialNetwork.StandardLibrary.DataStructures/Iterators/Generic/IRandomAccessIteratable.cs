namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators.Generic;

public interface IRandomAccessIteratable<TArg> : IBidirectionalIteratable<TArg>
{
	new public IRandomAccessIterator<TArg> Begin { get; }
	new public IRandomAccessIterator<TArg> End { get; }

	new public IRandomAccessIterator<TArg> ReverseBegin { get; }
	new public IRandomAccessIterator<TArg> ReverseEnd { get; }

	IBidirectionalIterator<TArg> IBidirectionalIteratable<TArg>.Begin => Begin;
	IBidirectionalIterator<TArg> IBidirectionalIteratable<TArg>.End => End;

	IBidirectionalIterator<TArg> IBidirectionalIteratable<TArg>.ReverseBegin => ReverseBegin;
	IBidirectionalIterator<TArg> IBidirectionalIteratable<TArg>.ReverseEnd => ReverseEnd;
}

namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators.Generic;

public interface IBidirectionalIteratable<TArg> : IForwardIteratable<TArg>
{
	new public IBidirectionalIterator<TArg> Begin { get; }
	new public IBidirectionalIterator<TArg> End { get; }

	new public IBidirectionalIterator<TArg> ReverseBegin { get; }
	new public IBidirectionalIterator<TArg> ReverseEnd { get; }

	IForwardIterator<TArg> IForwardIteratable<TArg>.Begin => Begin;
	IForwardIterator<TArg> IForwardIteratable<TArg>.End => End;
}

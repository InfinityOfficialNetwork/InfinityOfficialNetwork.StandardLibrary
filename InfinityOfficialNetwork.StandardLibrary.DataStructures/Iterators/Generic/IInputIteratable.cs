using System.Collections;

namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators.Generic;

public interface IInputIteratable<TArg> : IIteratable<TArg>, IEnumerable<TArg>
{
	new public IInputIterator<TArg> Begin { get; }
	new public IInputIterator<TArg> End { get; }

	IIterator<TArg> IIteratable<TArg>.Begin => Begin;
	IIterator<TArg> IIteratable<TArg>.End => End;

	IEnumerator<TArg> IEnumerable<TArg>.GetEnumerator() => Begin.ToEnumerator(End);
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

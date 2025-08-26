namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators.Generic;

public interface IOutputIteratable<TArg> : IIteratable<TArg>
{
	new public IOutputIterator<TArg> Begin { get; }
	new public IOutputIterator<TArg> End { get; }

	IIterator<TArg> IIteratable<TArg>.Begin => Begin;
	IIterator<TArg> IIteratable<TArg>.End => End;
}

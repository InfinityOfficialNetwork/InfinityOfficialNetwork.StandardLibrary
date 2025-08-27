namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators.Generic;

public interface IForwardIteratable<TArg> : IOutputIteratable<TArg>, IInputIteratable<TArg>
{
	new public IForwardIterator<TArg> Begin { get; }
	new public IForwardIterator<TArg> End { get; }

	IOutputIterator<TArg> IOutputIteratable<TArg>.Begin => Begin;
	IOutputIterator<TArg> IOutputIteratable<TArg>.End => End;

	IInputIterator<TArg> IInputIteratable<TArg>.Begin => Begin;
	IInputIterator<TArg> IInputIteratable<TArg>.End => End;

	IIterator<TArg> IIteratable<TArg>.Begin => Begin;
	IIterator<TArg> IIteratable<TArg>.End => End;
}

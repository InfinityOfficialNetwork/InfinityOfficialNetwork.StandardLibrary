namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators.Generic;

public interface IBidirectionalIterator<T> : IForwardIterator<T>
{
	public void MovePrevious();
}

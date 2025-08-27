namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators.Generic;

public interface IRandomAccessIterator<T> : IBidirectionalIterator<T>
{
	public void MoveNext(int count);
	public void MovePrevious(int count);

	public T this[int index] { get; set; }

}

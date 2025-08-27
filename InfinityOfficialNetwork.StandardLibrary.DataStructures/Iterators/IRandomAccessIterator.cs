namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators;

public interface IRandomAccessIterator : IBidirectionalIterator
{
	bool MoveNext(int count);
	bool MovePrevious(int count);

	public object? this[int index] { get; }
}

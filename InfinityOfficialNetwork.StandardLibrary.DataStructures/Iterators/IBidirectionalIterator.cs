namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators;

public interface IBidirectionalIterator : IForwardIterator, IDisposable
{
	new public bool MovePrevious();
}

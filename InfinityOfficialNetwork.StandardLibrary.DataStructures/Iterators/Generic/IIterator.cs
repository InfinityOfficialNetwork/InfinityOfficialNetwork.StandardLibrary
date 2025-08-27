namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators.Generic
{
	public interface IIterator<T> : IDisposable
	{
		public bool NotEquals(IIterator<T> other);

	}
}

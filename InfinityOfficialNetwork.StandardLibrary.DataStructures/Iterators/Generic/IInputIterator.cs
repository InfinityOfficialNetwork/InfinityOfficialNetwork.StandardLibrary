namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators.Generic
{
	public interface IInputIterator<T> : IIterator<T>
	{
		public void MoveNext();
		public T Current { get; }
	}
}

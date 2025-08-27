namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators.Generic
{
	public interface IOutputIterator<T> : IIterator<T>
	{
		public void MoveNext();
		public T Current { set; }

	}
}

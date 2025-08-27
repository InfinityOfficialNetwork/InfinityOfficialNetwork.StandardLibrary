namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators.Generic;

public interface IForwardIterator<T> : IInputIterator<T>, IOutputIterator<T>
{
	new public T Current { get; set; }
	new public void MoveNext();

	T IInputIterator<T>.Current => Current;

	T IOutputIterator<T>.Current { set => Current = value; }

	void IInputIterator<T>.MoveNext() => MoveNext();
	void IOutputIterator<T>.MoveNext() => MoveNext();
}

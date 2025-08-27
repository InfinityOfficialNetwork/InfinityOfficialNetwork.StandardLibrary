using System.Collections;

namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators
{
	public interface IForwardIterator : IEnumerator, IDisposable
	{
		new public object? Current { get; set; }
		new public bool MoveNext();
		new public void Reset();


		object? IEnumerator.Current => Current;
		bool IEnumerator.MoveNext() => MoveNext();
		void IEnumerator.Reset() => Reset();
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators.Generic;

public interface IRandomAccessIterator<T> : IBidirectionalIterator<T>
{
	public void MoveNext(int count);
	public void MovePrevious(int count);

	public T this[int index] { get; set; }

}

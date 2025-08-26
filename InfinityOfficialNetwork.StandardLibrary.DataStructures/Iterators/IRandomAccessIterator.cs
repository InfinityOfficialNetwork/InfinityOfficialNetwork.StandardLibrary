using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators;

public interface IRandomAccessIterator : IBidirectionalIterator
{
	bool MoveNext(int count);
	bool MovePrevious(int count);

	public object? this[int index] { get; }
}

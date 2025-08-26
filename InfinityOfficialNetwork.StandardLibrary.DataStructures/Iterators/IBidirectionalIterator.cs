using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators;

public interface IBidirectionalIterator : IForwardIterator, IDisposable
{
	new public bool MovePrevious();
}

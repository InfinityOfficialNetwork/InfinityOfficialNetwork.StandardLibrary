using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators.Generic;

public interface IIteratable<TArg> : IDisposable
{
	public IIterator<TArg> Begin { get; }
	public IIterator<TArg> End { get; }
}

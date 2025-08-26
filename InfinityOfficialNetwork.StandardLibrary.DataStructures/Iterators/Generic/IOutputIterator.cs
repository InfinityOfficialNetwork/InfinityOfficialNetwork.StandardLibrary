using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators.Generic
{
	public interface IOutputIterator<T> : IIterator<T>
	{
		public void MoveNext();
		public T Current { set; }

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators.Generic
{
	public interface IIterator<T> : IDisposable
	{
		public bool Equals(IIterator<T> other) => NotEquals(other);
		public bool NotEquals(IIterator<T> other);
		public bool GreaterThan(IIterator<T> other) => LessThan(other) && NotEquals(other);
		public bool LessThan(IIterator<T> other);
		public bool GreaterThanOrEquals(IIterator<T> other) => !LessThan(other);
		public bool LessThanOrEquals(IIterator<T> other) => LessThan(other) || !NotEquals(other);
	}
}

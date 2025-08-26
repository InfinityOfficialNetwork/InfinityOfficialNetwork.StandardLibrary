using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators.Generic
{
	public static class IteratorExtensions
	{
		private struct InputIteratorEnumerator<T> : IEnumerator<T>, IEnumerator
		{
			public InputIteratorEnumerator(IInputIterator<T> begin, IInputIterator<T> end)
			{
				current = begin;
				this.end = end;
			}

			private IInputIterator<T> current, end;

			T IEnumerator<T>.Current => current.Current;

			object? IEnumerator.Current => current.Current;

			void IDisposable.Dispose()
			{
				current.Dispose();
				end.Dispose();
			}

			bool IEnumerator.MoveNext()
			{
				current.MoveNext();
				return current.NotEquals(end);
			}

			void IEnumerator.Reset()
			{
				throw new InvalidOperationException("InputIterator cannot be reset");
			}
		}

		private struct ForwardIteratorEnumerator<T> : IEnumerator<T>, IEnumerator
		{
			public ForwardIteratorEnumerator(IForwardIterator<T> begin, IForwardIterator<T> end)
			{
				this.begin = current = begin;
				this.end = end;
			}

			private IForwardIterator<T> begin, current, end;

			T IEnumerator<T>.Current => current.Current;

			object? IEnumerator.Current => current.Current;

			void IDisposable.Dispose()
			{
				current.Dispose();
				end.Dispose();
			}

			bool IEnumerator.MoveNext()
			{
				current.MoveNext();
				return current.NotEquals(end);
			}

			void IEnumerator.Reset()
			{
				current = begin;
			}
		}

		public static IEnumerator<T> ToEnumerator<T>(this IInputIterator<T> begin, IInputIterator<T> end)
		{
			return new InputIteratorEnumerator<T>(begin, end);
		}

		public static IEnumerator<T> ToEnumerator<T>(this IForwardIterator<T> begin, IForwardIterator<T> end)
		{
			return new ForwardIteratorEnumerator<T>(begin, end);
		}
	}
}

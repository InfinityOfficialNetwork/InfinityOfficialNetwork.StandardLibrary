using InfinityOfficialNetwork.StandardLibrary.Common;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using InfinityOfficialNetwork.StandardLibrary.Iterators.Dispatching.Iterators;
using InfinityOfficialNetwork.StandardLibrary.Iterators.Dispatching.Iterations;
using InfinityOfficialNetwork.StandardLibrary.Iterators.Operations;
using InfinityOfficialNetwork.StandardLibrary.Iterators.Tiers.Base;
using InfinityOfficialNetwork.StandardLibrary.Iterators.Tiers.Levels;

namespace InfinityOfficialNetwork.StandardLibrary.Iterators;






public interface IIterator
{
	private ref struct ReadOnlyIterationAdapter<TArg, TIterator, TIterationVisitor> : IReadOnlyIteratorVisitor<TArg>
	where TArg : allows ref struct
	where TIterator : IReadOnlyIterator<TArg>, allows ref struct
	where TIterationVisitor : IReadOnlyIterationVisitor<TArg>, allows ref struct
	{
		private TIterator _end;
		private TIterationVisitor _visitor;

		public ReadOnlyIterationAdapter(TIterator end, TIterationVisitor visitor)
		{
			_end = end;
			_visitor = visitor;
		}

		public void VisitReadOnlyStreamIterator<TIteratorActual>(TIteratorActual iterator)
			where TIteratorActual : IReadOnlyStreamIterator<TArg>, allows ref struct
		{
			// Since TIteratorActual is dynamically resolved to the exact concrete type of TConstrainedIterator,
			// we can safely cast our _end (which is of type TConstrainedIterator) to TIteratorActual without boxing.
			ref TIteratorActual endActual = ref Unsafe.As<TIterator, TIteratorActual>(ref _end);
			_visitor.VisitReadOnlyStreamIterator(iterator, endActual);
		}

		public void VisitReadOnlyForwardIterator<TIteratorActual>(TIteratorActual iterator)
			where TIteratorActual : IReadOnlyForwardIterator<TArg>, allows ref struct
		{
			ref TIteratorActual endActual = ref Unsafe.As<TIterator, TIteratorActual>(ref _end);
			_visitor.VisitReadOnlyForwardIterator(iterator, endActual);
		}

		public void VisitReadOnlyBidirectionalIterator<TIteratorActual>(TIteratorActual iterator)
			where TIteratorActual : IReadOnlyBidirectionalIterator<TArg>, allows ref struct
		{
			ref TIteratorActual endActual = ref Unsafe.As<TIterator, TIteratorActual>(ref _end);
			_visitor.VisitReadOnlyBidirectionalIterator(iterator, endActual);
		}

		public void VisitReadOnlyRandomAccessIterator<TIteratorActual>(TIteratorActual iterator)
			where TIteratorActual : IReadOnlyRandomAccessIterator<TArg>, allows ref struct
		{
			ref TIteratorActual endActual = ref Unsafe.As<TIterator, TIteratorActual>(ref _end);
			_visitor.VisitReadOnlyRandomAccessIterator(iterator, endActual);
		}
	}


	public static void ReadOnlyIterateOver<TArg, TIterator, TIterationVisitor>(TIterator begin, TIterator end, TIterationVisitor visitor)
		where TArg : allows ref struct
		where TIterator : IReadOnlyIterator<TArg>, allows ref struct
		where TIterationVisitor : IReadOnlyIterationVisitor<TArg>, allows ref struct
	{
		var adapter = new ReadOnlyIterationAdapter<TArg, TIterator, TIterationVisitor>(end, visitor);

		begin.ReadOnlyAccept(adapter);
	}
}







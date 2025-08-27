using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators;

namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators;

public interface IIteratable<TArg> : IDisposable
{
	public IIterator<TArg> Begin { get; }
	public IIterator<TArg> End { get; }
}

public interface IInputIteratable<TArg> : IIteratable<TArg>
{
	new public IInputIterator<TArg> Begin { get; }
	new public IInputIterator<TArg> End { get; }

	IIterator<TArg> IIteratable<TArg>.Begin { get => Begin; }
	IIterator<TArg> IIteratable<TArg>.End { get => End; }
}

public interface IOutputIteratable<TArg> : IIteratable<TArg>
{
	new public IOutputIterator<TArg> Begin { get; }
	new public IOutputIterator<TArg> End { get; }

	IIterator<TArg> IIteratable<TArg>.Begin { get => Begin; }
	IIterator<TArg> IIteratable<TArg>.End { get => End; }
}

public interface IStreamingInputIteratable<TArg> : IInputIteratable<TArg>
{
	new public IStreamingInputIterator<TArg> Begin { get; }
	new public IStreamingInputIterator<TArg> End { get; }

	IInputIterator<TArg> IInputIteratable<TArg>.Begin { get => Begin; }
	IInputIterator<TArg> IInputIteratable<TArg>.End { get => End; }
}

public interface IStreamingOutputIteratable<TArg> : IOutputIteratable<TArg>
{
	new public IStreamingOutputIterator<TArg> Begin { get; }
	new public IStreamingOutputIterator<TArg> End { get; }

	IOutputIterator<TArg> IOutputIteratable<TArg>.Begin { get => Begin; }
	IOutputIterator<TArg> IOutputIteratable<TArg>.End { get => End; }
}

public interface IStreamingIteratable<TArg> : IStreamingInputIteratable<TArg>, IStreamingOutputIteratable<TArg>
{
	new public IStreamingIterator<TArg> Begin { get; }
	new public IStreamingIterator<TArg> End { get; }
	IIterator<TArg> IIteratable<TArg>.Begin { get => Begin; }
	IIterator<TArg> IIteratable<TArg>.End { get => End; }
	IInputIterator<TArg> IInputIteratable<TArg>.Begin { get => Begin; }
	IInputIterator<TArg> IInputIteratable<TArg>.End { get => End; }
	IOutputIterator<TArg> IOutputIteratable<TArg>.Begin { get => Begin; }
	IOutputIterator<TArg> IOutputIteratable<TArg>.End { get => End; }
	IStreamingInputIterator<TArg> IStreamingInputIteratable<TArg>.Begin { get => Begin; }
	IStreamingInputIterator<TArg> IStreamingInputIteratable<TArg>.End { get => End; }
	IStreamingOutputIterator<TArg> IStreamingOutputIteratable<TArg>.Begin { get => Begin; }
	IStreamingOutputIterator<TArg> IStreamingOutputIteratable<TArg>.End { get => End; }
}


public interface IForwardInputIteratable<TArg> : IStreamingInputIteratable<TArg>
{
	new public IForwardInputIterator<TArg> Begin { get; }
	new public IForwardInputIterator<TArg> End { get; }

	IIterator<TArg> IIteratable<TArg>.Begin { get => Begin; }
	IIterator<TArg> IIteratable<TArg>.End { get => End; }
	IInputIterator<TArg> IInputIteratable<TArg>.Begin { get => Begin; }
	IInputIterator<TArg> IInputIteratable<TArg>.End { get => End; }
	IStreamingInputIterator<TArg> IStreamingInputIteratable<TArg>.Begin { get => Begin; }
	IStreamingInputIterator<TArg> IStreamingInputIteratable<TArg>.End { get => End; }
}

public interface IForwardOutputIteratable<TArg> : IStreamingOutputIteratable<TArg>
{
	new public IForwardOutputIterator<TArg> Begin { get; }
	new public IForwardOutputIterator<TArg> End { get; }

	IIterator<TArg> IIteratable<TArg>.Begin { get => Begin; }
	IIterator<TArg> IIteratable<TArg>.End { get => End; }
	IOutputIterator<TArg> IOutputIteratable<TArg>.Begin { get => Begin; }
	IOutputIterator<TArg> IOutputIteratable<TArg>.End { get => End; }
	IStreamingOutputIterator<TArg> IStreamingOutputIteratable<TArg>.Begin { get => Begin; }
	IStreamingOutputIterator<TArg> IStreamingOutputIteratable<TArg>.End { get => End; }
}

public interface IForwardIteratable<TArg> : IForwardInputIteratable<TArg>, IForwardOutputIteratable<TArg>, IStreamingIteratable<TArg>
{
	new public IForwardIterator<TArg> Begin { get; }
	new public IForwardIterator<TArg> End { get; }
	IIterator<TArg> IIteratable<TArg>.Begin { get => Begin; }
	IIterator<TArg> IIteratable<TArg>.End { get => End; }
	IInputIterator<TArg> IInputIteratable<TArg>.Begin { get => Begin; }
	IInputIterator<TArg> IInputIteratable<TArg>.End { get => End; }
	IOutputIterator<TArg> IOutputIteratable<TArg>.Begin { get => Begin; }
	IOutputIterator<TArg> IOutputIteratable<TArg>.End { get => End; }
	IStreamingInputIterator<TArg> IStreamingInputIteratable<TArg>.Begin { get => Begin; }
	IStreamingInputIterator<TArg> IStreamingInputIteratable<TArg>.End { get => End; }
	IStreamingOutputIterator<TArg> IStreamingOutputIteratable<TArg>.Begin { get => Begin; }
	IStreamingOutputIterator<TArg> IStreamingOutputIteratable<TArg>.End { get => End; }
	IForwardInputIterator<TArg> IForwardInputIteratable<TArg>.Begin { get => Begin; }
	IForwardInputIterator<TArg> IForwardInputIteratable<TArg>.End { get => End; }
	IForwardOutputIterator<TArg> IForwardOutputIteratable<TArg>.Begin { get => Begin; }
	IForwardOutputIterator<TArg> IForwardOutputIteratable<TArg>.End { get => End; }
	IStreamingIterator<TArg> IStreamingIteratable<TArg>.Begin { get => Begin; }
	IStreamingIterator<TArg> IStreamingIteratable<TArg>.End { get => End; }
}


public interface IBidirectionalInputIteratable<TArg> : IForwardInputIteratable<TArg>
{
	new public IBidirectionalInputIterator<TArg> Begin { get; }
	new public IBidirectionalInputIterator<TArg> End { get; }

	IIterator<TArg> IIteratable<TArg>.Begin { get => Begin; }
	IIterator<TArg> IIteratable<TArg>.End { get => End; }
	IInputIterator<TArg> IInputIteratable<TArg>.Begin { get => Begin; }
	IInputIterator<TArg> IInputIteratable<TArg>.End { get => End; }
	IStreamingInputIterator<TArg> IStreamingInputIteratable<TArg>.Begin { get => Begin; }
	IStreamingInputIterator<TArg> IStreamingInputIteratable<TArg>.End { get => End; }
	IForwardInputIterator<TArg> IForwardInputIteratable<TArg>.Begin { get => Begin; }
	IForwardInputIterator<TArg> IForwardInputIteratable<TArg>.End { get => End; }
}

public interface IBidirectionalOutputIteratable<TArg> : IForwardOutputIteratable<TArg>
{
	new public IBidirectionalOutputIterator<TArg> Begin { get; }
	new public IBidirectionalOutputIterator<TArg> End { get; }

	IIterator<TArg> IIteratable<TArg>.Begin { get => Begin; }
	IIterator<TArg> IIteratable<TArg>.End { get => End; }
	IOutputIterator<TArg> IOutputIteratable<TArg>.Begin { get => Begin; }
	IOutputIterator<TArg> IOutputIteratable<TArg>.End { get => End; }
	IStreamingOutputIterator<TArg> IStreamingOutputIteratable<TArg>.Begin { get => Begin; }
	IStreamingOutputIterator<TArg> IStreamingOutputIteratable<TArg>.End { get => End; }
	IForwardOutputIterator<TArg> IForwardOutputIteratable<TArg>.Begin { get => Begin; }
	IForwardOutputIterator<TArg> IForwardOutputIteratable<TArg>.End { get => End; }
}

public interface IBidirectionalIteratable<TArg> : IBidirectionalInputIteratable<TArg>, IBidirectionalOutputIteratable<TArg>, IForwardIteratable<TArg>
{
	new public IBidirectionalIterator<TArg> Begin { get; }
	new public IBidirectionalIterator<TArg> End { get; }

	IIterator<TArg> IIteratable<TArg>.Begin { get => Begin; }
	IIterator<TArg> IIteratable<TArg>.End { get => End; }
	IInputIterator<TArg> IInputIteratable<TArg>.Begin { get => Begin; }
	IInputIterator<TArg> IInputIteratable<TArg>.End { get => End; }
	IOutputIterator<TArg> IOutputIteratable<TArg>.Begin { get => Begin; }
	IOutputIterator<TArg> IOutputIteratable<TArg>.End { get => End; }
	IStreamingInputIterator<TArg> IStreamingInputIteratable<TArg>.Begin { get => Begin; }
	IStreamingInputIterator<TArg> IStreamingInputIteratable<TArg>.End { get => End; }
	IStreamingOutputIterator<TArg> IStreamingOutputIteratable<TArg>.Begin { get => Begin; }
	IStreamingOutputIterator<TArg> IStreamingOutputIteratable<TArg>.End { get => End; }
	IForwardInputIterator<TArg> IForwardInputIteratable<TArg>.Begin { get => Begin; }
	IForwardInputIterator<TArg> IForwardInputIteratable<TArg>.End { get => End; }
	IForwardOutputIterator<TArg> IForwardOutputIteratable<TArg>.Begin { get => Begin; }
	IForwardOutputIterator<TArg> IForwardOutputIteratable<TArg>.End { get => End; }
	IStreamingIterator<TArg> IStreamingIteratable<TArg>.Begin { get => Begin; }
	IStreamingIterator<TArg> IStreamingIteratable<TArg>.End { get => End; }
	IBidirectionalInputIterator<TArg> IBidirectionalInputIteratable<TArg>.Begin => Begin;
	IBidirectionalInputIterator<TArg> IBidirectionalInputIteratable<TArg>.End => End;
	IBidirectionalOutputIterator<TArg> IBidirectionalOutputIteratable<TArg>.Begin => Begin;
	IBidirectionalOutputIterator<TArg> IBidirectionalOutputIteratable<TArg>.End => End;
	IForwardIterator<TArg> IForwardIteratable<TArg>.Begin => Begin;
	IForwardIterator<TArg> IForwardIteratable<TArg>.End => End;
}


public interface IRandomAccessInputIteratable<TArg> : IBidirectionalInputIteratable<TArg>
{
	new public IRandomAccessInputIterator<TArg> Begin { get; }
	new public IRandomAccessInputIterator<TArg> End { get; }

	IIterator<TArg> IIteratable<TArg>.Begin { get => Begin; }
	IIterator<TArg> IIteratable<TArg>.End { get => End; }
	IInputIterator<TArg> IInputIteratable<TArg>.Begin { get => Begin; }
	IInputIterator<TArg> IInputIteratable<TArg>.End { get => End; }
	IStreamingInputIterator<TArg> IStreamingInputIteratable<TArg>.Begin { get => Begin; }
	IStreamingInputIterator<TArg> IStreamingInputIteratable<TArg>.End { get => End; }
	IForwardInputIterator<TArg> IForwardInputIteratable<TArg>.Begin { get => Begin; }
	IForwardInputIterator<TArg> IForwardInputIteratable<TArg>.End { get => End; }
	IBidirectionalInputIterator<TArg> IBidirectionalInputIteratable<TArg>.Begin { get => Begin; }
	IBidirectionalInputIterator<TArg> IBidirectionalInputIteratable<TArg>.End { get => End; }
}

public interface IRandomAccessOutputIteratable<TArg> : IBidirectionalOutputIteratable<TArg>
{
	new public IRandomAccessOutputIterator<TArg> Begin { get; }
	new public IRandomAccessOutputIterator<TArg> End { get; }

	IIterator<TArg> IIteratable<TArg>.Begin { get => Begin; }
	IIterator<TArg> IIteratable<TArg>.End { get => End; }
	IOutputIterator<TArg> IOutputIteratable<TArg>.Begin { get => Begin; }
	IOutputIterator<TArg> IOutputIteratable<TArg>.End { get => End; }
	IStreamingOutputIterator<TArg> IStreamingOutputIteratable<TArg>.Begin { get => Begin; }
	IStreamingOutputIterator<TArg> IStreamingOutputIteratable<TArg>.End { get => End; }
	IForwardOutputIterator<TArg> IForwardOutputIteratable<TArg>.Begin { get => Begin; }
	IForwardOutputIterator<TArg> IForwardOutputIteratable<TArg>.End { get => End; }
	IBidirectionalOutputIterator<TArg> IBidirectionalOutputIteratable<TArg>.Begin { get => Begin; }
	IBidirectionalOutputIterator<TArg> IBidirectionalOutputIteratable<TArg>.End { get => End; }
}

public interface IRandomAccessIteratable<TArg> : IRandomAccessInputIteratable<TArg>, IRandomAccessOutputIteratable<TArg>, IBidirectionalIteratable<TArg>
{
	new public IRandomAccessIterator<TArg> Begin { get; }
	new public IRandomAccessIterator<TArg> End { get; }

	IIterator<TArg> IIteratable<TArg>.Begin { get => Begin; }
	IIterator<TArg> IIteratable<TArg>.End { get => End; }
	IInputIterator<TArg> IInputIteratable<TArg>.Begin { get => Begin; }
	IInputIterator<TArg> IInputIteratable<TArg>.End { get => End; }
	IOutputIterator<TArg> IOutputIteratable<TArg>.Begin { get => Begin; }
	IOutputIterator<TArg> IOutputIteratable<TArg>.End { get => End; }
	IStreamingInputIterator<TArg> IStreamingInputIteratable<TArg>.Begin { get => Begin; }
	IStreamingInputIterator<TArg> IStreamingInputIteratable<TArg>.End { get => End; }
	IStreamingOutputIterator<TArg> IStreamingOutputIteratable<TArg>.Begin { get => Begin; }
	IStreamingOutputIterator<TArg> IStreamingOutputIteratable<TArg>.End { get => End; }
	IForwardInputIterator<TArg> IForwardInputIteratable<TArg>.Begin { get => Begin; }
	IForwardInputIterator<TArg> IForwardInputIteratable<TArg>.End { get => End; }
	IForwardOutputIterator<TArg> IForwardOutputIteratable<TArg>.Begin { get => Begin; }
	IForwardOutputIterator<TArg> IForwardOutputIteratable<TArg>.End { get => End; }
	IStreamingIterator<TArg> IStreamingIteratable<TArg>.Begin { get => Begin; }
	IStreamingIterator<TArg> IStreamingIteratable<TArg>.End { get => End; }
	IBidirectionalInputIterator<TArg> IBidirectionalInputIteratable<TArg>.Begin => Begin;
	IBidirectionalInputIterator<TArg> IBidirectionalInputIteratable<TArg>.End => End;
	IBidirectionalOutputIterator<TArg> IBidirectionalOutputIteratable<TArg>.Begin => Begin;
	IBidirectionalOutputIterator<TArg> IBidirectionalOutputIteratable<TArg>.End => End;
	IForwardIterator<TArg> IForwardIteratable<TArg>.Begin => Begin;
	IForwardIterator<TArg> IForwardIteratable<TArg>.End => End;
	IRandomAccessInputIterator<TArg> IRandomAccessInputIteratable<TArg>.Begin { get => Begin; }
	IRandomAccessInputIterator<TArg> IRandomAccessInputIteratable<TArg>.End { get => End; }
	IRandomAccessOutputIterator<TArg> IRandomAccessOutputIteratable<TArg>.Begin { get => Begin; }
	IRandomAccessOutputIterator<TArg> IRandomAccessOutputIteratable<TArg>.End { get => End; }
	IBidirectionalIterator<TArg> IBidirectionalIteratable<TArg>.Begin { get => Begin; }
	IBidirectionalIterator<TArg> IBidirectionalIteratable<TArg>.End { get => End; }
}


public interface IContiguousInputIteratable<TArg> : IRandomAccessInputIteratable<TArg>
{
	new public IContiguousInputIterator<TArg> Begin { get; }
	new public IContiguousInputIterator<TArg> End { get; }

	IIterator<TArg> IIteratable<TArg>.Begin { get => Begin; }
	IIterator<TArg> IIteratable<TArg>.End { get => End; }
	IInputIterator<TArg> IInputIteratable<TArg>.Begin { get => Begin; }
	IInputIterator<TArg> IInputIteratable<TArg>.End { get => End; }
	IStreamingInputIterator<TArg> IStreamingInputIteratable<TArg>.Begin { get => Begin; }
	IStreamingInputIterator<TArg> IStreamingInputIteratable<TArg>.End { get => End; }
	IForwardInputIterator<TArg> IForwardInputIteratable<TArg>.Begin { get => Begin; }
	IForwardInputIterator<TArg> IForwardInputIteratable<TArg>.End { get => End; }
	IBidirectionalInputIterator<TArg> IBidirectionalInputIteratable<TArg>.Begin { get => Begin; }
	IBidirectionalInputIterator<TArg> IBidirectionalInputIteratable<TArg>.End { get => End; }
	IRandomAccessInputIterator<TArg> IRandomAccessInputIteratable<TArg>.Begin { get => Begin; }
	IRandomAccessInputIterator<TArg> IRandomAccessInputIteratable<TArg>.End { get => End; }
}

public interface IContiguousOutputIteratable<TArg> : IRandomAccessOutputIteratable<TArg>
{
	new public IContiguousOutputIterator<TArg> Begin { get; }
	new public IContiguousOutputIterator<TArg> End { get; }

	IIterator<TArg> IIteratable<TArg>.Begin { get => Begin; }
	IIterator<TArg> IIteratable<TArg>.End { get => End; }
	IOutputIterator<TArg> IOutputIteratable<TArg>.Begin { get => Begin; }
	IOutputIterator<TArg> IOutputIteratable<TArg>.End { get => End; }
	IStreamingOutputIterator<TArg> IStreamingOutputIteratable<TArg>.Begin { get => Begin; }
	IStreamingOutputIterator<TArg> IStreamingOutputIteratable<TArg>.End { get => End; }
	IForwardOutputIterator<TArg> IForwardOutputIteratable<TArg>.Begin { get => Begin; }
	IForwardOutputIterator<TArg> IForwardOutputIteratable<TArg>.End { get => End; }
	IBidirectionalOutputIterator<TArg> IBidirectionalOutputIteratable<TArg>.Begin { get => Begin; }
	IBidirectionalOutputIterator<TArg> IBidirectionalOutputIteratable<TArg>.End { get => End; }
	IRandomAccessOutputIterator<TArg> IRandomAccessOutputIteratable<TArg>.Begin { get => Begin; }
	IRandomAccessOutputIterator<TArg> IRandomAccessOutputIteratable<TArg>.End { get => End; }
}

public interface IContiguousIteratable<TArg> : IRandomAccessIteratable<TArg>, IContiguousInputIteratable<TArg>, IContiguousOutputIteratable<TArg>
{
	new public IContiguousIterator<TArg> Begin { get; }
	new public IContiguousIterator<TArg> End { get; }

	IIterator<TArg> IIteratable<TArg>.Begin { get => Begin; }
	IIterator<TArg> IIteratable<TArg>.End { get => End; }
	IInputIterator<TArg> IInputIteratable<TArg>.Begin { get => Begin; }
	IInputIterator<TArg> IInputIteratable<TArg>.End { get => End; }
	IOutputIterator<TArg> IOutputIteratable<TArg>.Begin { get => Begin; }
	IOutputIterator<TArg> IOutputIteratable<TArg>.End { get => End; }
	IStreamingInputIterator<TArg> IStreamingInputIteratable<TArg>.Begin { get => Begin; }
	IStreamingInputIterator<TArg> IStreamingInputIteratable<TArg>.End { get => End; }
	IStreamingOutputIterator<TArg> IStreamingOutputIteratable<TArg>.Begin { get => Begin; }
	IStreamingOutputIterator<TArg> IStreamingOutputIteratable<TArg>.End { get => End; }
	IForwardInputIterator<TArg> IForwardInputIteratable<TArg>.Begin { get => Begin; }
	IForwardInputIterator<TArg> IForwardInputIteratable<TArg>.End { get => End; }
	IForwardOutputIterator<TArg> IForwardOutputIteratable<TArg>.Begin { get => Begin; }
	IForwardOutputIterator<TArg> IForwardOutputIteratable<TArg>.End { get => End; }
	IStreamingIterator<TArg> IStreamingIteratable<TArg>.Begin { get => Begin; }
	IStreamingIterator<TArg> IStreamingIteratable<TArg>.End { get => End; }
	IBidirectionalInputIterator<TArg> IBidirectionalInputIteratable<TArg>.Begin => Begin;
	IBidirectionalInputIterator<TArg> IBidirectionalInputIteratable<TArg>.End => End;
	IBidirectionalOutputIterator<TArg> IBidirectionalOutputIteratable<TArg>.Begin => Begin;
	IBidirectionalOutputIterator<TArg> IBidirectionalOutputIteratable<TArg>.End => End;
	IForwardIterator<TArg> IForwardIteratable<TArg>.Begin => Begin;
	IForwardIterator<TArg> IForwardIteratable<TArg>.End => End;
	IRandomAccessInputIterator<TArg> IRandomAccessInputIteratable<TArg>.Begin { get => Begin; }
	IRandomAccessInputIterator<TArg> IRandomAccessInputIteratable<TArg>.End { get => End; }
	IRandomAccessOutputIterator<TArg> IRandomAccessOutputIteratable<TArg>.Begin { get => Begin; }
	IRandomAccessOutputIterator<TArg> IRandomAccessOutputIteratable<TArg>.End { get => End; }
	IBidirectionalIterator<TArg> IBidirectionalIteratable<TArg>.Begin { get => Begin; }
	IBidirectionalIterator<TArg> IBidirectionalIteratable<TArg>.End { get => End; }
	IContiguousInputIterator<TArg> IContiguousInputIteratable<TArg>.Begin { get => Begin; }
	IContiguousInputIterator<TArg> IContiguousInputIteratable<TArg>.End { get => End; }
	IContiguousOutputIterator<TArg> IContiguousOutputIteratable<TArg>.Begin { get => Begin; }
	IContiguousOutputIterator<TArg> IContiguousOutputIteratable<TArg>.End { get => End; }
	IRandomAccessIterator<TArg> IRandomAccessIteratable<TArg>.Begin { get => Begin; }
	IRandomAccessIterator<TArg> IRandomAccessIteratable<TArg>.End { get => End; }
}
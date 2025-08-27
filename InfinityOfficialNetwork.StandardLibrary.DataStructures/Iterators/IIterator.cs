namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators;

public interface IIterator<TArg> : IDisposable, IEquatable<IIterator<TArg>>
{
	public void MoveNext();
}

public interface IInputIterator<TArg> : IIterator<TArg>
{
	public TArg Current { get; }
}

public interface IOutputIterator<TArg> : IIterator<TArg>
{
	public TArg Current { set; }
}

public interface IStreamingInputIterator<TArg> : IInputIterator<TArg>
{
	new public TArg Current { get; }
	TArg IInputIterator<TArg>.Current => Current;
}

public interface IStreamingOutputIterator<TArg> : IOutputIterator<TArg>
{
	new public TArg Current { set; }
	TArg IOutputIterator<TArg>.Current { set => Current = value; }
}

public interface IStreamingIterator<TArg> : IStreamingInputIterator<TArg>, IStreamingOutputIterator<TArg>
{ 
	new public TArg Current { get; set; }
	TArg IInputIterator<TArg>.Current => Current;
	TArg IOutputIterator<TArg>.Current { set =>  Current = value; }
	TArg IStreamingInputIterator<TArg>.Current => Current;
	TArg IStreamingOutputIterator<TArg>.Current { set => Current = value; }
}


public interface IForwardInputIterator<TArg> : IStreamingInputIterator<TArg>, ICloneable
{
	new public TArg Current { get; }
	TArg IInputIterator<TArg>.Current => Current;
	TArg IStreamingInputIterator<TArg>.Current => Current;
}

public interface IForwardOutputIterator<TArg> : IStreamingOutputIterator<TArg>, ICloneable
{
	new public TArg Current { set; }
	TArg IOutputIterator<TArg>.Current { set => Current = value; }
	TArg IStreamingOutputIterator<TArg>.Current { set => Current = value; }
}

public interface IForwardIterator<TArg> : IForwardInputIterator<TArg>, IForwardOutputIterator<TArg>, IStreamingIterator<TArg>, ICloneable
{
	new public ref TArg Current { get; }
	TArg IInputIterator<TArg>.Current => Current;
	TArg IOutputIterator<TArg>.Current { set => Current = value; }
	TArg IStreamingInputIterator<TArg>.Current => Current;
	TArg IStreamingOutputIterator<TArg>.Current { set => Current = value; }
	TArg IForwardInputIterator<TArg>.Current => Current;
	TArg IForwardOutputIterator<TArg>.Current { set => Current = value; }
	TArg IStreamingIterator<TArg>.Current { get => Current; set => Current = value; }
}


public interface IBidirectionalInputIterator<TArg> : IForwardInputIterator<TArg>
{
	public void MovePrevious();

	new public TArg Current { get; }
	TArg IInputIterator<TArg>.Current => Current;
	TArg IStreamingInputIterator<TArg>.Current => Current;
	TArg IForwardInputIterator<TArg>.Current => Current;
}

public interface IBidirectionalOutputIterator<TArg> : IForwardOutputIterator<TArg>
{
	public void MovePrevious();

	new public TArg Current { set; }
	TArg IOutputIterator<TArg>.Current { set => Current = value; }
	TArg IStreamingOutputIterator<TArg>.Current { set => Current = value; }
	TArg IForwardOutputIterator<TArg>.Current { set => Current = value; }
}

public interface IBidirectionalIterator<TArg> : IBidirectionalInputIterator<TArg>, IBidirectionalOutputIterator<TArg>, IForwardIterator<TArg>
{
	new public ref TArg Current { get; }
	TArg IInputIterator<TArg>.Current => Current;
	TArg IOutputIterator<TArg>.Current { set => Current = value; }
	TArg IStreamingInputIterator<TArg>.Current => Current;
	TArg IStreamingOutputIterator<TArg>.Current { set => Current = value; }
	TArg IForwardInputIterator<TArg>.Current => Current;
	TArg IForwardOutputIterator<TArg>.Current { set => Current = value; }
	TArg IStreamingIterator<TArg>.Current { get => Current; set => Current = value; }
	TArg IBidirectionalInputIterator<TArg>.Current => Current;
	TArg IBidirectionalOutputIterator<TArg>.Current { set => Current = value; }
	ref TArg IForwardIterator<TArg>.Current => ref Current;
}


public interface IRandomAccessInputIterator<TArg> : IBidirectionalInputIterator<TArg>
{
	public void MoveNext(int count);
	public void MovePrevious(int count);

	new public TArg Current { get; }
	TArg IInputIterator<TArg>.Current => Current;
	TArg IStreamingInputIterator<TArg>.Current => Current;
	TArg IForwardInputIterator<TArg>.Current => Current;
	TArg IBidirectionalInputIterator<TArg>.Current => Current;
}

public interface IRandomAccessOutputIterator<TArg> : IBidirectionalOutputIterator<TArg>
{
	public void MoveNext(int count);
	public void MovePrevious(int count);

	new public TArg Current { set; }
	TArg IOutputIterator<TArg>.Current { set => Current = value; }
	TArg IStreamingOutputIterator<TArg>.Current { set => Current = value; }
	TArg IForwardOutputIterator<TArg>.Current { set => Current = value; }
	TArg IBidirectionalOutputIterator<TArg>.Current { set => Current = value; }
}

public interface IRandomAccessIterator<TArg> : IRandomAccessInputIterator<TArg>, IRandomAccessOutputIterator<TArg>, IBidirectionalIterator<TArg>
{
	new public ref TArg Current { get; }
	TArg IInputIterator<TArg>.Current => Current;
	TArg IOutputIterator<TArg>.Current { set => Current = value; }
	TArg IStreamingInputIterator<TArg>.Current => Current;
	TArg IStreamingOutputIterator<TArg>.Current { set => Current = value; }
	TArg IForwardInputIterator<TArg>.Current => Current;
	TArg IForwardOutputIterator<TArg>.Current { set => Current = value; }
	TArg IStreamingIterator<TArg>.Current { get => Current; set => Current = value; }
	TArg IBidirectionalInputIterator<TArg>.Current => Current;
	TArg IBidirectionalOutputIterator<TArg>.Current { set => Current = value; }
	ref TArg IForwardIterator<TArg>.Current => ref Current;
	TArg IRandomAccessInputIterator<TArg>.Current => Current;
	TArg IRandomAccessOutputIterator<TArg>.Current { set => Current = value; }
	ref TArg IBidirectionalIterator<TArg>.Current => ref Current;
}


public interface IContiguousInputIterator<TArg> : IRandomAccessInputIterator<TArg>
{
	new public TArg Current { get; }
	TArg IInputIterator<TArg>.Current => Current;
	TArg IStreamingInputIterator<TArg>.Current => Current;
	TArg IForwardInputIterator<TArg>.Current => Current;
	TArg IBidirectionalInputIterator<TArg>.Current => Current;
	TArg IRandomAccessInputIterator<TArg>.Current => Current;
}

public interface IContiguousOutputIterator<TArg> : IRandomAccessOutputIterator<TArg>
{
	new public TArg Current { set; }
	TArg IOutputIterator<TArg>.Current { set => Current = value; }
	TArg IStreamingOutputIterator<TArg>.Current { set => Current = value; }
	TArg IForwardOutputIterator<TArg>.Current { set => Current = value; }
	TArg IBidirectionalOutputIterator<TArg>.Current { set => Current = value; }
	TArg IRandomAccessOutputIterator<TArg>.Current { set => Current = value; }
}

public interface IContiguousIterator<TArg> : IContiguousInputIterator<TArg>, IContiguousOutputIterator<TArg>, IRandomAccessIterator<TArg>
{
	new public ref TArg Current { get; }
	TArg IInputIterator<TArg>.Current => Current;
	TArg IOutputIterator<TArg>.Current { set => Current = value; }
	TArg IStreamingInputIterator<TArg>.Current => Current;
	TArg IStreamingOutputIterator<TArg>.Current { set => Current = value; }
	TArg IForwardInputIterator<TArg>.Current => Current;
	TArg IForwardOutputIterator<TArg>.Current { set => Current = value; }
	TArg IStreamingIterator<TArg>.Current { get => Current; set => Current = value; }
	TArg IBidirectionalInputIterator<TArg>.Current => Current;
	TArg IBidirectionalOutputIterator<TArg>.Current { set => Current = value; }
	ref TArg IForwardIterator<TArg>.Current => ref Current;
	TArg IRandomAccessInputIterator<TArg>.Current => Current;
	TArg IRandomAccessOutputIterator<TArg>.Current { set => Current = value; }
	ref TArg IBidirectionalIterator<TArg>.Current => ref Current;
	TArg IContiguousInputIterator<TArg>.Current => Current;
	TArg IContiguousOutputIterator<TArg>.Current { set => Current = value; }
	ref TArg IRandomAccessIterator<TArg>.Current => ref Current;
}
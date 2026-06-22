namespace InfinityOfficialNetwork.StandardLibrary.Iterators.Operations;
public interface IIteratorMoveNext
{
	public void MoveNext();
}

public interface IIteratorMoveNextMany : IIteratorMoveNext
{
	public void MoveNext(ulong count);
}

public interface IIteratorMoveBack
{
	public void MoveBack();
}

public interface IIteratorMoveBackMany : IIteratorMoveBack
{
	public void MoveBack(ulong count);
}

public interface IIteratorValueGet<out TArg>
	where TArg : allows ref struct
{
	public TArg Value { get; }
}

public interface IIteratorValueSet<in TArg>
	where TArg : allows ref struct
{
	public TArg Value { set; }
}

public interface IIteratorValueGetSet<TArg> : IIteratorValueGet<TArg>, IIteratorValueSet<TArg>
	where TArg : allows ref struct
{
	new public TArg Value { get; set; }
	//TArg IIteratorValueGet<TArg>.Value => Value;
	//TArg IIteratorValueSet<TArg>.Value { set => Value = value; }
}

public interface IIteratorValueGetRef<TArg> : IIteratorValueGetSet<TArg>
	where TArg : allows ref struct
{
	public ref TArg ValueRef { get; }
	//TArg IIteratorValueGet<TArg>.Value => Value;
	//TArg IIteratorValueSet<TArg>.Value { set => Value = value; }
	//TArg IIteratorValueGetSet<TArg>.Value { get => Value; set => Value = value; }
}

public interface IIteratorValueGetPtr<TArg>
	where TArg : allows ref struct
{
	public unsafe TArg* Ptr { get; }
}

public interface IIteratorCloneable<TSelf>
	where TSelf : IIteratorCloneable<TSelf>, allows ref struct
{
	public TSelf Clone();
}

public interface IIteratorEquatable<TSelf>
	where TSelf : IIteratorEquatable<TSelf>, allows ref struct
{
	public bool Equals(TSelf other);
	public static virtual bool operator ==(TSelf lhs, TSelf rhs) => lhs.Equals(rhs);
	public static virtual bool operator !=(TSelf lhs, TSelf rhs) => !lhs.Equals(rhs);
}

public interface IIteratorComparable<TSelf> : IIteratorEquatable<TSelf>
	where TSelf : IIteratorComparable<TSelf>, allows ref struct
{
	//returns the number of elements between this and other, or negative if this preceeds other
	public long Compare(TSelf other);
	public static virtual bool operator >(TSelf lhs, TSelf rhs) => lhs.Compare(rhs) > 0;
	public static virtual bool operator <(TSelf lhs, TSelf rhs) => lhs.Compare(rhs) < 0;
	public static virtual bool operator <=(TSelf lhs, TSelf rhs) => lhs.Compare(rhs) <= 0;
	public static virtual bool operator >=(TSelf lhs, TSelf rhs) => lhs.Compare(rhs) >= 0;
}
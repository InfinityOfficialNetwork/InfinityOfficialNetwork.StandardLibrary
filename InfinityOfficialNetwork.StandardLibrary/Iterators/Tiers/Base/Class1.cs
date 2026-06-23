using InfinityOfficialNetwork.StandardLibrary.Iterators.Dispatching.Iterators;

namespace InfinityOfficialNetwork.StandardLibrary.Iterators.Tiers.Base;
public interface IReadOnlyIterator<out TArg> : IIterator
	where TArg : allows ref struct
{
	public void ReadOnlyAccept<TIteratorVisitor>(TIteratorVisitor visitor) where TIteratorVisitor : IReadOnlyIteratorVisitor<TArg>, allows ref struct;
	public TRet ReadOnlyAccept<TIteratorVisitor, TRet>(TIteratorVisitor visitor) where TIteratorVisitor : IReadOnlyIteratorVisitor<TArg, TRet>, allows ref struct;
}

public interface IWriteOnlyIterator<in TArg> : IIterator
	where TArg : allows ref struct
{
	public void WriteOnlyAccept<TIteratorVisitor>(TIteratorVisitor visitor) where TIteratorVisitor : IWriteOnlyIteratorVisitor<TArg>, allows ref struct;
	public TRet WriteOnlyAccept<TIteratorVisitor, TRet>(TIteratorVisitor visitor) where TIteratorVisitor : IWriteOnlyIteratorVisitor<TArg, TRet>, allows ref struct;
}

public interface IIterator<TArg> : IIterator
	where TArg : allows ref struct
{
	public void Accept<TIteratorVisitor>(TIteratorVisitor visitor) where TIteratorVisitor : IIteratorVisitor<TArg>, allows ref struct;
	public TRet Accept<TIteratorVisitor, TRet>(TIteratorVisitor visitor) where TIteratorVisitor : IIteratorVisitor<TArg, TRet>, allows ref struct;
}

public interface IRefIterator<TArg> : IIterator
	where TArg : allows ref struct
{
	public void RefAccept<TIteratorVisitor>(TIteratorVisitor visitor) where TIteratorVisitor : IRefIteratorVisitor<TArg>, allows ref struct;
	public TRet RefAccept<TIteratorVisitor, TRet>(TIteratorVisitor visitor) where TIteratorVisitor : IRefIteratorVisitor<TArg, TRet>, allows ref struct;
}
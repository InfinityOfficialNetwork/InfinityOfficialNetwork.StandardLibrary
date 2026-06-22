using InfinityOfficialNetwork.StandardLibrary.Iterators.Dispatching.Iterators;

namespace InfinityOfficialNetwork.StandardLibrary.Iterators.Tiers.Base;
public interface IReadOnlyIterator<out TArg> : IIterator
	where TArg : allows ref struct
{
	public void ReadOnlyAccept<TIteratorVisitor>(TIteratorVisitor visitor) where TIteratorVisitor : IReadOnlyIteratorVisitor<TArg>, allows ref struct;
	public void ReadOnlyAccept<TIteratorVisitor>() where TIteratorVisitor : IStaticReadOnlyIteratorVisitor<TArg>, allows ref struct;
	public TRet ReadOnlyAccept<TIteratorVisitor, TRet>(TIteratorVisitor visitor) where TIteratorVisitor : IReadOnlyIteratorVisitor<TArg, TRet>, allows ref struct;
	public TRet ReadOnlyAccept<TIteratorVisitor, TRet>() where TIteratorVisitor : IStaticReadOnlyIteratorVisitor<TArg, TRet>, allows ref struct;
}

public interface IWriteOnlyIterator<in TArg> : IIterator
	where TArg : allows ref struct
{
	public void WriteOnlyAccept<TIteratorVisitor>(TIteratorVisitor visitor) where TIteratorVisitor : IWriteOnlyIteratorVisitor<TArg>, allows ref struct;
	public void WriteOnlyAccept<TIteratorVisitor>() where TIteratorVisitor : IStaticWriteOnlyIteratorVisitor<TArg>, allows ref struct;
	public TRet WriteOnlyAccept<TIteratorVisitor, TRet>(TIteratorVisitor visitor) where TIteratorVisitor : IWriteOnlyIteratorVisitor<TArg, TRet>, allows ref struct;
	public TRet WriteOnlyAccept<TIteratorVisitor, TRet>() where TIteratorVisitor : IStaticWriteOnlyIteratorVisitor<TArg, TRet>, allows ref struct;
}

public interface IIterator<TArg> : IIterator
	where TArg : allows ref struct
{
	public void Accept<TIteratorVisitor>(TIteratorVisitor visitor) where TIteratorVisitor : IIteratorVisitor<TArg>, allows ref struct;
	public void Accept<TIteratorVisitor>() where TIteratorVisitor : IStaticIteratorVisitor<TArg>, allows ref struct;
	public TRet Accept<TIteratorVisitor, TRet>(TIteratorVisitor visitor) where TIteratorVisitor : IIteratorVisitor<TArg, TRet>, allows ref struct;
	public TRet Accept<TIteratorVisitor, TRet>() where TIteratorVisitor : IStaticIteratorVisitor<TArg, TRet>, allows ref struct;
}

public interface IRefIterator<TArg> : IIterator
	where TArg : allows ref struct
{
	public void RefAccept<TIteratorVisitor>(TIteratorVisitor visitor) where TIteratorVisitor : IRefIteratorVisitor<TArg>, allows ref struct;
	public void RefAccept<TIteratorVisitor>() where TIteratorVisitor : IStaticRefIteratorVisitor<TArg>, allows ref struct;
	public TRet RefAccept<TIteratorVisitor, TRet>(TIteratorVisitor visitor) where TIteratorVisitor : IRefIteratorVisitor<TArg, TRet>, allows ref struct;
	public TRet RefAccept<TIteratorVisitor, TRet>() where TIteratorVisitor : IStaticRefIteratorVisitor<TArg, TRet>, allows ref struct;
}
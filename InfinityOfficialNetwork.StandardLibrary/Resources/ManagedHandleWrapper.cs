namespace InfinityOfficialNetwork.StandardLibrary.Resources;

public static class ManagedHandleWrapper
{
	public static ManagedHandleWrapper<THandle> CreateFrom<THandle>(MoveReference<THandle> moveReference)
		where THandle : unmanaged, IHandle<THandle>
	{
		ManagedHandleWrapper<THandle> wrapper = new();
		moveReference.MoveTo(ref wrapper.value);
		return wrapper;
	}

	public static ManagedHandleWrapper<THandle> CreateFrom<THandle>(CopyReference<THandle> moveReference)
	where THandle : unmanaged, ICopyableHandle<THandle>
	{
		ManagedHandleWrapper<THandle> wrapper = new();
		moveReference.CopyTo(ref wrapper.value);
		return wrapper;
	}

	public static ManagedHandleWrapper<THandle> CreateFrom<THandle>(CloneReference<THandle> moveReference)
	where THandle : unmanaged, ICloneableHandle<THandle>
	{
		ManagedHandleWrapper<THandle> wrapper = new();
		moveReference.CloneTo(ref wrapper.value);
		return wrapper;
	}

	public static ManagedHandleWrapper<THandle> MoveToManagedHandleWrapper<THandle>(this ref THandle handle)
	where THandle : unmanaged, IHandle<THandle>
	{
		ManagedHandleWrapper<THandle> wrapper = new();
		handle.MoveTo(ref wrapper.value);
		return wrapper;
	}

	public static ManagedHandleWrapper<THandle> CopyToManagedHandleWrapper<THandle>(this ref THandle handle)
	where THandle : unmanaged, ICopyableHandle<THandle>
	{
		ManagedHandleWrapper<THandle> wrapper = new();
		handle.CopyTo(ref wrapper.value);
		return wrapper;
	}

	public static ManagedHandleWrapper<THandle> CloneToManagedHandleWrapper<THandle>(this ref THandle handle)
	where THandle : unmanaged, ICloneableHandle<THandle>
	{
		ManagedHandleWrapper<THandle> wrapper = new();
		handle.CloneTo(ref wrapper.value);
		return wrapper;
	}
}

public class ManagedHandleWrapper<THandle> : IDisposable
	where THandle : unmanaged, IHandle<THandle>
{
	internal protected THandle value;

	public THandle Value { get => value; }

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		Value.Dispose();
	}

	~ManagedHandleWrapper()
	{
		Value.Dispose();
	}

	public ManagedHandleWrapper(MoveReference<THandle> moveReference)
	{
		moveReference.MoveTo(ref value);
	}

	internal ManagedHandleWrapper() { }
}
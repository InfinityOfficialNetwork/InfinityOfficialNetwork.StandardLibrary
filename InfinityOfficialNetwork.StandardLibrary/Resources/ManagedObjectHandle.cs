using InfinityOfficialNetwork.StandardLibrary.Common;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace InfinityOfficialNetwork.StandardLibrary.Resources;


public partial struct ManagedObjectHandle<TArg> : IHandle<ManagedObjectHandle<TArg>>, ICopyableHandle<ManagedObjectHandle<TArg>>, IEquatable<ManagedObjectHandle<TArg>>
{
	private GCHandle gcHandle;


	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ManagedObjectHandle()
	{
		gcHandle = default;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ManagedObjectHandle(TArg obj)
	{
		gcHandle = GCHandle.Alloc(obj, GCHandleType.Normal);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ManagedObjectHandle(MoveReference<ManagedObjectHandle<TArg>> from)
	{
		from.MoveTo(ref this);
	}

	public bool Active
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => !gcHandle.Equals(default);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Dispose()
	{
		if (gcHandle != default)
		{
			gcHandle.Free();
			gcHandle = default;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void MoveTo(ref ManagedObjectHandle<TArg> target)
	{
		target.gcHandle = gcHandle;
		gcHandle = default;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void CopyTo(ref ManagedObjectHandle<TArg> target)
	{
		target.gcHandle = GCHandle.Alloc(gcHandle.Target, GCHandleType.Normal);
	}

	public TArg Value { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => (TArg)gcHandle.Target!; }

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ManagedObjectHandle<TArg> FromObject(TArg obj)
	{
		var handle = new ManagedObjectHandle<TArg> { gcHandle = GCHandle.Alloc(obj, GCHandleType.Normal) };

		return handle;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(ManagedObjectHandle<TArg> other) => gcHandle.Equals(other.gcHandle);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override bool Equals(object? obj) => obj != null && obj is ManagedObjectHandle<TArg> && Equals((ManagedObjectHandle<TArg>)obj);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode() => gcHandle.GetHashCode();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(ManagedObjectHandle<TArg> left, ManagedObjectHandle<TArg> right) => left.Equals(right);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(ManagedObjectHandle<TArg> left, ManagedObjectHandle<TArg> right) => !(left == right);

	public static ManagedObjectHandle<TArg> ToManagedObjectHandle<TOut>(ManagedObjectHandle<TArg> @in) where TOut : TArg => new ManagedObjectHandle<TArg>() { gcHandle = @in.gcHandle };
}

public struct ManagedObjectCloneableHandle<TArg> : IHandle<ManagedObjectCloneableHandle<TArg>>, ICopyableHandle<ManagedObjectCloneableHandle<TArg>>, ICloneableHandle<ManagedObjectCloneableHandle<TArg>>, IEquatable<ManagedObjectCloneableHandle<TArg>>
	where TArg : ICloneable<TArg>
{
	private GCHandle gcHandle;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ManagedObjectCloneableHandle() => gcHandle = default;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ManagedObjectCloneableHandle(TArg obj) => gcHandle = GCHandle.Alloc(obj, GCHandleType.Normal);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ManagedObjectCloneableHandle(MoveReference<ManagedObjectCloneableHandle<TArg>> from) => from.MoveTo(ref this);

	public bool Active { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => !gcHandle.Equals(default); }

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Dispose()
	{
		if (gcHandle != default)
		{
			gcHandle.Free();
			gcHandle = default;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void MoveTo(ref ManagedObjectCloneableHandle<TArg> target)
	{
		target.gcHandle = gcHandle;
		gcHandle = default;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void CopyTo(ref ManagedObjectCloneableHandle<TArg> target)
	{
		target.gcHandle = GCHandle.Alloc(gcHandle.Target, GCHandleType.Normal);
	}
	public void CloneTo(ref ManagedObjectCloneableHandle<TArg> target)
	{
		target.gcHandle = GCHandle.Alloc(((TArg)gcHandle.Target!).Clone(), GCHandleType.Normal);
	}

	public TArg Value { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => (TArg)gcHandle.Target!; }

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ManagedObjectCloneableHandle<TArg> FromObject(TArg obj)
	{
		var handle = new ManagedObjectCloneableHandle<TArg> { gcHandle = GCHandle.Alloc(obj, GCHandleType.Normal) };

		return handle;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(ManagedObjectCloneableHandle<TArg> other) => gcHandle.Equals(other.gcHandle);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override bool Equals(object? obj)
	{
		return obj != null && obj is ManagedObjectCloneableHandle<TArg> && Equals((ManagedObjectCloneableHandle<TArg>)obj);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode() => gcHandle.GetHashCode();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(ManagedObjectCloneableHandle<TArg> left, ManagedObjectCloneableHandle<TArg> right) => left.Equals(right);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(ManagedObjectCloneableHandle<TArg> left, ManagedObjectCloneableHandle<TArg> right) => !(left == right);

	public static ManagedObjectCloneableHandle<TArg> ToManagedObjectCloneableHandle<TOut>(ManagedObjectCloneableHandle<TArg> @in) where TOut : TArg => new ManagedObjectCloneableHandle<TArg>() { gcHandle = @in.gcHandle };
}

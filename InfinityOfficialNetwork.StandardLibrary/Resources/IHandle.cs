using InfinityOfficialNetwork.StandardLibrary.Attributes;
using InfinityOfficialNetwork.StandardLibrary.Common;
using System.Runtime.CompilerServices;

namespace InfinityOfficialNetwork.StandardLibrary.Resources;

/// <summary>
/// Represents any class which uses unmanaged structs as handles and is fully capable of handling them correctly. Meant to be used as a generic constraint for handle-using code.
/// </summary>
public interface IHandleAware { }

/// <summary> 
/// Represents operations for disposing of manually managed resources.
/// <br/>
/// This type is not to be implemened directly. Implement <![CDATA[IHandle<TSelf>]]> instead
/// </summary>
public interface IHandle : IDisposable
{
	/// <summary>
	/// Whether any resources in this object is value. This value must be false for default constructed objects (must be backed by a zero-able field specifying validity).
	/// </summary>
	public bool Active
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[NothrowGuaranteeExceptionSpecification]
		get;
	}
}

/// <summary>
/// Represents a manually managed handle to a resource. Implementing types shall follow move-semantics.
/// <br/>
/// If any object is zeroed, it is null and contains no resources; otherwise it is implementation defined whether it is active
/// </summary>
/// <typeparam name="TSelf"></typeparam>
public interface IHandle<TSelf> : IHandle
	where TSelf : unmanaged, IHandle<TSelf>
{
	/// <summary>
	/// Moves all resources to destination object and zeros called-on object. Undefined behavior if called on a an inactive object
	/// </summary>
	/// <param name="target"></param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[NothrowGuaranteeExceptionSpecification]
	public void MoveTo(ref TSelf target);
}

/// <summary>
/// Represents a handle where the underlying resource supports multiple handles to the same resource. This does not copy the actual resource, see ICloneableHandle.
/// </summary>
/// <typeparam name="TSelf"></typeparam>
public interface ICopyableHandle<TSelf> : IHandle<TSelf>
	where TSelf : unmanaged, ICopyableHandle<TSelf>
{
	/// <summary>
	/// Copies this handle to specified resource to target. Does not copy the underlying resource.
	/// </summary>
	/// <param name="target"></param>
	[BasicGuaranteeExceptionSpecification<Exception>]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void CopyTo(ref TSelf target);
}

public interface ICloneableHandle<TSelf> : IHandle<TSelf>, ICloneable<TSelf>
	where TSelf : unmanaged, ICloneableHandle<TSelf>, ICloneable<TSelf>
{
	/// <summary>
	/// Clones all resources targeted by this object to target. Target may be uninitialized.
	/// </summary>
	/// <param name="target"></param>
	[BasicGuaranteeExceptionSpecification<Exception>]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void CloneTo(ref TSelf target);

	[BasicGuaranteeExceptionSpecification<Exception>]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	TSelf ICloneable<TSelf>.Clone()
	{
		Unsafe.SkipInit(out TSelf target);
		CloneTo(ref target);
		return target;
	}
}

public static class HandleExtensions
{
	/// <summary>
	/// Creates a MoveReference to move the resources from this object.
	/// <br/>
	/// The object will be in an uninitialized (zero-able field set to zero) state after the MoveReference is passed to any method taking a MoveReference.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[NothrowGuaranteeExceptionSpecification]
	public static MoveReference<TArg> MoveFrom<TArg>(this ref TArg managedObject) where TArg : unmanaged, IHandle<TArg> => new MoveReference<TArg>(ref managedObject);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BasicGuaranteeExceptionSpecification<Exception>]
	public static CopyReference<TArg> CopyFrom<TArg>(this ref TArg managedObject) where TArg : unmanaged, ICopyableHandle<TArg> => new CopyReference<TArg>(ref managedObject);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BasicGuaranteeExceptionSpecification<Exception>]
	public static CloneReference<TArg> CloneFrom<TArg>(this ref TArg managedObject) where TArg : unmanaged, ICloneableHandle<TArg> => new CloneReference<TArg>(ref managedObject);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[NothrowGuaranteeExceptionSpecification]
	public static BorrowReference<TArg> BorrowFrom<TArg>(this ref TArg managedObject) where TArg : unmanaged, IHandle<TArg> => new BorrowReference<TArg>(ref managedObject);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[NothrowGuaranteeExceptionSpecification]
	public static ReadOnlyBorrowReference<TArg> BorrowReadOnlyFrom<TArg>(this ref TArg managedObject) where TArg : unmanaged, IHandle<TArg> => new ReadOnlyBorrowReference<TArg>(ref managedObject);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[NothrowGuaranteeExceptionSpecification]
	public static OutputReference<TArg> OutputTo<TArg>(this ref TArg managedObject) where TArg : unmanaged, IHandle<TArg> => new OutputReference<TArg>(ref managedObject);
}

/// <summary>
/// Represents a handle to transfer resources. This must be used exactly once.
/// </summary>
/// <typeparam name="THandle"></typeparam>
public readonly ref struct MoveReference<THandle>
	where THandle : unmanaged, IHandle<THandle>
{
	internal readonly ref THandle value;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[NothrowGuaranteeExceptionSpecification]
	internal MoveReference(ref THandle value)
	{
		this.value = ref value;
	}

	/// <summary>
	/// Moves all resources contained by this object to target. This will set the origin object to uninitialized state.
	/// <br/>
	/// It is undefined behavior if this method is called more than once, or not at all for a single resource.
	/// </summary>
	/// <param name="target"></param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[NothrowGuaranteeExceptionSpecification]
	public void MoveTo(ref THandle target)
	{
		value.MoveTo(ref target);
	}
}

/// <summary>
/// Represents a handle to copy handles to the same resource. This can be used any number of times or not at all.
/// </summary>
/// <typeparam name="THandle"></typeparam>
public readonly ref struct CopyReference<THandle>
	where THandle : unmanaged, IHandle<THandle>, ICopyableHandle<THandle>
{
	internal readonly ref readonly THandle value;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[NothrowGuaranteeExceptionSpecification]
	internal CopyReference(in THandle value)
	{
		this.value = ref value;
	}

	/// <summary>
	/// Moves all resources contained by this object to target. This will set the origin object to uninitialized state.
	/// </summary>
	/// <param name="target"></param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BasicGuaranteeExceptionSpecification<Exception>]
	public void CopyTo(ref THandle target)
	{
		value.CopyTo(ref target);
	}
}

/// <summary>
/// Represents a handle to clone the entire object/resource graph. This can be used any number of times or none at all.
/// </summary>
/// <typeparam name="THandle"></typeparam>
public readonly ref struct CloneReference<THandle>
	where THandle : unmanaged, IHandle<THandle>, ICloneableHandle<THandle>
{
	internal readonly ref readonly THandle value;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[NothrowGuaranteeExceptionSpecification]
	internal CloneReference(in THandle value)
	{
		this.value = ref value;
	}

	/// <summary>
	/// Moves all resources contained by this object to target. This will set the origin object to uninitialized state.
	/// </summary>
	/// <param name="target"></param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BasicGuaranteeExceptionSpecification<Exception>]
	public void CloneTo(ref THandle target)
	{
		value.CloneTo(ref target);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BasicGuaranteeExceptionSpecification<Exception>]
	public THandle CloneTo()
	{
		Unsafe.SkipInit(out THandle handle);
		value.CloneTo(ref handle);
		return handle;
	}
}

/// <summary>
/// Represents a handle to borrow resources. It is invalid to move to/from or otherwise change the resource referenced by this handle.
/// <br/>
/// Not meant for copy/clone operations.
/// </summary>
/// <typeparam name="THandle"></typeparam>
public readonly ref struct BorrowReference<THandle>
	where THandle : unmanaged, IHandle<THandle>
{
	internal readonly ref THandle value;

	[NothrowGuaranteeExceptionSpecification]
	internal BorrowReference(ref THandle value)
	{
		this.value = ref value;
	}

	/// <summary>
	/// Accesses the resource pointed by the handle.
	/// </summary>
	/// <param name="target"></param>
	public readonly ref THandle Value
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[NothrowGuaranteeExceptionSpecification]
		get => ref value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "<Pending>")]
	public static implicit operator ReadOnlyBorrowReference<THandle>(BorrowReference<THandle> borrowReference) => new ReadOnlyBorrowReference<THandle>(ref borrowReference.value);
}

/// <summary>
/// Represents a handle to borrow resources immutably. It is invalid to move to/from or otherwise change the resource referenced by this handle.
/// <br/>
/// Not meant for copy/clone operations.
/// </summary>
/// <typeparam name="THandle"></typeparam>
public readonly ref struct ReadOnlyBorrowReference<THandle>
	where THandle : unmanaged, IHandle<THandle>
{
	internal readonly ref readonly THandle value;

	[NothrowGuaranteeExceptionSpecification]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal ReadOnlyBorrowReference(ref readonly THandle value)
	{
		this.value = ref value;
	}

	/// <summary>
	/// Accesses the resource pointed by the handle.
	/// </summary>
	/// <param name="target"></param>
	public readonly ref readonly THandle Value
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[NothrowGuaranteeExceptionSpecification]
		get => ref value;
	}
}

/// <summary>
/// Represents a reference to write to. Output handle referenced is guaranteed to be uninitialized and invalid. This must be written to exactly once.
/// </summary>
/// <typeparam name="THandle"></typeparam>
public readonly ref struct OutputReference<THandle>
	where THandle : unmanaged, IHandle<THandle>
{
	internal readonly ref THandle value;

	[NothrowGuaranteeExceptionSpecification]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal OutputReference(ref THandle value)
	{
		this.value = ref value;
	}

	[NothrowGuaranteeExceptionSpecification]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void OutputFrom(MoveReference<THandle> moveReference)
	{
		moveReference.MoveTo(ref value);
	}
}

public static class HandleReferenceExtensions
{
	[NothrowGuaranteeExceptionSpecification]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ReadOnlyBorrowReference<THandle> ToReadOnlyBorrowReference<THandle>(this BorrowReference<THandle> handle)
		where THandle : unmanaged, IHandle<THandle>
		=> new ReadOnlyBorrowReference<THandle>(ref handle.value);

	[NothrowGuaranteeExceptionSpecification]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static CopyReference<THandle> ToCopyReference<THandle>(this BorrowReference<THandle> handle)
		where THandle : unmanaged, ICopyableHandle<THandle>
		=> new CopyReference<THandle>(in handle.value);

	[NothrowGuaranteeExceptionSpecification]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static CopyReference<THandle> ToCopyReference<THandle>(this ReadOnlyBorrowReference<THandle> handle)
		where THandle : unmanaged, ICopyableHandle<THandle>
		=> new CopyReference<THandle>(in handle.value);

	[NothrowGuaranteeExceptionSpecification]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static CloneReference<THandle> ToCloneReference<THandle>(this BorrowReference<THandle> handle)
		where THandle : unmanaged, ICloneableHandle<THandle>
		=> new CloneReference<THandle>(in handle.value);

	[NothrowGuaranteeExceptionSpecification]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static CloneReference<THandle> ToCloneReference<THandle>(this ReadOnlyBorrowReference<THandle> handle)
		where THandle : unmanaged, ICloneableHandle<THandle>
		=> new CloneReference<THandle>(in handle.value);
}
namespace InfinityOfficialNetwork.StandardLibrary.Resources;

public interface IMemoryHandle<TArg> 
	where TArg : unmanaged
{
	unsafe ref TArg this[long index] { get => ref Pointer[index]; }

	unsafe TArg* Pointer { get; }
	ref TArg Ref { get; }
}
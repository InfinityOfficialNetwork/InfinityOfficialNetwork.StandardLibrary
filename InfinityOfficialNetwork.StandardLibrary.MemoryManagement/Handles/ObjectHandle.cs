namespace InfinityOfficialNetwork.StandardLibrary.MemoryManagement.Handles
{
	public abstract class ObjectHandle<T> : MemoryHandle
	{
		new public unsafe T* Pointer => (T*)handle;
	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinityOfficialNetwork.StandardLibrary.MemoryManagement.Handles;

namespace InfinityOfficialNetwork.StandardLibrary.MemoryManagement.Objects
{
	public interface IObjectAllocator<TArg>
	{
		public unsafe TArg* AllocateObject();
		public unsafe TArg* AllocateObject(nint count);
		public ObjectHandle<TArg> AllocateObjectHandle();
		public ObjectHandle<TArg> AllocateObjectHandle(nint count);
		public unsafe void Free(TArg* ptr);
	}
}

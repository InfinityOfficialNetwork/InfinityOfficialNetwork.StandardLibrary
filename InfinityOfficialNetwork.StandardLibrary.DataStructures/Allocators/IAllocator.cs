using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Allocators
{
	public interface IAllocator
	{
		public unsafe void* Allocate(nint size);
		public unsafe T* Allocate<T>(nint count) where T : struct;
		public unsafe void Free(void* ptr);
		public unsafe void Free<T>(T* ptr) where T : struct;
	}
}

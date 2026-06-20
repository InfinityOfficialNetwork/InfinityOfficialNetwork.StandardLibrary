using InfinityOfficialNetwork.StandardLibrary.Memory;
using InfinityOfficialNetwork.StandardLibrary.Utilities;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace InfinityOfficialNetwork.StandardLibrary.Tests;

[TestClass]
public class Test1
{
    [TestMethod]
    public unsafe void TestMethod1()
    {

		GCHandle<string>* ptr = AllocatorUtilities<NativeAllocator>.AllocateUninitializedObject<GCHandle<string>>();
		*ptr = new GCHandle<string>(new string("test"));

		Debugger.Log(1, null, ptr->Target);

		ptr->Dispose();
		AllocatorUtilities<NativeAllocator>.FreeObject(ptr);
	}
}

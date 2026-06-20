using InfinityOfficialNetwork.StandardLibrary.Common;
using InfinityOfficialNetwork.StandardLibrary.Contracts;
using InfinityOfficialNetwork.StandardLibrary.Exceptions;
using InfinityOfficialNetwork.StandardLibrary.Memory;
using InfinityOfficialNetwork.StandardLibrary.Resources;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace InfinityOfficialNetwork.StandardLibrary.Test;



internal class Program
{
	static void Main(string[] args)
	{
		int a = (int)TestFunc(0);
	}

	static Expected<object, Exception> TestFunc(int a)
	{
		if (a == 0)
			return (object)a;
		else
			return Expected.FromException(new Exception());
	}
}

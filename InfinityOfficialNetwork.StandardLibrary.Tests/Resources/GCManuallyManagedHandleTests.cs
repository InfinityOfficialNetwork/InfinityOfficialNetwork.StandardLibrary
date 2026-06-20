using Microsoft.VisualStudio.TestTools.UnitTesting;
using InfinityOfficialNetwork.StandardLibrary.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfinityOfficialNetwork.StandardLibrary.Resources.Tests;

[TestClass()]
public class GCManuallyManagedHandleTests
{


	[TestMethod()]
	public void MoveToTest()
	{
		ManagedObjectHandle<string> hTestString = default;
		ManagedObjectHandle<string> hTestStringTarget = default;

		try
		{
			string testString = new string("test");

			hTestString = ManagedObjectHandle<string>.FromObject(testString);

			hTestStringTarget = default;

			hTestString.MoveFrom().MoveTo(ref hTestStringTarget);

			Assert.IsTrue(hTestString.Active == false);
			Assert.IsTrue(hTestStringTarget.Active == true);

			Assert.IsTrue(hTestStringTarget.Value == testString);
		}
		finally
		{
			hTestString.Dispose();
			hTestStringTarget.Dispose();
		}
	}

	[TestMethod()]
	public void FromObjectTest()
	{
		string testString = new string("test");

		using ManagedObjectHandle<string> hTestString = ManagedObjectHandle<string>.FromObject(testString);

		GC.Collect(GC.MaxGeneration, GCCollectionMode.Aggressive, true, true);
		GC.Collect(GC.MaxGeneration, GCCollectionMode.Aggressive, true, true);

		Assert.IsTrue(hTestString.Value == "test");
	}
}
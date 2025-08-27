using InfinityOfficialNetwork.StandardLibrary.DataStructures.Containers.Primary;
using InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators;

namespace InfinityOfficialNetwork.StandardLibrary.Tests;

[TestClass]
public class Iterators
{
    [TestMethod]
    public void BasicIterator1()
    {
        IVector<int> vector = new Vector<int>(100,100);

        for (IContiguousIterator<int> i = vector.Begin, e = vector.End; !i.Equals(e); i.MoveNext())
            Assert.IsTrue(i.Current == 100);
    }

	[TestMethod]
	public void BasicIterator2()
	{
		IVector<int> vector = new Vector<int>(10000,0);

		int j = 0;
		for (IContiguousIterator<int> i = vector.Begin, e = vector.End; !i.Equals(e); i.MoveNext())
			i.Current = j++;

		j = 0;

		for (IContiguousIterator<int> i = vector.Begin, e = vector.End; !i.Equals(e); i.MoveNext())
			Assert.IsTrue(i.Current == j++);
	}
}

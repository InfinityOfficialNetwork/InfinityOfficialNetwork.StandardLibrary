using InfinityOfficialNetwork.StandardLibrary.DataStructures.Containers.Primary;

namespace InfinityOfficialNetwork.StandardLibrary.Tests;

[TestClass]
public class Constructor
{
    [TestMethod]
    public void DefaultConstructor()
    {
        using IVector<int> ints = new Vector<int>();

        Assert.IsTrue(ints.Size == 0);
        Assert.IsTrue(ints.ReserveSize == VectorDefaultParameters.InitialReserve);
		Assert.IsTrue(ints.IsEmpty);
	}

	[TestMethod]
	public void ValueConstructor()
	{
		using IVector<int> ints = new Vector<int>(100, 123);

		Assert.IsTrue(ints.Size == 100);
		Assert.IsTrue(!ints.IsEmpty);
		for (int i = 0; i < 100; i++)
			Assert.IsTrue(ints[i] == 123);
	}

	[TestMethod]
	public void FactoryConstructor()
	{
		int acc = 0;
		using IVector<int> ints = new Vector<int>(100, () => acc++);

		acc = 0;

		Assert.IsTrue(ints.Size == 100);
		Assert.IsTrue(!ints.IsEmpty);
		for (int i = 0; i < 100; i++)
			Assert.IsTrue(ints[i] == acc++);
	}

	[TestMethod]
	public void FactoryConstructor2()
	{
		int acc = 0;
		using IVector<int> ints = new Vector<int>(100, (i) => acc++ + i);

		acc = 0;

		Assert.IsTrue(ints.Size == 100);
		Assert.IsTrue(!ints.IsEmpty);
		for (int i = 0; i < 100; i++)
			Assert.IsTrue(ints[i] == acc++ + i);
	}

	[TestMethod]
	public void ContainerConstructor()
	{
		int acc = 0;
		using IVector<int> ints = new Vector<int>(10000, (i) => acc++ + i);

		using IVector<int> ints2 = new Vector<int>(ints);

		for (int i = 0; i <= 100; i++)
			Assert.IsTrue(ints2[i] == ints[i]);
	}

}

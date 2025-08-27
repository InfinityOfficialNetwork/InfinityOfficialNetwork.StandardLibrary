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

    [TestMethod]
    public void Constructor_Default_CreatesEmptyVector()
    {
        // Arrange & Act
        IVector<int> vector = new Vector<int>();

        // Assert
        Assert.IsTrue(vector.Size == 0);
        Assert.IsTrue(vector.IsEmpty);
    }

    [TestMethod]
    public void Constructor_WithValue_SetsSizeAndValue()
    {
        // Arrange & Act
        IVector<int> vector = new Vector<int>(42);

        // Assert
        Assert.IsTrue(vector.Size == 1);
        Assert.IsTrue(vector.Front == 42);
    }

    [TestMethod]
    public void Constructor_WithCountAndValue_SetsSize()
    {
        // Arrange & Act
        IVector<int> vector = new Vector<int>(5, 10);

        // Assert
        Assert.IsTrue(vector.Size == 5);
    }

    [TestMethod]
    public void Constructor_WithCountAndValue_FillsWithCorrectValue()
    {
        // Arrange & Act
        IVector<int> vector = new Vector<int>(5, 10);

        // Assert
        for (int i = 0; i < 5; i++)
        {
            Assert.IsTrue(vector.At(i) == 10);
        }
    }

    [TestMethod]
    public void Constructor_WithCountAndValue_ZeroCount_CreatesEmptyVector()
    {
        // Arrange & Act
        IVector<int> vector = new Vector<int>(0, 10);

        // Assert
        Assert.IsTrue(vector.Size == 0);
        Assert.IsTrue(vector.IsEmpty);
    }

    [TestMethod]
    public void Constructor_WithCountAndFactory_SetsSize()
    {
        // Arrange & Act
        IVector<int> vector = new Vector<int>(10, i => i * 2);

        // Assert
        Assert.IsTrue(vector.Size == 10);
    }

    [TestMethod]
    public void Constructor_WithCountAndFactory_FillsWithCorrectValues()
    {
        // Arrange & Act
        IVector<int> vector = new Vector<int>(10, i => i * 2);

        // Assert
        for (int i = 0; i < 10; i++)
        {
            Assert.IsTrue(vector.At(i) == i * 2);
        }
    }

    [TestMethod]
    public void Constructor_WithCountAndFactory_ZeroCount_CreatesEmptyVector()
    {
        // Arrange & Act
        IVector<int> vector = new Vector<int>(0, i => i * 2);

        // Assert
        Assert.IsTrue(vector.Size == 0);
        Assert.IsTrue(vector.IsEmpty);
    }

    [TestMethod]
    public void Constructor_WithIteratorRange_CopiesAllElements()
    {
        // Arrange
        IVector<int> sourceVector = new Vector<int>(5, i => i + 1);

        // Act
        IVector<int> vector = new Vector<int>(sourceVector.Begin, sourceVector.End);

        // Assert
        Assert.IsTrue(vector.Size == 5);
        for (int i = 0; i < 5; i++)
        {
            Assert.IsTrue(vector.At(i) == i + 1);
        }
    }

    [TestMethod]
    public void Constructor_WithEmptyIteratorRange_CreatesEmptyVector()
    {
        // Arrange
        IVector<int> sourceVector = new Vector<int>();

        // Act
        IVector<int> vector = new Vector<int>(sourceVector.Begin, sourceVector.End);

        // Assert
        Assert.IsTrue(vector.Size == 0);
        Assert.IsTrue(vector.IsEmpty);
    }
}

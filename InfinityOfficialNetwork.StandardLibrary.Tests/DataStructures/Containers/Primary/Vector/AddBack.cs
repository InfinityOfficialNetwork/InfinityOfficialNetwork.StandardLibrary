using InfinityOfficialNetwork.StandardLibrary.DataStructures.Containers.Primary;

namespace InfinityOfficialNetwork.StandardLibrary.Tests;

[TestClass]
public class AddBack
{
	[TestMethod]
	public void AddBack_SingleElement_AddsCorrectly()
	{
		// Arrange
		var vector = new Vector<int>();

		// Act
		vector.AddBack(10);
		vector.AddBack(20);

		// Assert
		Assert.IsTrue(vector.Size == 2);
		Assert.IsTrue(vector.At(0) == 10);
		Assert.IsTrue(vector.At(1) == 20);
	}

	[TestMethod]
	public void AddBack_MultipleElements_AddsCorrectly()
	{
		// Arrange
		var vector = new Vector<int>();
		int count = 5;
		int value = 42;

		// Act
		vector.AddBack(count, value);

		// Assert
		Assert.IsTrue(vector.Size == count);
		for (int i = 0; i < vector.Size; i++)
		{
			Assert.IsTrue(vector.At(i) == value);
		}
	}


	[TestMethod]
	public void AddBackRange_CopiesCorrectly()
	{
		// Arrange
		var sourceVector = new Vector<int>();
		for (int i = 0; i < 5; i++)
		{
			sourceVector.AddBack(i);
		}

		var destVector = new Vector<int>();

		// Act
		destVector.AddBackRange(sourceVector.Begin, sourceVector.End);

		// Assert
		Assert.IsTrue(destVector.Size == sourceVector.Size);
		for (int i = 0; i < destVector.Size; i++)
		{
			Assert.IsTrue(destVector.At(i) == sourceVector.At(i));
		}
	}

	[TestMethod]
	public void RemoveBack_SingleElement_ReducesSize()
	{
		// Arrange
		var vector = new Vector<int>(3, 100);

		// Act
		vector.RemoveBack();

		// Assert
		Assert.IsTrue(vector.Size == 2);
		Assert.IsTrue(vector.At(0) == 100);
		Assert.IsTrue(vector.At(1) == 100);
	}

	[TestMethod]
	public void RemoveBack_MultipleElements_ReducesSizeCorrectly()
	{
		// Arrange
		var vector = new Vector<int>(10, 50);
		int countToRemove = 4;

		// Act
		vector.RemoveBack(countToRemove);

		// Assert
		Assert.IsTrue(vector.Size == 6);
	}

	[TestMethod]
	public void Clear_EmptiesVector()
	{
		// Arrange
		var vector = new Vector<int>(10, 10);

		// Act
		vector.Clear();

		// Assert
		Assert.IsTrue(vector.Size == 0);
		Assert.IsTrue(vector.IsEmpty);
	}
}

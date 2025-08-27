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

	//
	// Tests for Add Operations (10 tests)
	//
	[TestMethod]
	public void AddBack_ToEmptyVector_AddsElement()
	{
		// Arrange
		IVector<int> vector = new Vector<int>();

		// Act
		vector.AddBack(100);

		// Assert
		Assert.IsTrue(vector.Size == 1);
		Assert.IsTrue(vector.Front == 100);
	}

	[TestMethod]
	public void AddBack_ToExistingVector_AddsElementToEnd()
	{
		// Arrange
		IVector<int> vector = new Vector<int>(3, 10);

		// Act
		vector.AddBack(20);

		// Assert
		Assert.IsTrue(vector.Size == 4);
		Assert.IsTrue(vector.Back == 20);
	}

	[TestMethod]
	public void AddBack_SingleElement_TriggersResize()
	{
		// Arrange
		IVector<int> vector = new Vector<int>();
		int initialReserve = vector.ReserveSize;

		vector.AddBack(1);
		vector.AddBack(2);
		vector.AddBack(3);
		vector.AddBack(4);

		// Act
		vector.AddBack(5); // This should trigger a resize

		// Assert
		Assert.IsTrue(vector.Size == 5);
		Assert.IsTrue(vector.ReserveSize > initialReserve);
	}

	[TestMethod]
	public void AddBack_MultipleElements_ToEmptyVector()
	{
		// Arrange
		IVector<int> vector = new Vector<int>();
		int count = 5;
		int value = 42;

		// Act
		vector.AddBack(count, value);

		// Assert
		Assert.IsTrue(vector.Size == count);
		for (int i = 0; i < count; i++)
		{
			Assert.IsTrue(vector.At(i) == value);
		}
	}

	[TestMethod]
	public void AddBack_MultipleElements_TriggersResize()
	{
		// Arrange
		IVector<int> vector = new Vector<int>(2, 10);
		int initialReserve = vector.ReserveSize;
		int count = 4;

		// Act
		vector.AddBack(count, 20);

		// Assert
		Assert.IsTrue(vector.Size == 6);
		Assert.IsTrue(vector.ReserveSize > initialReserve);
	}

	[TestMethod]
	public void AddBack_WithFactory_AddsElementsCorrectly()
	{
		// Arrange
		IVector<int> vector = new Vector<int>();
		int count = 5;

		// Act
		vector.AddBack(count, i => i * 10);

		// Assert
		Assert.IsTrue(vector.Size == count);
		for (int i = 0; i < count; i++)
		{
			Assert.IsTrue(vector.At(i) == i * 10);
		}
	}

	[TestMethod]
	public void AddBackRange_FromEmptyVector_NoChange()
	{
		// Arrange
		IVector<int> vector = new Vector<int>(5, 1);
		IVector<int> sourceVector = new Vector<int>();

		// Act
		vector.AddBackRange(sourceVector.Begin, sourceVector.End);

		// Assert
		Assert.IsTrue(vector.Size == 5);
	}

	[TestMethod]
	public void AddBackRange_FromVector_AddsElements()
	{
		// Arrange
		IVector<int> vector = new Vector<int>(3, 10);
		IVector<int> sourceVector = new Vector<int>(2, i => i + 20);

		// Act
		vector.AddBackRange(sourceVector.Begin, sourceVector.End);

		// Assert
		Assert.IsTrue(vector.Size == 5);
		Assert.IsTrue(vector.At(3) == 20);
		Assert.IsTrue(vector.At(4) == 21);
	}

	[TestMethod]
	public void AddBackRange_TriggersResize()
	{
		// Arrange
		IVector<int> vector = new Vector<int>(3, 10);
		IVector<int> sourceVector = new Vector<int>(5, 1);
		int initialReserve = vector.ReserveSize;

		// Act
		vector.AddBackRange(sourceVector.Begin, sourceVector.End);

		// Assert
		Assert.IsTrue(vector.Size == 8);
		Assert.IsTrue(vector.ReserveSize > initialReserve);
	}

	[TestMethod]
	public void AddBackRange_WithEqualSizes_TriggersResize()
	{
		// Arrange
		IVector<int> vector = new Vector<int>(4, 1); // Fills initial reserve
		IVector<int> sourceVector = new Vector<int>(4, 2);
		int initialReserve = vector.ReserveSize;

		// Act
		vector.AddBackRange(sourceVector.Begin, sourceVector.End);

		// Assert
		Assert.IsTrue(vector.Size == 8);
		Assert.IsTrue(vector.ReserveSize > initialReserve);
	}

	//
	// Tests for AddBack(T value) - (10 tests)
	//
	[TestMethod]
	public void AddBack_ToEmptyVector_AddsElement2()
	{
		IVector<int> vector = new Vector<int>();
		vector.AddBack(100);
		Assert.IsTrue(vector.Size == 1);
		Assert.IsTrue(vector.Front == 100);
	}

	[TestMethod]
	public void AddBack_ToExistingVector_AddsElementToEnd2()
	{
		IVector<int> vector = new Vector<int>(3, 10);
		vector.AddBack(20);
		Assert.IsTrue(vector.Size == 4);
		Assert.IsTrue(vector.Back == 20);
	}

	[TestMethod]
	public void AddBack_FirstElement_SetsFrontAndBack()
	{
		IVector<int> vector = new Vector<int>();
		vector.AddBack(123);
		Assert.IsTrue(vector.Front == 123);
		Assert.IsTrue(vector.Back == 123);
	}

	[TestMethod]
	public void AddBack_MultipleElements_CheckOrderAndValues()
	{
		IVector<int> vector = new Vector<int>();
		vector.AddBack(1);
		vector.AddBack(2);
		vector.AddBack(3);
		Assert.IsTrue(vector.Size == 3);
		Assert.IsTrue(vector.At(0) == 1);
		Assert.IsTrue(vector.At(1) == 2);
		Assert.IsTrue(vector.At(2) == 3);
	}

	[TestMethod]
	public void AddBack_NegativeValue_AddsCorrectly()
	{
		IVector<int> vector = new Vector<int>();
		vector.AddBack(-50);
		Assert.IsTrue(vector.Size == 1);
		Assert.IsTrue(vector.Front == -50);
	}

	[TestMethod]
	public void AddBack_ZeroValue_AddsCorrectly()
	{
		IVector<int> vector = new Vector<int>();
		vector.AddBack(0);
		Assert.IsTrue(vector.Size == 1);
		Assert.IsTrue(vector.Front == 0);
	}

	[TestMethod]
	public void AddBack_TriggersResize()
	{
		IVector<int> vector = new Vector<int>();
		int initialReserve = vector.ReserveSize;
		vector.AddBack(1); vector.AddBack(2); vector.AddBack(3); vector.AddBack(4);
		vector.AddBack(5); // This should trigger a resize
		Assert.IsTrue(vector.Size == 5);
		Assert.IsTrue(vector.ReserveSize > initialReserve);
	}

	[TestMethod]
	public void AddBack_AfterResize_AddsCorrectly()
	{
		IVector<int> vector = new Vector<int>();
		vector.AddBack(1); vector.AddBack(2); vector.AddBack(3); vector.AddBack(4);
		vector.AddBack(5);
		vector.AddBack(6);
		Assert.IsTrue(vector.Size == 6);
		Assert.IsTrue(vector.Back == 6);
	}

	[TestMethod]
	public void AddBack_MultipleTimes_CorrectFinalState()
	{
		IVector<int> vector = new Vector<int>();
		int addCount = 20;
		for (int i = 0; i < addCount; i++)
		{
			vector.AddBack(i);
		}
		Assert.IsTrue(vector.Size == addCount);
		Assert.IsTrue(vector.Front == 0);
		Assert.IsTrue(vector.Back == 19);
	}

	[TestMethod]
	public void AddBack_LargeNumber_MaintainsOrder()
	{
		IVector<int> vector = new Vector<int>();
		int largeCount = 10000;
		for (int i = 0; i < largeCount; i++)
		{
			vector.AddBack(i);
		}
		Assert.IsTrue(vector.Size == largeCount);
		for (int i = 0; i < largeCount; i++)
		{
			Assert.IsTrue(vector.At(i) == i);
		}
	}


	//
	// Tests for AddBack(int count, T value) - (15 tests)
	//
	[TestMethod]
	public void AddBackMultiple_WithPositiveCount()
	{
		IVector<int> vector = new Vector<int>();
		vector.AddBack(5, 42);
		Assert.IsTrue(vector.Size == 5);
		for (int i = 0; i < 5; i++)
		{
			Assert.IsTrue(vector.At(i) == 42);
		}
	}

	[TestMethod]
	public void AddBackMultiple_ToExistingVector()
	{
		IVector<int> vector = new Vector<int>(3, 10);
		vector.AddBack(2, 20);
		Assert.IsTrue(vector.Size == 5);
		Assert.IsTrue(vector.Back == 20);
	}

	[TestMethod]
	public void AddBackMultiple_ZeroCount_NoChange()
	{
		IVector<int> vector = new Vector<int>(3, 10);
		int initialSize = vector.Size;
		int initialReserve = vector.ReserveSize;
		vector.AddBack(0, 99);
		Assert.IsTrue(vector.Size == initialSize);
		Assert.IsTrue(vector.ReserveSize == initialReserve);
	}

	[TestMethod]
	public void AddBackMultiple_CountExceedsCapacity_TriggersResize()
	{
		IVector<int> vector = new Vector<int>(2, 10); // Initial reserve is 4
		int initialReserve = vector.ReserveSize;
		vector.AddBack(3, 20); // 3 + 2 = 5, exceeds reserve
		Assert.IsTrue(vector.Size == 5);
		Assert.IsTrue(vector.ReserveSize > initialReserve);
	}

	[TestMethod]
	public void AddBackMultiple_LargeCount_TriggersMultipleResizes()
	{
		IVector<int> vector = new Vector<int>();
		vector.AddBack(100, 1);
		Assert.IsTrue(vector.Size == 100);
		Assert.IsTrue(vector.Back == 1);
	}

	[TestMethod]
	public void AddBackMultiple_LargeCountToExistingVector()
	{
		IVector<int> vector = new Vector<int>(10, 10);
		vector.AddBack(100, 20);
		Assert.IsTrue(vector.Size == 110);
		Assert.IsTrue(vector.At(9) == 10);
		Assert.IsTrue(vector.Back == 20);
	}

	[TestMethod]
	public void AddBackMultiple_WithNegativeValue()
	{
		IVector<int> vector = new Vector<int>();
		vector.AddBack(5, -1);
		Assert.IsTrue(vector.Size == 5);
		Assert.IsTrue(vector.At(0) == -1);
		Assert.IsTrue(vector.At(4) == -1);
	}

	[TestMethod]
	public void AddBackMultiple_ZeroCountToEmptyVector()
	{
		IVector<int> vector = new Vector<int>();
		vector.AddBack(0, 10);
		Assert.IsTrue(vector.Size == 0);
		Assert.IsTrue(vector.IsEmpty);
	}

	[TestMethod]
	public void AddBackMultiple_CheckCorrectValuesAfterAddition()
	{
		IVector<int> vector = new Vector<int>();
		vector.AddBack(5, 10);
		vector.AddBack(3, 20);
		Assert.IsTrue(vector.Size == 8);
		Assert.IsTrue(vector.At(4) == 10);
		Assert.IsTrue(vector.At(5) == 20);
		Assert.IsTrue(vector.At(7) == 20);
	}

	[TestMethod]
	public void AddBackMultiple_ToVectorWithMixedData()
	{
		IVector<int> vector = new Vector<int>(5, i => i);
		vector.AddBack(3, 100);
		Assert.IsTrue(vector.Size == 8);
		Assert.IsTrue(vector.At(4) == 4);
		Assert.IsTrue(vector.At(5) == 100);
	}

	[TestMethod]
	public void AddBackMultiple_VeryLargeCount_PopulatesCorrectly()
	{
		IVector<int> vector = new Vector<int>();
		vector.AddBack(50000, 123);
		Assert.IsTrue(vector.Size == 50000);
		Assert.IsTrue(vector.Back == 123);
		Assert.IsTrue(vector.Front == 123);
	}

	[TestMethod]
	public void AddBackMultiple_CountOne_BehavesLikeSingleElement()
	{
		IVector<int> vector = new Vector<int>();
		vector.AddBack(1, 99);
		Assert.IsTrue(vector.Size == 1);
		Assert.IsTrue(vector.Front == 99);
	}

	[TestMethod]
	public void AddBackMultiple_AfterSingleElementAdd()
	{
		IVector<int> vector = new Vector<int>();
		vector.AddBack(10);
		vector.AddBack(3, 20);
		Assert.IsTrue(vector.Size == 4);
		Assert.IsTrue(vector.Front == 10);
		Assert.IsTrue(vector.Back == 20);
	}

	[TestMethod]
	public void AddBackMultiple_InSequence_CorrectFinalState()
	{
		IVector<int> vector = new Vector<int>();
		vector.AddBack(2, 1);
		vector.AddBack(3, 2);
		vector.AddBack(5, 3);
		Assert.IsTrue(vector.Size == 10);
		Assert.IsTrue(vector.At(1) == 1);
		Assert.IsTrue(vector.At(2) == 2);
		Assert.IsTrue(vector.At(4) == 2);
		Assert.IsTrue(vector.At(5) == 3);
		Assert.IsTrue(vector.Back == 3);
	}


	//
	// Tests for AddBack(int count, Func<int, T> factory) - (15 tests)
	//
	[TestMethod]
	public void AddBackWithFactory_WithPositiveCount()
	{
		IVector<int> vector = new Vector<int>();
		vector.AddBack(5, i => i * 10);
		Assert.IsTrue(vector.Size == 5);
		Assert.IsTrue(vector.At(0) == 0);
		Assert.IsTrue(vector.At(4) == 40);
	}

	[TestMethod]
	public void AddBackWithFactory_ToExistingVector()
	{
		IVector<int> vector = new Vector<int>(3, 10);
		vector.AddBack(2, i => 20 + i);
		Assert.IsTrue(vector.Size == 5);
		Assert.IsTrue(vector.At(3) == 20);
		Assert.IsTrue(vector.At(4) == 21);
	}

	[TestMethod]
	public void AddBackWithFactory_ZeroCount_NoChange()
	{
		IVector<int> vector = new Vector<int>(3, 10);
		int initialSize = vector.Size;
		int initialReserve = vector.ReserveSize;
		vector.AddBack(0, i => i);
		Assert.IsTrue(vector.Size == initialSize);
		Assert.IsTrue(vector.ReserveSize == initialReserve);
	}

	[TestMethod]
	public void AddBackWithFactory_CountExceedsCapacity_TriggersResize()
	{
		IVector<int> vector = new Vector<int>(2, 10);
		int initialReserve = vector.ReserveSize;
		vector.AddBack(3, i => i);
		Assert.IsTrue(vector.Size == 5);
		Assert.IsTrue(vector.ReserveSize > initialReserve);
	}

	[TestMethod]
	public void AddBackWithFactory_LargeCount_PopulatesCorrectly()
	{
		IVector<int> vector = new Vector<int>();
		vector.AddBack(10000, i => i % 10);
		Assert.IsTrue(vector.Size == 10000);
		Assert.IsTrue(vector.At(0) == 0);
		Assert.IsTrue(vector.At(9) == 9);
		Assert.IsTrue(vector.At(10) == 0);
	}

	[TestMethod]
	public void AddBackWithFactory_AfterSingleElementAdd()
	{
		IVector<int> vector = new Vector<int>();
		vector.AddBack(100);
		vector.AddBack(5, i => i * 2);
		Assert.IsTrue(vector.Size == 6);
		Assert.IsTrue(vector.Front == 100);
		Assert.IsTrue(vector.Back == 8);
	}

	[TestMethod]
	public void AddBackWithFactory_UsingIndexInCalculation()
	{
		IVector<int> vector = new Vector<int>();
		vector.AddBack(4, i => (i * i));
		Assert.IsTrue(vector.At(0) == 0);
		Assert.IsTrue(vector.At(1) == 1);
		Assert.IsTrue(vector.At(2) == 4);
		Assert.IsTrue(vector.At(3) == 9);
	}

	[TestMethod]
	public void AddBackWithFactory_ComplexCalculation()
	{
		IVector<int> vector = new Vector<int>();
		vector.AddBack(5, i => i * 2 + 1);
		Assert.IsTrue(vector.At(0) == 1);
		Assert.IsTrue(vector.At(1) == 3);
		Assert.IsTrue(vector.At(2) == 5);
	}

	[TestMethod]
	public void AddBackWithFactory_AfterMultipleElementsAdd()
	{
		IVector<int> vector = new Vector<int>();
		vector.AddBack(3, 100);
		vector.AddBack(3, i => i);
		Assert.IsTrue(vector.Size == 6);
		Assert.IsTrue(vector.At(2) == 100);
		Assert.IsTrue(vector.At(3) == 0);
		Assert.IsTrue(vector.Back == 2);
	}

	[TestMethod]
	public void AddBackWithFactory_VeryLargeCount_ToExistingVector()
	{
		IVector<int> vector = new Vector<int>(10, 50);
		vector.AddBack(20000, i => i + 100);
		Assert.IsTrue(vector.Size == 20010);
		Assert.IsTrue(vector.At(9) == 50);
		Assert.IsTrue(vector.At(10) == 100);
		Assert.IsTrue(vector.Back == 20100 - 1); // last index
	}

	[TestMethod]
	public void AddBackWithFactory_AddingFromZeroCount()
	{
		IVector<int> vector = new Vector<int>();
		vector.AddBack(3, i => i);
		Assert.IsTrue(vector.Size == 3);
		Assert.IsTrue(vector.At(0) == 0);
		Assert.IsTrue(vector.At(1) == 1);
		Assert.IsTrue(vector.At(2) == 2);
	}

	[TestMethod]
	public void AddBackWithFactory_AddingFromNonZeroCount()
	{
		IVector<int> vector = new Vector<int>(5, i => i);
		vector.AddBack(3, i => i + 5);
		Assert.IsTrue(vector.Size == 8);
		Assert.IsTrue(vector.At(5) == 5);
		Assert.IsTrue(vector.At(6) == 6);
		Assert.IsTrue(vector.At(7) == 7);
	}


	[TestMethod]
	public void AddBackWithFactory_AddsDifferentDataTypes()
	{
		IVector<double> vector = new Vector<double>();
		vector.AddBack(4, i => (double)i / 2);
		Assert.IsTrue(vector.Size == 4);
		Assert.IsTrue(vector.At(0) == 0.0);
		Assert.IsTrue(vector.At(1) == 0.5);
	}
}

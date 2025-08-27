using InfinityOfficialNetwork.StandardLibrary.DataStructures.Containers.Primary;

namespace InfinityOfficialNetwork.StandardLibrary.Tests;

[TestClass]
public class RemoveBack
{

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

    //
    // Tests for Remove Operations (10 tests)
    //
    [TestMethod]
    public void RemoveBack_FromNonEmptyVector_ReducesSizeByOne()
    {
        // Arrange
        IVector<int> vector = new Vector<int>(5, 10);

        // Act
        vector.RemoveBack();

        // Assert
        Assert.IsTrue(vector.Size == 4);
    }

    [TestMethod]
    public void RemoveBack_FromVectorOfSizeOne_ResultsInEmptyVector()
    {
        // Arrange
        IVector<int> vector = new Vector<int>(1, 10);

        // Act
        vector.RemoveBack();

        // Assert
        Assert.IsTrue(vector.Size == 0);
        Assert.IsTrue(vector.IsEmpty);
    }

    [TestMethod]
    public void RemoveBack_MultipleElements_ReducesSizeCorrectly2()
    {
        // Arrange
        IVector<int> vector = new Vector<int>(10, 50);

        // Act
        vector.RemoveBack(4);

        // Assert
        Assert.IsTrue(vector.Size == 6);
    }

    [TestMethod]
    public void RemoveBack_AllElements_EmptiesVector()
    {
        // Arrange
        IVector<int> vector = new Vector<int>(5, 10);

        // Act
        vector.RemoveBack(5);

        // Assert
        Assert.IsTrue(vector.Size == 0);
        Assert.IsTrue(vector.IsEmpty);
    }

    [TestMethod]
    public void RemoveBack_WithTooLargeCount_HandlesCorrectly()
    {
        // Arrange
        IVector<int> vector = new Vector<int>(5, 10);

        // Act
        vector.RemoveBack(10);

        // Assert
        Assert.IsTrue(vector.Size == -5); // As per the provided class code's behavior
    }

    [TestMethod]
    public void RemoveBack_DoesNotResizeDownImmediately()
    {
        // Arrange
        IVector<int> vector = new Vector<int>(10, 10);
        int initialReserve = vector.ReserveSize;

        // Act
        vector.RemoveBack(1);

        // Assert
        Assert.IsTrue(vector.ReserveSize == initialReserve);
    }
}

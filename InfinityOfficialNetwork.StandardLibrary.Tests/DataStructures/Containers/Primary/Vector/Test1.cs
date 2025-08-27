using InfinityOfficialNetwork.StandardLibrary.DataStructures.Containers.Primary;

namespace InfinityOfficialNetwork.StandardLibrary.Tests;

[TestClass]
public class Test1
{



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




    [TestMethod]
    public void Clear_EmptiesVector2()
    {
        // Arrange
        IVector<int> vector = new Vector<int>(10, 10);

        // Act
        vector.Clear();

        // Assert
        Assert.IsTrue(vector.Size == 0);
        Assert.IsTrue(vector.IsEmpty);
    }

    [TestMethod]
    public void Clear_OnEmptyVector_DoesNothing()
    {
        // Arrange
        IVector<int> vector = new Vector<int>();

        // Act
        vector.Clear();

        // Assert
        Assert.IsTrue(vector.Size == 0);
        Assert.IsTrue(vector.IsEmpty);
    }

    [TestMethod]
    public void Clear_ResetsCapacity()
    {
        // Arrange
        IVector<int> vector = new Vector<int>(10, 10);
        int increasedReserve = vector.ReserveSize;

        // Act
        vector.Clear();

        // Assert
        Assert.IsTrue(vector.ReserveSize == 4); // The default initial reserve
    }


    //
    // Tests for Capacity Management (10 tests)
    //
    [TestMethod]
    public void Reserve_WithLargerSize_IncreasesReserve()
    {
        // Arrange
        IVector<int> vector = new Vector<int>(5, 10);
        int initialReserve = vector.ReserveSize;
        int newReserve = 20;

        // Act
        vector.Reserve(newReserve);

        // Assert
        Assert.IsTrue(vector.Size == 5);
        Assert.IsTrue(vector.ReserveSize >= newReserve);
    }

    [TestMethod]
    public void Reserve_WithSmallerSize_DoesNotChangeReserve()
    {
        // Arrange
        IVector<int> vector = new Vector<int>(10, 10);
        int initialReserve = vector.ReserveSize;
        int newReserve = 5;

        // Act
        vector.Reserve(newReserve);

        // Assert
        Assert.IsTrue(vector.Size == 10);
        Assert.IsTrue(vector.ReserveSize == initialReserve);
    }

    [TestMethod]
    public void Reserve_ToSameSize_DoesNotChangeReserve()
    {
        // Arrange
        IVector<int> vector = new Vector<int>(5, 10);
        int initialReserve = vector.ReserveSize;

        // Act
        vector.Reserve(initialReserve);

        // Assert
        Assert.IsTrue(vector.ReserveSize == initialReserve);
    }

    [TestMethod]
    public void Reserve_WithZeroSize_DoesNotChangeReserve()
    {
        // Arrange
        IVector<int> vector = new Vector<int>(5, 10);
        int initialReserve = vector.ReserveSize;

        // Act
        vector.Reserve(0);

        // Assert
        Assert.IsTrue(vector.ReserveSize == initialReserve);
    }

    [TestMethod]
    public void Reserve_EmptyVector_WithLargerSize_IncreasesReserve()
    {
        // Arrange
        IVector<int> vector = new Vector<int>();
        int newReserve = 20;

        // Act
        vector.Reserve(newReserve);

        // Assert
        Assert.IsTrue(vector.Size == 0);
        Assert.IsTrue(vector.ReserveSize >= newReserve);
    }

    [TestMethod]
    public void ShrinkToFit_WithExcessCapacity_ReducesReserve()
    {
        // Arrange
        IVector<int> vector = new Vector<int>(10, 10);
        vector.RemoveBack(5);
        Assert.IsTrue(vector.Size < vector.ReserveSize);

        // Act
        vector.ShrinkToFit();

        // Assert
        Assert.IsTrue(vector.ReserveSize == vector.Size);
    }

    [TestMethod]
    public void ShrinkToFit_WithNoExcessCapacity_DoesNotChangeReserve()
    {
        // Arrange
        IVector<int> vector = new Vector<int>(5, 10);
        vector.ShrinkToFit();
        int initialReserve = vector.ReserveSize;

        // Act
        vector.ShrinkToFit();

        // Assert
        Assert.IsTrue(vector.ReserveSize == initialReserve);
    }

    [TestMethod]
    public void ShrinkToFit_EmptyVector_ReducesToZeroReserve()
    {
        // Arrange
        IVector<int> vector = new Vector<int>();

        // Act
        vector.ShrinkToFit();

        // Assert
        Assert.IsTrue(vector.ReserveSize == 0);
    }

    [TestMethod]
    public void IsEmpty_OnEmptyVector_ReturnsTrue()
    {
        // Arrange
        IVector<int> vector = new Vector<int>();

        // Assert
        Assert.IsTrue(vector.IsEmpty);
    }

    [TestMethod]
    public void IsEmpty_OnNonEmptyVector_ReturnsFalse()
    {
        // Arrange
        IVector<int> vector = new Vector<int>(1, 1);

        // Assert
        Assert.IsTrue(!vector.IsEmpty);
    }


    // A simple unmanaged struct for testing purposes.
    private struct Point
    {
        public int x;
        public int y;
    }

    //
    // Tests for RemoveBack() - (2 tests)
    //
    [TestMethod]
    public void RemoveBack_FromNonEmptyVector_DecreasesSize()
    {
        // Arrange
        var vector = new Vector<Point>();
        vector.AddBack(new Point { x = 1, y = 1 });
        vector.AddBack(new Point { x = 2, y = 2 });
        int initialSize = vector.Size;

        // Act
        vector.RemoveBack();

        // Assert
        Assert.AreEqual(initialSize - 1, vector.Size);
        Assert.AreEqual(1, vector.At(0).x);
    }

    //
    // Tests for Reserve(int newReserveSize) - (2 tests)
    //
    [TestMethod]
    public void Reserve_IncreasesReserveSize_WhenNewSizeIsLarger()
    {
        // Arrange
        var vector = new Vector<Point>();
        vector.AddBack(new Point { x = 1, y = 1 });
        int initialReserve = vector.ReserveSize;

        // Act
        vector.Reserve(initialReserve + 10);

        // Assert
        Assert.AreEqual(initialReserve + 10, vector.ReserveSize);
        Assert.AreEqual(1, vector.Size);
    }

    [TestMethod]
    public void Reserve_DoesNotDecreaseReserveSize_WhenNewSizeIsSmaller()
    {
        // Arrange
        var vector = new Vector<Point>();
        vector.Reserve(10);
        int initialReserve = vector.ReserveSize;

        // Act
        vector.Reserve(5);

        // Assert
        Assert.AreEqual(initialReserve, vector.ReserveSize);
        Assert.AreEqual(0, vector.Size);
    }

    //
    // Tests for Resize(int newSize, T value) - (2 tests)
    //
    [TestMethod]
    public void Resize_ToLargerSize_AddsReserve()
    {
        // Arrange
        var vector = new Vector<Point>();
        vector.AddBack(20, new Point { x = 1, y = 1 });
        int initialSize = vector.Size;
        Point defaultValue = new Point { x = 99, y = 99 };

        // Act
        vector.Resize(initialSize + 2);

        // Assert
        Assert.AreEqual(initialSize, vector.Size);
        Assert.AreEqual(initialSize + 2, vector.ReserveSize);
        Assert.AreEqual(1, vector.At(0).x);
    }

    [TestMethod]
    public void Resize_ToSmallerSize_RemovesElements()
    {
        // Arrange
        var vector = new Vector<Point>();
        vector.AddBack(new Point { x = 1, y = 1 });
        vector.AddBack(new Point { x = 2, y = 2 });
        vector.AddBack(new Point { x = 3, y = 3 });

        // Act
        vector.Resize(2);

        // Assert
        Assert.AreEqual(2, vector.Size);
        Assert.AreEqual(1, vector.At(0).x);
        Assert.AreEqual(2, vector.At(1).x);
    }

    //
    // Tests for Clear() - (2 tests)
    //
    [TestMethod]
    public void Clear_FromNonEmptyVector_SetsSizeToZero()
    {
        // Arrange
        var vector = new Vector<Point>();
        vector.AddBack(new Point { x = 1, y = 1 });

        // Act
        vector.Clear();

        // Assert
        Assert.AreEqual(0, vector.Size);
        Assert.IsTrue(vector.IsEmpty);
    }

    [TestMethod]
    public void Clear_FromEmptyVector_DoesNothing()
    {
        // Arrange
        var vector = new Vector<Point>();

        // Act
        vector.Clear();

        // Assert
        Assert.AreEqual(0, vector.Size);
        Assert.IsTrue(vector.IsEmpty);
    }

    //
    // Tests for ShrinkToFit() - (2 tests)
    //
    [TestMethod]
    public void ShrinkToFit_DecreasesReserveSize_WhenReserveIsLargerThanSize()
    {
        // Arrange
        var vector = new Vector<Point>();
        vector.Reserve(10);
        vector.AddBack(new Point { x = 1, y = 1 });
        vector.AddBack(new Point { x = 2, y = 2 });
        Assert.IsTrue(vector.Size < vector.ReserveSize);

        // Act
        vector.ShrinkToFit();

        // Assert
        Assert.AreEqual(vector.Size, vector.ReserveSize);
        Assert.AreEqual(2, vector.Size);
    }

    [TestMethod]
    public void ShrinkToFit_DoesNotChangeReserveSize_WhenReserveEqualsSize()
    {
        // Arrange
        var vector = new Vector<Point>();
        vector.AddBack(new Point { x = 1, y = 1 });
        vector.AddBack(new Point { x = 2, y = 2 });

        // Act
        vector.ShrinkToFit();

        // Assert
        Assert.AreEqual(vector.Size, vector.ReserveSize);
        Assert.AreEqual(2, vector.Size);
    }
}

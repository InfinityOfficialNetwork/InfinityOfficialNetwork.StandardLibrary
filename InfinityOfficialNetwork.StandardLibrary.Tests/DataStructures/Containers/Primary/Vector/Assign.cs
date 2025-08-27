using InfinityOfficialNetwork.StandardLibrary.DataStructures.Containers.Primary;

namespace InfinityOfficialNetwork.StandardLibrary.Tests;

[TestClass]
public class Assign
{
    //
    // Tests for Assign(int count, T value) - (25 tests)
    //
    [TestMethod]
    public void Assign_ToEmptyVector_CorrectSizeAndValue()
    {
        IVector<int> vector = new Vector<int>();
        vector.Assign(5, 42);
        Assert.IsTrue(vector.Size == 5);
        for (int i = 0; i < 5; i++)
        {
            Assert.IsTrue(vector.At(i) == 42);
        }
    }

    [TestMethod]
    public void Assign_ToSmallerVector_IncreasesSize()
    {
        IVector<int> vector = new Vector<int>(3, 10);
        vector.Assign(5, 42);
        Assert.IsTrue(vector.Size == 5);
        Assert.IsTrue(vector.At(0) == 42);
        Assert.IsTrue(vector.At(4) == 42);
    }

    [TestMethod]
    public void Assign_ToLargerVector_DecreasesSize()
    {
        IVector<int> vector = new Vector<int>(8, 10);
        vector.Assign(5, 42);
        Assert.IsTrue(vector.Size == 5);
        Assert.IsTrue(vector.At(0) == 42);
        Assert.IsTrue(vector.At(4) == 42);
    }

    [TestMethod]
    public void Assign_ZeroCount_ClearsVector()
    {
        IVector<int> vector = new Vector<int>(5, 10);
        vector.Assign(0, 42);
        Assert.IsTrue(vector.Size == 0);
        Assert.IsTrue(vector.IsEmpty);
    }

    [TestMethod]
    public void Assign_ZeroCountToEmptyVector_RemainsEmpty()
    {
        IVector<int> vector = new Vector<int>();
        vector.Assign(0, 42);
        Assert.IsTrue(vector.Size == 0);
        Assert.IsTrue(vector.IsEmpty);
    }

    [TestMethod]
    public void Assign_SameSize_OverwritesValues()
    {
        IVector<int> vector = new Vector<int>(5, 10);
        vector.Assign(5, 42);
        Assert.IsTrue(vector.Size == 5);
        Assert.IsTrue(vector.At(0) == 42);
        Assert.IsTrue(vector.At(4) == 42);
    }

    [TestMethod]
    public void Assign_LargeCount_CorrectSizeAndValue()
    {
        IVector<int> vector = new Vector<int>();
        vector.Assign(10000, 99);
        Assert.IsTrue(vector.Size == 10000);
        Assert.IsTrue(vector.Front == 99);
        Assert.IsTrue(vector.Back == 99);
    }

    [TestMethod]
    public void Assign_CountExceedsCapacity_TriggersResize()
    {
        IVector<int> vector = new Vector<int>(2, 10); // Initial reserve might be 4
        int initialReserve = vector.ReserveSize;
        vector.Assign(10, 20); // Assigning 10 elements, exceeds initial capacity
        Assert.IsTrue(vector.Size == 10);
        Assert.IsTrue(vector.ReserveSize >= 10);
    }

    [TestMethod]
    public void Assign_AfterResize_CorrectState()
    {
        IVector<int> vector = new Vector<int>();
        vector.Assign(5, 10);
        vector.Assign(3, 20);
        Assert.IsTrue(vector.Size == 3);
        Assert.IsTrue(vector.Front == 20);
    }

    [TestMethod]
    public void Assign_ToVectorWithNegativeValues()
    {
        IVector<int> vector = new Vector<int>(3, -10);
        vector.Assign(2, -50);
        Assert.IsTrue(vector.Size == 2);
        Assert.IsTrue(vector.Front == -50);
        Assert.IsTrue(vector.Back == -50);
    }

    [TestMethod]
    public void Assign_NegativeValue_OverwritesPositives()
    {
        IVector<int> vector = new Vector<int>(5, 10);
        vector.Assign(5, -1);
        Assert.IsTrue(vector.Size == 5);
        Assert.IsTrue(vector.At(0) == -1);
        Assert.IsTrue(vector.At(4) == -1);
    }

    [TestMethod]
    public void Assign_WithSameCountButDifferentValue()
    {
        IVector<int> vector = new Vector<int>(5, 10);
        vector.Assign(5, 20);
        Assert.IsTrue(vector.Size == 5);
        Assert.IsTrue(vector.At(0) == 20);
        Assert.IsTrue(vector.At(4) == 20);
    }

    [TestMethod]
    public void Assign_Twice_FinalStateIsCorrect()
    {
        IVector<int> vector = new Vector<int>();
        vector.Assign(10, 10);
        vector.Assign(5, 5);
        Assert.IsTrue(vector.Size == 5);
        Assert.IsTrue(vector.Front == 5);
        Assert.IsTrue(vector.Back == 5);
    }

    [TestMethod]
    public void Assign_LargeCountToLargerVector()
    {
        IVector<int> vector = new Vector<int>(20000, 1);
        vector.Assign(10000, 2);
        Assert.IsTrue(vector.Size == 10000);
        Assert.IsTrue(vector.Front == 2);
        Assert.IsTrue(vector.Back == 2);
    }

    [TestMethod]
    public void Assign_LargeCountToSmallerVector()
    {
        IVector<int> vector = new Vector<int>(5, 1);
        vector.Assign(10000, 2);
        Assert.IsTrue(vector.Size == 10000);
        Assert.IsTrue(vector.Front == 2);
        Assert.IsTrue(vector.Back == 2);
    }

    [TestMethod]
    public void Assign_ToEmptyVector_TriggersResize()
    {
        IVector<int> vector = new Vector<int>();
        int initialReserve = vector.ReserveSize;
        vector.Assign(10, 1);
        Assert.IsTrue(vector.Size == 10);
        Assert.IsTrue(vector.ReserveSize >= 10);
    }

    [TestMethod]
    public void Assign_FromVectorWithDifferentType()
    {
        IVector<double> vector = new Vector<double>(3, 1.1);
        vector.Assign(2, 2.2);
        Assert.IsTrue(vector.Size == 2);
        Assert.IsTrue(vector.At(0) == 2.2);
    }

    [TestMethod]
    public void Assign_ToVectorWithZeroCapacity()
    {
        IVector<int> vector = new Vector<int>();
        vector.Assign(5, 10);
        Assert.IsTrue(vector.Size == 5);
        Assert.IsTrue(vector.At(0) == 10);
    }

    [TestMethod]
    public void Assign_AfterAddBack()
    {
        IVector<int> vector = new Vector<int>();
        vector.AddBack(10);
        vector.Assign(3, 20);
        Assert.IsTrue(vector.Size == 3);
        Assert.IsTrue(vector.Front == 20);
        Assert.IsTrue(vector.Back == 20);
    }

    [TestMethod]
    public void Assign_WithSmallestPossibleCount()
    {
        IVector<int> vector = new Vector<int>(1, 100);
        vector.Assign(1, 200);
        Assert.IsTrue(vector.Size == 1);
        Assert.IsTrue(vector.Front == 200);
    }

    [TestMethod]
    public void Assign_WithLargeCount_AndDifferentReserve()
    {
        IVector<int> vector = new Vector<int>();
        vector.Reserve(50);
        int initialReserve = vector.ReserveSize;
        vector.Assign(50, 50);
        Assert.IsTrue(vector.Size == 50);
        Assert.IsTrue(vector.ReserveSize == initialReserve);
    }

    [TestMethod]
    public void Assign_AfterMultipleAdds()
    {
        IVector<int> vector = new Vector<int>();
        for (int i = 0; i < 10; i++) vector.AddBack(i);
        vector.Assign(5, 99);
        Assert.IsTrue(vector.Size == 5);
        Assert.IsTrue(vector.At(0) == 99);
        Assert.IsTrue(vector.At(4) == 99);
    }
}

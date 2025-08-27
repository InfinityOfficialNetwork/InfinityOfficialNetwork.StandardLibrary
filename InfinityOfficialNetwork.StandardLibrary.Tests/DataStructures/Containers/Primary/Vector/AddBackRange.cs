using InfinityOfficialNetwork.StandardLibrary.DataStructures.Containers.Primary;

namespace InfinityOfficialNetwork.StandardLibrary.Tests;

[TestClass]
public class AddBackRange
{
    // A private helper method to quickly create a vector with content.
    private IVector<int> CreateVector(int count, int value)
    {
        IVector<int> vector = new Vector<int>();
        vector.AddBack(count, value);
        return vector;
    }

    

    //
    // Tests for AddBackRange(IIterator<T> first, IIterator<T> last) - (10 tests)
    //
    [TestMethod]
    public void AddBackRange_FromEmptyVector_NoChange()
    {
        IVector<int> vector = new Vector<int>(5, 1);
        IVector<int> sourceVector = new Vector<int>();
        int initialSize = vector.Size;
        vector.AddBackRange(sourceVector.Begin, sourceVector.End);
        Assert.IsTrue(vector.Size == initialSize);
    }

    [TestMethod]
    public void AddBackRange_FromVector_AddsElements()
    {
        IVector<int> vector = new Vector<int>(3, 10);
        IVector<int> sourceVector = new Vector<int>(2, i => i + 20);
        vector.AddBackRange(sourceVector.Begin, sourceVector.End);
        Assert.IsTrue(vector.Size == 5);
        Assert.IsTrue(vector.At(3) == 20);
        Assert.IsTrue(vector.At(4) == 21);
    }

    [TestMethod]
    public void AddBackRange_TriggersResize()
    {
        IVector<int> vector = new Vector<int>(3, 10);
        IVector<int> sourceVector = new Vector<int>(5, 1);
        int initialReserve = vector.ReserveSize;
        vector.AddBackRange(sourceVector.Begin, sourceVector.End);
        Assert.IsTrue(vector.Size == 8);
        Assert.IsTrue(vector.ReserveSize > initialReserve);
    }

    [TestMethod]
    public void AddBackRange_WithEqualSizes_TriggersResize()
    {
        IVector<int> vector = new Vector<int>(4, 1);
        IVector<int> sourceVector = new Vector<int>(4, 2);
        int initialReserve = vector.ReserveSize;
        vector.AddBackRange(sourceVector.Begin, sourceVector.End);
        Assert.IsTrue(vector.Size == 8);
        Assert.IsTrue(vector.ReserveSize > initialReserve);
    }

    [TestMethod]
    public void AddBackRange_CopiesValuesCorrectly()
    {
        IVector<int> vector = new Vector<int>();
        IVector<int> sourceVector = new Vector<int>(3, i => i);
        vector.AddBackRange(sourceVector.Begin, sourceVector.End);
        Assert.IsTrue(vector.Size == 3);
        Assert.IsTrue(vector.At(0) == 0);
        Assert.IsTrue(vector.At(1) == 1);
        Assert.IsTrue(vector.At(2) == 2);
    }

    [TestMethod]
    public void AddBackRange_ToEmptyVector()
    {
        IVector<int> vector = new Vector<int>();
        IVector<int> sourceVector = new Vector<int>(5, 10);
        vector.AddBackRange(sourceVector.Begin, sourceVector.End);
        Assert.IsTrue(vector.Size == 5);
        Assert.IsTrue(vector.Back == 10);
    }

    [TestMethod]
    public void AddBackRange_FromSubRange_CopiesCorrectly()
    {
        IVector<int> vector = new Vector<int>();
        IVector<int> sourceVector = new Vector<int>(5, i => i + 1);

        // Create an iterator for a sub-range (elements 2, 3, 4)
        var first = sourceVector.Begin;
        first.MoveNext();
        first.MoveNext();
        var last = sourceVector.End;

        vector.AddBackRange(first, last);
        Assert.IsTrue(vector.Size == 3);
        Assert.IsTrue(vector.At(0) == 3);
        Assert.IsTrue(vector.At(1) == 4);
        Assert.IsTrue(vector.At(2) == 5);
    }

    [TestMethod]
    public void AddBackRange_OverlappingWithSelf_CopiesCorrectly()
    {
        IVector<int> vector = new Vector<int>(4, i => i);
        var first = vector.Begin;
        first.MoveNext();
        first.MoveNext();
        var last = vector.End;

        vector.AddBackRange(first, last);
        Assert.IsTrue(vector.Size == 6);
        Assert.IsTrue(vector.Back == 3);
        Assert.IsTrue(vector.At(4) == 2);
    }

    [TestMethod]
    public void AddBackRange_LargeRange_CopiesAllElements()
    {
        IVector<int> vector = new Vector<int>();
        IVector<int> sourceVector = new Vector<int>(10000, 10);
        vector.AddBackRange(sourceVector.Begin, sourceVector.End);
        Assert.IsTrue(vector.Size == 10000);
        Assert.IsTrue(vector.Front == 10);
        Assert.IsTrue(vector.Back == 10);
    }

    [TestMethod]
    public void AddBackRange_LargeRange_ToNonEmptyVector()
    {
        IVector<int> vector = new Vector<int>(10, 5);
        IVector<int> sourceVector = new Vector<int>(10000, 10);
        vector.AddBackRange(sourceVector.Begin, sourceVector.End);
        Assert.IsTrue(vector.Size == 10010);
        Assert.IsTrue(vector.At(9) == 5);
        Assert.IsTrue(vector.At(10) == 10);
        Assert.IsTrue(vector.Back == 10);
    }
}

using InfinityOfficialNetwork.StandardLibrary.DataStructures.Containers.Primary;

namespace InfinityOfficialNetwork.StandardLibrary.Tests;

[TestClass]
public class AssignRange
{
    //
    // Tests for AssignRange(IVector<T> source) - (25 tests)
    //
    [TestMethod]
    public void AssignRange_ToEmptyVector_FromNonEmptySource()
    {
        IVector<int> vector = new Vector<int>();
        IVector<int> source = new Vector<int>(5, 10);
        vector.AssignRange(source);
        Assert.IsTrue(vector.Size == 5);
        Assert.IsTrue(vector.At(0) == 10);
        Assert.IsTrue(vector.At(4) == 10);
    }

    [TestMethod]
    public void AssignRange_ToNonEmptyVector_FromEmptySource_ClearsVector()
    {
        IVector<int> vector = new Vector<int>(5, 10);
        IVector<int> source = new Vector<int>();
        vector.AssignRange(source);
        Assert.IsTrue(vector.Size == 0);
        Assert.IsTrue(vector.IsEmpty);
    }

    [TestMethod]
    public void AssignRange_FromSmallerSource_DecreasesSize()
    {
        IVector<int> vector = new Vector<int>(5, 10);
        IVector<int> source = new Vector<int>(3, 20);
        vector.AssignRange(source);
        Assert.IsTrue(vector.Size == 3);
        Assert.IsTrue(vector.At(0) == 20);
        Assert.IsTrue(vector.At(2) == 20);
    }

    [TestMethod]
    public void AssignRange_FromLargerSource_IncreasesSizeAndTriggersResize()
    {
        IVector<int> vector = new Vector<int>(3, 10);
        IVector<int> source = new Vector<int>(8, 20);
        int initialReserve = vector.ReserveSize;
        vector.AssignRange(source);
        Assert.IsTrue(vector.Size == 8);
        Assert.IsTrue(vector.At(0) == 20);
        Assert.IsTrue(vector.At(7) == 20);
        Assert.IsTrue(vector.ReserveSize >= 8);
    }

    [TestMethod]
    public void AssignRange_FromSameSizeSource_OverwritesValues()
    {
        IVector<int> vector = new Vector<int>(5, 10);
        IVector<int> source = new Vector<int>(5, 20);
        vector.AssignRange(source);
        Assert.IsTrue(vector.Size == 5);
        Assert.IsTrue(vector.At(0) == 20);
        Assert.IsTrue(vector.At(4) == 20);
    }

    [TestMethod]
    public void AssignRange_FromSourceWithMixedValues()
    {
        IVector<int> vector = new Vector<int>();
        IVector<int> source = new Vector<int>();
        source.AddBack(1); source.AddBack(2); source.AddBack(3);
        vector.AssignRange(source);
        Assert.IsTrue(vector.Size == 3);
        Assert.IsTrue(vector.At(0) == 1);
        Assert.IsTrue(vector.At(1) == 2);
        Assert.IsTrue(vector.At(2) == 3);
    }

    [TestMethod]
    public void AssignRange_AfterOtherOperations()
    {
        IVector<int> vector = new Vector<int>(5, 5);
        IVector<int> source = new Vector<int>(3, 10);
        vector.AssignRange(source);
        Assert.IsTrue(vector.Size == 3);
        Assert.IsTrue(vector.Front == 10);
    }

    [TestMethod]
    public void AssignRange_LargeSource_ToSmallerVector_CopiesAllElements()
    {
        IVector<int> vector = new Vector<int>(5, 1);
        IVector<int> source = new Vector<int>(10000, 2);
        vector.AssignRange(source);
        Assert.IsTrue(vector.Size == 10000);
        Assert.IsTrue(vector.Front == 2);
        Assert.IsTrue(vector.Back == 2);
    }

    [TestMethod]
    public void AssignRange_SelfAssignment_NoChange()
    {
        IVector<int> vector = new Vector<int>(5, 10);
        vector.AssignRange(vector);
        Assert.IsTrue(vector.Size == 5);
        Assert.IsTrue(vector.At(0) == 10);
        Assert.IsTrue(vector.Back == 10);
    }

    [TestMethod]
    public void AssignRange_OverlappingWithSelf_CopiesCorrectly()
    {
        IVector<int> vector = new Vector<int>(5, i => i);
        var subVector = new Vector<int>();
        subVector.AddBack(vector.At(1));
        subVector.AddBack(vector.At(2));

        vector.AssignRange(subVector);
        Assert.IsTrue(vector.Size == 2);
        Assert.IsTrue(vector.At(0) == 1);
        Assert.IsTrue(vector.At(1) == 2);
    }

    [TestMethod]
    public void AssignRange_ToVectorWithZeroCapacity()
    {
        IVector<int> vector = new Vector<int>();
        IVector<int> source = new Vector<int>(5, 10);
        vector.AssignRange(source);
        Assert.IsTrue(vector.Size == 5);
        Assert.IsTrue(vector.At(0) == 10);
    }

    [TestMethod]
    public void AssignRange_SourceValuesCopied_NotReferenced()
    {
        IVector<int> vector = new Vector<int>();
        IVector<int> source = new Vector<int>(3, 10);
        vector.AssignRange(source);
        source.Assign(3, 20); // Modify source
        Assert.IsTrue(vector.At(0) == 10); // Destination should not be affected
    }

    [TestMethod]
    public void AssignRange_EmptySourceToEmptyVector_NoChange()
    {
        IVector<int> vector = new Vector<int>();
        IVector<int> source = new Vector<int>();
        vector.AssignRange(source);
        Assert.IsTrue(vector.Size == 0);
        Assert.IsTrue(vector.IsEmpty);
    }

    [TestMethod]
    public void AssignRange_WithLargeSource_ToLargeDestination()
    {
        IVector<int> vector = new Vector<int>(10000, 1);
        IVector<int> source = new Vector<int>(10000, 2);
        vector.AssignRange(source);
        Assert.IsTrue(vector.Size == 10000);
        Assert.IsTrue(vector.At(9999) == 2);
    }

    [TestMethod]
    public void AssignRange_WithMixedSourceData()
    {
        IVector<int> vector = new Vector<int>();
        IVector<int> source = new Vector<int>(10, i => i % 2);
        vector.AssignRange(source);
        Assert.IsTrue(vector.Size == 10);
        Assert.IsTrue(vector.At(0) == 0);
        Assert.IsTrue(vector.At(1) == 1);
    }

    [TestMethod]
    public void AssignRange_FromSource_ContainingNegativeValues()
    {
        IVector<int> vector = new Vector<int>();
        IVector<int> source = new Vector<int>(3, -10);
        vector.AssignRange(source);
        Assert.IsTrue(vector.Size == 3);
        Assert.IsTrue(vector.Front == -10);
    }

    [TestMethod]
    public void AssignRange_ChainCalls()
    {
        IVector<int> vector1 = new Vector<int>(5, 1);
        IVector<int> vector2 = new Vector<int>(3, 2);
        IVector<int> vector3 = new Vector<int>(7, 3);

        vector1.AssignRange(vector2);
        vector1.AssignRange(vector3);

        Assert.IsTrue(vector1.Size == 7);
        Assert.IsTrue(vector1.Front == 3);
        Assert.IsTrue(vector1.Back == 3);
    }

    [TestMethod]
    public void AssignRange_FromSourceCreatedWithAssign()
    {
        IVector<int> vector = new Vector<int>();
        IVector<int> source = new Vector<int>();
        source.Assign(5, 100);
        vector.AssignRange(source);
        Assert.IsTrue(vector.Size == 5);
        Assert.IsTrue(vector.Front == 100);
    }

    [TestMethod]
    public void AssignRange_FromSourceCreatedWithFactory()
    {
        IVector<int> vector = new Vector<int>();
        IVector<int> source = new Vector<int>(3, i => i + 1);
        vector.AssignRange(source);
        Assert.IsTrue(vector.Size == 3);
        Assert.IsTrue(vector.At(0) == 1);
        Assert.IsTrue(vector.At(1) == 2);
    }

    // New tests using unmanaged struct types
    private struct Point
    {
        public int x;
        public int y;
    }

    [TestMethod]
    public void AssignRange_ToVector_WithStructValues()
    {
        IVector<Point> vector = new Vector<Point>();
        IVector<Point> source = new Vector<Point>();
        source.AddBack(new Point { x = 1, y = 2 });
        source.AddBack(new Point { x = 3, y = 4 });
        vector.AssignRange(source);
        Assert.IsTrue(vector.Size == 2);
        Assert.IsTrue(vector.At(0).x == 1);
        Assert.IsTrue(vector.At(1).y == 4);
    }

    [TestMethod]
    public void AssignRange_FromLargerSource_WithStructs()
    {
        IVector<Point> vector = new Vector<Point>(1, new Point { x = 0, y = 0 });
        IVector<Point> source = new Vector<Point>();
        source.AddBack(new Point { x = 10, y = 20 });
        source.AddBack(new Point { x = 30, y = 40 });
        source.AddBack(new Point { x = 50, y = 60 });
        vector.AssignRange(source);
        Assert.IsTrue(vector.Size == 3);
        Assert.IsTrue(vector.At(0).x == 10);
        Assert.IsTrue(vector.At(2).y == 60);
    }

    [TestMethod]
    public void AssignRange_CopiesStructValues_NotReferenced()
    {
        IVector<Point> vector = new Vector<Point>();
        IVector<Point> source = new Vector<Point>();
        Point p = new Point { x = 100, y = 200 };
        source.AddBack(p);
        vector.AssignRange(source);
        p.x = 500; // Modify the original struct
        Assert.IsTrue(vector.At(0).x == 100); // The value in the vector should be unchanged
    }

    [TestMethod]
    public void AssignRange_ToVectorWithMixedData()
    {
        IVector<int> vector = new Vector<int>(5, i => i);
        IVector<int> source = new Vector<int>(3, 100);
        vector.AssignRange(source);
        Assert.IsTrue(vector.Size == 3);
        Assert.IsTrue(vector.Front == 100);
    }
}

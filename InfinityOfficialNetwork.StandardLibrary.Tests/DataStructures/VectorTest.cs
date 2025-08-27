using InfinityOfficialNetwork.StandardLibrary.DataStructures.Containers.Old;

namespace InfinityOfficialNetwork.StandardLibrary.Tests.DataStructures
{
	[TestClass]
	public sealed class VectorTest
	{
		// ====================================================================
		// Helper Methods and Struct
		// ====================================================================

		// Simple struct for testing
		private struct MyStruct : IEquatable<MyStruct>
		{
			public int Id;
			public bool IsActive;

			public MyStruct(int id, bool isActive)
			{
				Id = id;
				IsActive = isActive;
			}

			public bool Equals(MyStruct other)
			{
				return Id == other.Id && IsActive == other.IsActive;
			}

			public override int GetHashCode()
			{
				return HashCode.Combine(Id, IsActive);
			}
		}

		private struct StructWithReference
		{
			public int Id;
			public string Name;
		}

		// ====================================================================
		// Constructor & Basic Property Tests
		// ====================================================================

		[TestMethod]
		public void Constructor_InitializesEmptyVector()
		{
			using var vector = new Vector<int>();
			Assert.AreEqual(0, vector.Count);
			Assert.IsFalse(vector.IsReadOnly);
			Assert.IsTrue(vector.IsEmpty);
		}

		[TestMethod]
		public void Constructor_InitializesWithArray()
		{
			var data = new int[] { 1, 2, 3, 4, 5 };
			using var vector = new Vector<int>(data);
			Assert.AreEqual(5, vector.Count);
			Assert.IsFalse(vector.IsEmpty);
			for (int i = 0; i < data.Length; i++)
			{
				Assert.AreEqual(data[i], vector[i]);
			}
		}

		[TestMethod]
		public void Constructor_InitializesWithEmptyArray()
		{
			var data = Array.Empty<int>();
			using var vector = new Vector<int>(data);
			Assert.AreEqual(0, vector.Count);
			Assert.IsTrue(vector.IsEmpty);
		}

		[TestMethod]
		public void Constructor_InitializesWithAnotherVector()
		{
			using var originalVector = new Vector<MyStruct> { new MyStruct(1, true), new MyStruct(2, false) };
			using var newVector = new Vector<MyStruct>(originalVector);
			Assert.AreEqual(originalVector.Count, newVector.Count);
			Assert.AreEqual(1, newVector[0].Id);
			Assert.AreEqual(2, newVector[1].Id);
		}

		[TestMethod]
		public void Indexer_SetAndGet_WorksCorrectly()
		{
			using var vector = new Vector<int>();
			vector.Add(10);
			vector[0] = 20;
			Assert.AreEqual(20, vector[0]);
		}

		[TestMethod]
		public void Clear_ResetsCountAndReallocatesMemory()
		{
			using var vector = new Vector<int> { 1, 2 };
			vector.Clear();
			Assert.AreEqual(0, vector.Count);
			vector.Add(100);
			Assert.AreEqual(1, vector.Count);
			Assert.AreEqual(100, vector[0]);
		}

		[TestMethod]
		public void StaticConstructor_ThrowsExceptionForStructsWithReferences()
		{
			Assert.ThrowsException<TypeInitializationException>(() =>
			{
				// This line will trigger the static constructor and the check
				var vector = new Vector<StructWithReference>();
			});
		}

		[TestMethod]
		public void StaticConstructor_AllowsStructsWithoutReferences()
		{
			try
			{
				// This should not throw an exception
				using var vector = new Vector<MyStruct>();
			}
			catch (InvalidOperationException)
			{
				Assert.Fail("Vector should allow structs without references.");
			}
		}

		[TestMethod]
		public void Constructor_InitializesWithIEnumerable()
		{
			var data = new List<int> { 1, 2, 3, 4, 5 };
			using var vector = new Vector<int>(data);
			Assert.AreEqual(5, vector.Count);
			Assert.IsFalse(vector.IsEmpty);
			for (int i = 0; i < data.Count; i++)
			{
				Assert.AreEqual(data[i], vector[i]);
			}
		}

		// ====================================================================
		// Add/Remove/Insert Tests
		// ====================================================================

		[TestMethod]
		public void Add_AddsItemAndIncrementsCount()
		{
			using var vector = new Vector<int>();
			vector.Add(10);
			Assert.AreEqual(1, vector.Count);
			Assert.AreEqual(10, vector[0]);
		}

		[TestMethod]
		public void Insert_InsertsItemAtBeginning()
		{
			using var vector = new Vector<int> { 2 };
			vector.Insert(0, 1);
			Assert.AreEqual(2, vector.Count);
			Assert.AreEqual(1, vector[0]);
			Assert.AreEqual(2, vector[1]);
		}

		[TestMethod]
		public void Insert_InsertsItemInMiddle()
		{
			using var vector = new Vector<int> { 1, 3 };
			vector.Insert(1, 2);
			Assert.AreEqual(3, vector.Count);
			Assert.AreEqual(1, vector[0]);
			Assert.AreEqual(2, vector[1]);
			Assert.AreEqual(3, vector[2]);
		}

		[TestMethod]
		public void InsertBack_AddsItemToTheEnd()
		{
			using var vector = new Vector<int>();
			vector.Add(1);
			Assert.AreEqual(1, vector.Count);
			Assert.AreEqual(1, vector[0]);
		}

		[TestMethod]
		public void Remove_RemovesFirstMatchingItem()
		{
			using var vector = new Vector<int> { 1, 2, 3 };
			vector.Remove(2);
			Assert.AreEqual(2, vector.Count);
			Assert.AreEqual(1, vector[0]);
			Assert.AreEqual(3, vector[1]);
		}

		[TestMethod]
		public void Remove_RemovesDuplicateItemsOnlyOnce()
		{
			using var vector = new Vector<int> { 1, 2, 1 };
			vector.Remove(1);
			Assert.AreEqual(2, vector.Count);
			Assert.AreEqual(2, vector[0]);
			Assert.AreEqual(1, vector[1]);
		}

		[TestMethod]
		public void RemoveAt_RemovesItemFromIndex()
		{
			using var vector = new Vector<int> { 1, 2, 3 };
			vector.RemoveAt(1);
			Assert.AreEqual(2, vector.Count);
			Assert.AreEqual(1, vector[0]);
			Assert.AreEqual(3, vector[1]);
		}

		[TestMethod]
		public void RemoveBack_RemovesTheLastItem()
		{
			using var vector = new Vector<int> { 1, 2, 3 };
			vector.RemoveEnd();
			Assert.AreEqual(2, vector.Count);
			Assert.AreEqual(1, vector[0]);
			Assert.AreEqual(2, vector[1]);
		}

		[TestMethod]
		public void RemoveBack_RemovesLastItemFromVectorOfOne()
		{
			using var vector = new Vector<int> { 1 };
			vector.RemoveEnd();
			Assert.AreEqual(0, vector.Count);
		}

		[TestMethod]
		public void AddRange_AddsItemsFromIEnumerable()
		{
			using var vector = new Vector<int>();
			var collection = new List<int> { 10, 20, 30 };
			vector.AddEndRange(collection);
			Assert.AreEqual(3, vector.Count);
			Assert.AreEqual(10, vector[0]);
			Assert.AreEqual(20, vector[1]);
			Assert.AreEqual(30, vector[2]);
		}

		[TestMethod]
		public void AddRange_AddsItemsFromArray()
		{
			using var vector = new Vector<int>();
			var array = new int[] { 10, 20, 30 };
			vector.AddEndRange(array);
			Assert.AreEqual(3, vector.Count);
			Assert.AreEqual(10, vector[0]);
			Assert.AreEqual(20, vector[1]);
			Assert.AreEqual(30, vector[2]);
		}

		[TestMethod]
		public void AddRange_AddsItemsFromIList()
		{
			using var vector = new Vector<int>();
			var list = new List<int> { 10, 20, 30 };
			vector.AddEndRange(list);
			Assert.AreEqual(3, vector.Count);
			Assert.AreEqual(10, vector[0]);
			Assert.AreEqual(20, vector[1]);
			Assert.AreEqual(30, vector[2]);
		}

		[TestMethod]
		public void AddRange_TriggersResizeWhenCapacityIsFull()
		{
			using var vector = new Vector<int>();
			const int initialCount = 4;
			for (int i = 0; i < initialCount; i++) vector.Add(i);
			Assert.AreEqual(initialCount, vector.Count);

			var collection = new int[] { 10, 20, 30 };
			vector.AddEndRange(collection);

			Assert.AreEqual(initialCount + collection.Length, vector.Count);
			Assert.AreEqual(3, vector[3]);
			Assert.AreEqual(10, vector[4]);
			Assert.AreEqual(30, vector[6]);
		}

		[TestMethod]
		public void RemoveRange_RemovesItemsFromMiddle()
		{
			using var vector = new Vector<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
			vector.AddBackRange(3, 2); // Remove 4 and 5
			Assert.AreEqual(6, vector.Count);
			Assert.AreEqual(1, vector[0]);
			Assert.AreEqual(3, vector[2]);
			Assert.AreEqual(6, vector[3]);
			Assert.AreEqual(8, vector[5]);
		}

		[TestMethod]
		public void RemoveRange_RemovesItemsFromBeginning()
		{
			using var vector = new Vector<int> { 1, 2, 3, 4, 5 };
			vector.AddBackRange(0, 2); // Remove 1 and 2
			Assert.AreEqual(3, vector.Count);
			Assert.AreEqual(3, vector[0]);
			Assert.AreEqual(4, vector[1]);
			Assert.AreEqual(5, vector[2]);
		}

		[TestMethod]
		public void RemoveRange_RemovesItemsFromEnd()
		{
			using var vector = new Vector<int> { 1, 2, 3, 4, 5 };
			vector.AddBackRange(3, 2); // Remove 4 and 5
			Assert.AreEqual(3, vector.Count);
			Assert.AreEqual(3, vector[2]);
		}

		[TestMethod]
		public void RemoveRange_RemovesAllItems()
		{
			using var vector = new Vector<int> { 1, 2, 3, 4, 5 };
			vector.AddBackRange(0, 5);
			Assert.AreEqual(0, vector.Count);
			Assert.IsTrue(vector.IsEmpty);
		}

		// ====================================================================
		// Contains & IndexOf Tests
		// ====================================================================

		[TestMethod]
		public void Contains_ReturnsTrueForExistingItem()
		{
			using var vector = new Vector<MyStruct> { new MyStruct(1, true) };
			Assert.IsTrue(vector.Contains(new MyStruct(1, true)));
		}

		[TestMethod]
		public void Contains_ReturnsFalseForNonExistingItem()
		{
			using var vector = new Vector<MyStruct> { new MyStruct(1, true) };
			Assert.IsFalse(vector.Contains(new MyStruct(2, true)));
		}

		[TestMethod]
		public void Contains_HandlesDuplicateItems()
		{
			using var vector = new Vector<int> { 1, 2, 1 };
			Assert.IsTrue(vector.Contains(1));
		}

		[TestMethod]
		public void IndexOf_ReturnsCorrectIndex()
		{
			using var vector = new Vector<int> { 10, 20 };
			Assert.AreEqual(1, vector.IndexOf(20));
		}

		[TestMethod]
		public void IndexOf_ReturnsFirstOccurrence()
		{
			using var vector = new Vector<int> { 10, 20, 10 };
			Assert.AreEqual(0, vector.IndexOf(10));
		}

		[TestMethod]
		public void IndexOf_ReturnsNegativeOneIfNotFound()
		{
			using var vector = new Vector<int> { 10 };
			Assert.AreEqual(-1, vector.IndexOf(20));
		}

		// ====================================================================
		// Stress & Complex Operation Tests
		// ====================================================================

		[TestMethod]
		public void Add_StressTest_AddsManyItemsCorrectly()
		{
			using var vector = new Vector<int>();
			const int count = 10_000_000;
			for (int i = 0; i < count; i++)
			{
				vector.Add(i);
			}
			Assert.AreEqual(count, vector.Count);
			for (int i = 0; i < count; i++)
			{
				Assert.AreEqual(i, vector[i]);
			}
		}

		[TestMethod]
		public void AddRange_StressTest_AddsManyItemsCorrectly()
		{
			using var vector = new Vector<int>();
			const int count = 1_000_000;
			var collection = Enumerable.Range(0, count).ToList();
			vector.AddEndRange(collection);
			Assert.AreEqual(count, vector.Count);
			for (int i = 0; i < count; i++)
			{
				Assert.AreEqual(i, vector[i]);
			}
		}

		[TestMethod]
		public void Add_TriggersResizeWhenCapacityIsFull()
		{
			using var vector = new Vector<int> { 0, 1, 2, 3 };
			vector.Add(4); // Triggers resize from 4 to 8
			Assert.AreEqual(5, vector.Count);
			Assert.AreEqual(0, vector[0]);
			Assert.AreEqual(4, vector[4]);
		}

		[TestMethod]
		public void Insert_TriggersResizeWhenCapacityIsFull()
		{
			using var vector = new Vector<int> { 0, 1, 2, 3 };
			vector.Insert(0, 99); // Triggers resize from 4 to 8
			Assert.AreEqual(5, vector.Count);
			Assert.AreEqual(99, vector[0]);
			Assert.AreEqual(0, vector[1]);
		}

		[TestMethod]
		public void AddAndRemove_InterleavedOperations_MaintainsCorrectCountAndValues()
		{
			using var vector = new Vector<int>();
			vector.Add(10);
			vector.Add(20);
			vector.Remove(10);
			vector.Add(30);
			Assert.AreEqual(2, vector.Count);
			Assert.AreEqual(20, vector[0]);
			Assert.AreEqual(30, vector[1]);
		}

		[TestMethod]
		public void MixOfOperations_MaintainsCorrectState()
		{
			using var vector = new Vector<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			vector.Remove(4); // [0, 1, 2, 3, 5, 6, 7, 8, 9]
			Assert.AreEqual(9, vector.Count);
			vector.RemoveAt(0); // [1, 2, 3, 5, 6, 7, 8, 9]
			vector.RemoveEnd(); // [1, 2, 3, 5, 6, 7, 8]
			Assert.AreEqual(7, vector.Count);
			vector.Insert(2, 99); // [1, 2, 99, 3, 5, 6, 7, 8]
			Assert.AreEqual(8, vector.Count);

			int[] expected = { 1, 2, 99, 3, 5, 6, 7, 8 };
			for (int i = 0; i < vector.Count; i++)
			{
				Assert.AreEqual(expected[i], vector[i]);
			}
		}

		// ====================================================================
		// Enumerator & Interface Tests
		// ====================================================================

		[TestMethod]
		public void GetEnumerator_IteratesOverAllElements()
		{
			using var vector = new Vector<int> { 1, 2, 3 };
			var list = new List<int>();
			foreach (var item in vector)
			{
				list.Add(item);
			}
			Assert.AreEqual(3, list.Count);
			Assert.AreEqual(1, list[0]);
			Assert.AreEqual(2, list[1]);
			Assert.AreEqual(3, list[2]);
		}

		[TestMethod]
		public void GetEnumerator_IteratesOverEmptyVector()
		{
			using var vector = new Vector<int>();
			Assert.AreEqual(0, vector.Count());
		}

		[TestMethod]
		public void CopyTo_CopiesElementsToArray()
		{
			using var vector = new Vector<int> { 1, 2 };
			var array = new int[5];
			vector.CopyTo(array, 1);
			Assert.AreEqual(0, array[0]);
			Assert.AreEqual(1, array[1]);
			Assert.AreEqual(2, array[2]);
			Assert.AreEqual(0, array[3]);
		}

		[TestMethod]
		public void Clone_CreatesIndependentCopy()
		{
			using var originalVector = new Vector<int> { 1, 2 };
			using var clonedVector = (Vector<int>)originalVector.Clone();
			originalVector.RemoveAt(0); // Modify the original

			Assert.AreEqual(1, originalVector.Count);
			Assert.AreEqual(2, originalVector[0]);
			Assert.AreEqual(2, clonedVector.Count);
			Assert.AreEqual(1, clonedVector[0]);
		}

		[TestMethod]
		public void Equals_ReturnsTrueForEqualVectors()
		{
			using var vector1 = new Vector<MyStruct> { new MyStruct(1, true), new MyStruct(2, false) };
			using var vector2 = new Vector<MyStruct> { new MyStruct(1, true), new MyStruct(2, false) };
			Assert.IsTrue(vector1.Equals(vector2));
		}

		[TestMethod]
		public void Equals_ReturnsFalseForUnequalVectors_DifferentValues()
		{
			using var vector1 = new Vector<MyStruct> { new MyStruct(1, true) };
			using var vector2 = new Vector<MyStruct> { new MyStruct(2, true) };
			Assert.IsFalse(vector1.Equals(vector2));
		}

		[TestMethod]
		public void Equals_ReturnsFalseForUnequalVectors_DifferentCounts()
		{
			using var vector1 = new Vector<MyStruct> { new MyStruct(1, true), new MyStruct(2, false) };
			using var vector2 = new Vector<MyStruct> { new MyStruct(1, true) };
			Assert.IsFalse(vector1.Equals(vector2));
		}

		// ====================================================================
		// Capacity & Allocator Tests
		// ====================================================================

		[TestMethod]
		public void ReserveCapacity_IncreasesCapacityWhenNeeded()
		{
			using var vector = new Vector<int> { 0, 1, 2, 3 };
			vector.AddReserve(10);
			Assert.AreEqual(4, vector.Count);
			Assert.AreEqual(10, vector.Reserve);
		}

		[TestMethod]
		public void ReserveCapacity_DoesNothingWhenReserveIsSmaller()
		{
			using var vector = new Vector<int> { 0, 1, 2, 3, 4 };
			int initialReserve = vector.Reserve;
			vector.AddReserve(3);
			Assert.AreEqual(initialReserve, vector.Reserve);
		}

		[TestMethod]
		public void RemoveBackThenAdd_StressTest_HandlesResizingDownAndUpCorrectly()
		{
			using var vector = new Vector<int>();
			const int initialCount = 1_000_000;
			const int removeCount = 999_990;

			for (int i = 0; i < initialCount; i++)
			{
				vector.Add(i);
			}
			Assert.AreEqual(initialCount, vector.Count);

			for (int i = 0; i < removeCount; i++)
			{
				vector.RemoveEnd();
			}
			Assert.AreEqual(initialCount - removeCount, vector.Count);
			Assert.AreEqual(9, vector[9]);

			for (int i = 0; i < 500; i++)
			{
				vector.Add(i);
			}
			Assert.AreEqual(initialCount - removeCount + 500, vector.Count);
			Assert.AreEqual(499, vector[vector.Count - 1]);
		}


		// ====================================================================
		// Algorithm Integration Tests
		// ====================================================================

		// Helper methods for algorithms are kept here for context.

		private void Merge(Vector<int> vector, int left, int mid, int right)
		{
			int n1 = mid - left + 1;
			int n2 = right - mid;
			using var leftVector = new Vector<int>();
			using var rightVector = new Vector<int>();
			for (int i = 0; i < n1; i++) { leftVector.Add(vector[left + i]); }
			for (int j = 0; j < n2; j++) { rightVector.Add(vector[mid + 1 + j]); }
			int i_ = 0, j_ = 0, k = left;
			while (i_ < n1 && j_ < n2)
			{
				if (leftVector[i_] <= rightVector[j_]) vector[k] = leftVector[i_++];
				else vector[k] = rightVector[j_++];
				k++;
			}
			while (i_ < n1) vector[k++] = leftVector[i_++];
			while (j_ < n2) vector[k++] = rightVector[j_++];
		}

		private void MergeSort(Vector<int> vector, int left, int right)
		{
			if (left < right)
			{
				int mid = left + (right - left) / 2;
				MergeSort(vector, left, mid);
				MergeSort(vector, mid + 1, right);
				Merge(vector, left, mid, right);
			}
		}

		private void Swap(Vector<int> vector, int i, int j)
		{
			int temp = vector[i];
			vector[i] = vector[j];
			vector[j] = temp;
		}

		private void Heapify(Vector<int> vector, int n, int i)
		{
			int largest = i;
			int left = 2 * i + 1;
			int right = 2 * i + 2;
			if (left < n && vector[left] > vector[largest]) largest = left;
			if (right < n && vector[right] > vector[largest]) largest = right;
			if (largest != i)
			{
				Swap(vector, i, largest);
				Heapify(vector, n, largest);
			}
		}

		private void BuildMaxHeap(Vector<int> vector)
		{
			int n = vector.Count;
			for (int i = n / 2 - 1; i >= 0; i--) Heapify(vector, n, i);
		}

		private int Partition(Vector<int> vector, int low, int high)
		{
			int pivot = vector[high];
			int i = low - 1;
			for (int j = low; j < high; j++)
			{
				if (vector[j] <= pivot)
				{
					i++;
					Swap(vector, i, j);
				}
			}
			Swap(vector, i + 1, high);
			return i + 1;
		}

		private void QuickSort(Vector<int> vector, int low, int high)
		{
			if (low < high)
			{
				int pivotIndex = Partition(vector, low, high);
				QuickSort(vector, low, pivotIndex - 1);
				QuickSort(vector, pivotIndex + 1, high);
			}
		}

		private bool BinarySearch(Vector<int> vector, int item)
		{
			int low = 0;
			int high = vector.Count - 1;
			while (low <= high)
			{
				int mid = low + (high - low) / 2;
				if (vector[mid] == item) return true;
				if (vector[mid] < item) low = mid + 1;
				else high = mid - 1;
			}
			return false;
		}

		[TestMethod]
		public void MergeSort_SortsRandomDataCorrectly()
		{
			using var vector = new Vector<int>();
			var random = new Random();
			const int count = 1_000_000;
			for (int i = 0; i < count; i++)
			{
				vector.Add(random.Next(1, count * 2));
			}
			MergeSort(vector, 0, vector.Count - 1);
			for (int i = 0; i < vector.Count - 1; i++)
			{
				Assert.IsTrue(vector[i] <= vector[i + 1], "Vector was not sorted correctly by MergeSort.");
			}
		}

		[TestMethod]
		public void HeapSort_SortsRandomDataCorrectly()
		{
			using var vector = new Vector<int>();
			var random = new Random();
			const int count = 1_000_000;
			for (int i = 0; i < count; i++)
			{
				vector.Add(random.Next(1, count * 2));
			}
			BuildMaxHeap(vector);
			for (int i = vector.Count - 1; i > 0; i--)
			{
				Swap(vector, 0, i);
				Heapify(vector, i, 0);
			}
			for (int i = 0; i < vector.Count - 1; i++)
			{
				Assert.IsTrue(vector[i] <= vector[i + 1], "Vector was not sorted correctly by HeapSort.");
			}
		}

		[TestMethod]
		public void QuickSort_SortsRandomDataCorrectly()
		{
			using var vector = new Vector<int>();
			var random = new Random();
			const int count = 1_000_000;
			for (int i = 0; i < count; i++)
			{
				vector.Add(random.Next(1, count * 2));
			}
			QuickSort(vector, 0, vector.Count - 1);
			for (int i = 0; i < vector.Count - 1; i++)
			{
				Assert.IsTrue(vector[i] <= vector[i + 1], "Vector was not sorted correctly by QuickSort.");
			}
		}

		[TestMethod]
		public void BinarySearch_FindsExistingItem()
		{
			using var vector = new Vector<int> { 1, 5, 10, 15, 20, 25, 30 };
			Assert.IsTrue(BinarySearch(vector, 20));
		}

		[TestMethod]
		public void BinarySearch_DoesNotFindNonExistingItem()
		{
			using var vector = new Vector<int> { 1, 5, 10, 15, 20, 25, 30 };
			Assert.IsFalse(BinarySearch(vector, 12));
		}
	}
}
using InfinityOfficialNetwork.StandardLibrary.DataStructures.Iterators;
using InfinityOfficialNetwork.StandardLibrary.MemoryManagement.Memory;

namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Containers.Primary;

#pragma warning disable CA2014, CS8605

public interface IVector<TArg> : IContainer<TArg>, IContiguousIteratable<TArg>, IContainerAssign<TArg>, IContainerElementBackAccess<TArg>, IContainerElementFrontAccess<TArg>, IContainerElementRandomAccess<TArg>, IContainerElementNativeArrayAccess<TArg>, IContainerCapacityIsEmpty<TArg>, IContainerCapacitySize<TArg>, IContainerCapacityResize<TArg>, IContainerCapacityReserve<TArg>, IContainerAddBack<TArg>, IContainerRemoveBack<TArg>, IContainerClear<TArg>
{ }

public interface IVectorParameters
{
	public static abstract int InitialReserve { get; }
	public static abstract int ReserveMultiplierNumerator { get; }
	public static abstract int ReserveMultiplierDenominator { get; }
}


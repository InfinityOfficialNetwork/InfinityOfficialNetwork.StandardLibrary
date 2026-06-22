namespace InfinityOfficialNetwork.StandardLibrary.Common;

/// <summary>
/// Represents a deep copy of the object graph and all associated resources.
/// </summary>
/// <typeparam name="TSelf"></typeparam>
public interface ICloneable<out TSelf> : ICloneable
	where TSelf : ICloneable<TSelf>
{
	new TSelf Clone();

	object ICloneable.Clone() => Clone();
}

/// <summary>
/// Represents a shallow copy of the object / resource.
/// </summary>
/// <typeparam name="TSelf"></typeparam>
public interface ICopyable<out TSelf>
{
	TSelf Copy();
}
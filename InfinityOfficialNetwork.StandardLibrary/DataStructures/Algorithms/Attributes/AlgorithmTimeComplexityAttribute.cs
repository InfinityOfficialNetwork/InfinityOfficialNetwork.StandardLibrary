namespace InfinityOfficialNetwork.StandardLibrary.DataStructures.Algorithms.Attributes
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class AlgorithmTimeComplexityAttribute : Attribute
	{
		public required Complexity Complexity { get; init; }
	}

	public enum Complexity
	{
		Constant,
		Logarithmic,
		Linear,
		LinearLogarithmic,
		Quadratic,
		Cubic,
		Exponential,
		Factorial
	}
}

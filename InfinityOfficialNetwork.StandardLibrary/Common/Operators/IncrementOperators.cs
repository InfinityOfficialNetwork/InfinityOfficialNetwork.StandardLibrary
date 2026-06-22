namespace InfinityOfficialNetwork.StandardLibrary.Common.Operators;

public interface IIncrementOperators<TSelf>
	where TSelf : IIncrementOperators<TSelf>
{
	public TSelf Increment();
}

[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "<Pending>")]
public static class IncrementOperatorsExtensions
{
	extension<TArg>(TArg)
		where TArg : IIncrementOperators<TArg>
	{
		public static TArg operator ++(TArg arg)
		{
			return arg.Increment();
		}
	}
}
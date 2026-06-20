using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "<Pending>")]

namespace InfinityOfficialNetwork.StandardLibrary.Attributes;



public interface ICondition<TParam>
{
	public abstract static bool Validate(TParam? param);
}

public interface INotNullCondition<TParam> : ICondition<TParam>
{
	public static bool ValidateNotNullCondition(TParam? param) => param != null;

	static bool ICondition<TParam>.Validate(TParam? param) => ValidateNotNullCondition(param);
}

public struct NotNullCondition<TParam> : INotNullCondition<TParam>
{ }

public interface INotDefaultCondition<TParam> : ICondition<TParam>
{
	public static bool ValidateNotDefaultCondition(TParam? param) => Equals(param, default(TParam));

	static bool ICondition<TParam>.Validate(TParam? param) => ValidateNotDefaultCondition(param);
}

public struct NotDefaultCondition<TParam> : INotDefaultCondition<TParam>
{ }

public interface IMinMaxCondition<TParam> : ICondition<TParam>, INotNullCondition<TParam>
	where TParam : IComparable<TParam>
{
	public static bool ValidateMinMaxCondition(TParam param) => param.CompareTo(Min) >= 0 && param.CompareTo(Max) < 0;
	public static TParam Min { get; }
	public static TParam Max { get; }

	static bool ICondition<TParam>.Validate(TParam? param) => ValidateNotNullCondition(param) && ValidateMinMaxCondition(param!);
}

//[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "<Pending>")]
//public struct MinMaxCondition<TParam> : IMinMaxCondition<TParam>
//	where TParam : IComparable<TParam>
//{
//	public static TParam Min { get; }
//	public static TParam Max { get; init; }
//}

public interface IAndCondition<TParam, TCondition1, TCondition2> : ICondition<TParam>
	where TCondition1 : ICondition<TParam>
	where TCondition2 : ICondition<TParam>
{
	public static bool ValidateAndCondition(TParam? param) => TCondition1.Validate(param) && TCondition2.Validate(param);

	static bool ICondition<TParam>.Validate(TParam? param) => ValidateAndCondition(param);
}

[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "<Pending>")]
public struct AndCondition<TParam, TCondition1, TCondition2> : IAndCondition<TParam, TCondition1, TCondition2>
	where TCondition1 : ICondition<TParam>
	where TCondition2 : ICondition<TParam>
{
}

public interface IOrCondition<TParam, TCondition1, TCondition2> : ICondition<TParam>
	where TCondition1 : ICondition<TParam>
	where TCondition2 : ICondition<TParam>
{
	public static bool ValidateOrCondition(TParam? param) => TCondition1.Validate(param) || TCondition2.Validate(param);

	static bool ICondition<TParam>.Validate(TParam? param) => ValidateOrCondition(param);
}

[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "<Pending>")]
public struct OrCondition<TParam, TCondition1, TCondition2> : IOrCondition<TParam, TCondition1, TCondition2>
	where TCondition1 : ICondition<TParam>
	where TCondition2 : ICondition<TParam>
{
}

public interface INotCondition<TParam, TCondition> : ICondition<TParam>
	where TCondition : ICondition<TParam>
{
	public static bool ValidateOrCondition(TParam? param) => !TCondition.Validate(param);

	static bool ICondition<TParam>.Validate(TParam? param) => ValidateOrCondition(param);
}

[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "<Pending>")]
public struct NotCondition<TParam, TCondition> : INotCondition<TParam, TCondition>
	where TCondition : ICondition<TParam>
{
}



[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = true)]
public sealed class ParameterPreconditionAttribute<TParam, TCondition> : Attribute
	where TCondition : ICondition<TParam>, new()
{

	[Conditional("DEBUG")]
	public void Validate(TParam? param) => TCondition.Validate(param);
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class InstancePreconditionAttribute<TParam, TCondition> : Attribute
	where TCondition : ICondition<TParam>, new()
{

	[Conditional("DEBUG")]
	public void Validate(TParam? instance) => TCondition.Validate(instance);
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class InstancePostconditionAttribute<TParam, TCondition> : Attribute
	where TCondition : ICondition<TParam>, new()
{

	[Conditional("DEBUG")]
	public void Validate(TParam? instance) => TCondition.Validate(instance);
}

[AttributeUsage(AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = true)]
public sealed class ReturnPostconditionAttribute<TParam, TCondition> : Attribute
	where TCondition : ICondition<TParam>, new()
{

	[Conditional("DEBUG")]
	public void Validate(TParam? instance) => TCondition.Validate(instance);
}

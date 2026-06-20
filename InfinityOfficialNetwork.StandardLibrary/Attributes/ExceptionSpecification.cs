using System;
using System.Collections.Generic;
using System.Text;

namespace InfinityOfficialNetwork.StandardLibrary.Attributes;

#pragma warning disable CA1813
#pragma warning disable CA1018

public enum ExceptionGuaranteeType
{
	None,
	Basic,
	Strong,
	Nothrow
}


[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class ExceptionSpecificationAttribute : Attribute
{
	public ExceptionGuaranteeType GuaranteeType { get; }
	public Type[] ExceptionTypes { get; }

	public Type[] MinimalCoveringExceptionTypes { get; }

	public ExceptionSpecificationAttribute(ExceptionGuaranteeType guaranteeType, Type[] exceptionTypes)
	{
		GuaranteeType = guaranteeType;
		ExceptionTypes = exceptionTypes;

		var coveringSet = new List<Type>(ExceptionTypes);
		var typesToRemove = new HashSet<Type>();

		foreach (Type tA in ExceptionTypes)
			if (typesToRemove.Contains(tA))
				continue;

			else foreach (Type tB in ExceptionTypes)
					if (tA == tB)
						continue;

					else if (tB.IsAssignableFrom(tA))
					{
						typesToRemove.Add(tA);

						break;
					}

		MinimalCoveringExceptionTypes = coveringSet.Where(t => !typesToRemove.Contains(t)).ToArray();
	}
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class NoGuaranteeExceptionSpecificationAttribute(Type[] exceptionTypes) : ExceptionSpecificationAttribute(ExceptionGuaranteeType.None, exceptionTypes)
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class BasicGuaranteeExceptionSpecificationAttribute(Type[] exceptionTypes) : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Basic, exceptionTypes)
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class StrongGuaranteeExceptionSpecificationAttribute(Type[] exceptionTypes) : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Strong, exceptionTypes)
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class NothrowGuaranteeExceptionSpecificationAttribute() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Nothrow, [])
{ }


[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class ExceptionSpecificationAttribute<T1>(ExceptionGuaranteeType guaranteeType) : ExceptionSpecificationAttribute(guaranteeType, [typeof(T1)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class ExceptionSpecificationAttribute<T1, T2>(ExceptionGuaranteeType guaranteeType) : ExceptionSpecificationAttribute(guaranteeType, [typeof(T1), typeof(T2)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class ExceptionSpecificationAttribute<T1, T2, T3>(ExceptionGuaranteeType guaranteeType) : ExceptionSpecificationAttribute(guaranteeType, [typeof(T1), typeof(T2), typeof(T3)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class ExceptionSpecificationAttribute<T1, T2, T3, T4>(ExceptionGuaranteeType guaranteeType) : ExceptionSpecificationAttribute(guaranteeType, [typeof(T1), typeof(T2), typeof(T3), typeof(T4)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class ExceptionSpecificationAttribute<T1, T2, T3, T4, T5>(ExceptionGuaranteeType guaranteeType) : ExceptionSpecificationAttribute(guaranteeType, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class ExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6>(ExceptionGuaranteeType guaranteeType) : ExceptionSpecificationAttribute(guaranteeType, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class ExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7>(ExceptionGuaranteeType guaranteeType) : ExceptionSpecificationAttribute(guaranteeType, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class ExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8>(ExceptionGuaranteeType guaranteeType) : ExceptionSpecificationAttribute(guaranteeType, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class ExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9>(ExceptionGuaranteeType guaranteeType) : ExceptionSpecificationAttribute(guaranteeType, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class ExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(ExceptionGuaranteeType guaranteeType) : ExceptionSpecificationAttribute(guaranteeType, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class ExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(ExceptionGuaranteeType guaranteeType) : ExceptionSpecificationAttribute(guaranteeType, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class ExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(ExceptionGuaranteeType guaranteeType) : ExceptionSpecificationAttribute(guaranteeType, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class ExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(ExceptionGuaranteeType guaranteeType) : ExceptionSpecificationAttribute(guaranteeType, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class ExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(ExceptionGuaranteeType guaranteeType) : ExceptionSpecificationAttribute(guaranteeType, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class ExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(ExceptionGuaranteeType guaranteeType) : ExceptionSpecificationAttribute(guaranteeType, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class ExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(ExceptionGuaranteeType guaranteeType) : ExceptionSpecificationAttribute(guaranteeType, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15), typeof(T16)])
{ }


[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class NoGuaranteeExceptionSpecificationAttribute<T1>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.None, [typeof(T1)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class NoGuaranteeExceptionSpecificationAttribute<T1, T2>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.None, [typeof(T1), typeof(T2)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class NoGuaranteeExceptionSpecificationAttribute<T1, T2, T3>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.None, [typeof(T1), typeof(T2), typeof(T3)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class NoGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.None, [typeof(T1), typeof(T2), typeof(T3), typeof(T4)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class NoGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.None, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class NoGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.None, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class NoGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.None, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class NoGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.None, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class NoGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.None, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class NoGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.None, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class NoGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.None, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class NoGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.None, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class NoGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.None, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class NoGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.None, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class NoGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.None, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class NoGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.None, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15), typeof(T16)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class BasicGuaranteeExceptionSpecificationAttribute<T1>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Basic, [typeof(T1)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class BasicGuaranteeExceptionSpecificationAttribute<T1, T2>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Basic, [typeof(T1), typeof(T2)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class BasicGuaranteeExceptionSpecificationAttribute<T1, T2, T3>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Basic, [typeof(T1), typeof(T2), typeof(T3)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class BasicGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Basic, [typeof(T1), typeof(T2), typeof(T3), typeof(T4)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class BasicGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Basic, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class BasicGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Basic, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class BasicGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Basic, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class BasicGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Basic, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class BasicGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Basic, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class BasicGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Basic, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class BasicGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Basic, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class BasicGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Basic, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class BasicGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Basic, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class BasicGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Basic, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class BasicGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Basic, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class BasicGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Basic, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15), typeof(T16)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class StrongGuaranteeExceptionSpecificationAttribute<T1>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Strong, [typeof(T1)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class StrongGuaranteeExceptionSpecificationAttribute<T1, T2>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Strong, [typeof(T1), typeof(T2)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class StrongGuaranteeExceptionSpecificationAttribute<T1, T2, T3>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Strong, [typeof(T1), typeof(T2), typeof(T3)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class StrongGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Strong, [typeof(T1), typeof(T2), typeof(T3), typeof(T4)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class StrongGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Strong, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class StrongGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Strong, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class StrongGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Strong, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class StrongGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Strong, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class StrongGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Strong, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class StrongGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Strong, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class StrongGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Strong, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class StrongGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Strong, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class StrongGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Strong, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class StrongGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Strong, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class StrongGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Strong, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15)])
{ }

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
public class StrongGuaranteeExceptionSpecificationAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>() : ExceptionSpecificationAttribute(ExceptionGuaranteeType.Strong, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15), typeof(T16)])
{ }
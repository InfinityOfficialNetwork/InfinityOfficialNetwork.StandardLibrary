
namespace InfinityOfficialNetwork.StandardLibrary.Attributes;

/// <summary>
/// Indicates interface is not meant to be implemented by user-code
/// </summary>
[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
public sealed class InternalImplementersOnlyAttribute : Attribute
{}
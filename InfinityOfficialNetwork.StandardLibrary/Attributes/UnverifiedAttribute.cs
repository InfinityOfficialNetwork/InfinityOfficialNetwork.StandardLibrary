using System;
using System.Collections.Generic;
using System.Text;

namespace InfinityOfficialNetwork.StandardLibrary.Attributes;

/// <summary>
/// Indicates that incorrect usage of the code can corrupt the runtime.
/// </summary>
[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
public sealed class UnverifiedAttribute : Attribute
{
}

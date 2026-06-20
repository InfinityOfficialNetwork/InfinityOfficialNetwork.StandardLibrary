// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "Stupid rule.")]
[assembly: SuppressMessage("Design", "CA1028:Enum Storage should be Int32", Justification = "Just dumb.")]
[assembly: SuppressMessage("Performance", "CA1849:Call async methods when in an async method", Justification = "call nonblocking sync in async")]
[assembly: SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "I hate them damn globalists")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
[assembly: SuppressMessage("Design", "CA1019:Define accessors for attribute arguments", Justification = "<Pending>")]
[assembly: SuppressMessage("Style", "IDE0011:Add braces", Justification = "braces look stupid")]
[assembly: SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Justification = "<Pending>")]
[assembly: SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "<Pending>")]
[assembly: SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "<Pending>")]

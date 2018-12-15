// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Potential Code Quality Issues", "RECS0017:Possible compare of value type with 'null'", Justification = "We really only want to doublecheck that it is not null.  We don't care if it is the default value of a value type.", Scope = "member", Target = "~M:ExtendableEnums.ExtendableEnumBase`2.#ctor(`1,System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Potential Code Quality Issues", "RECS0108:Warns about static fields in generic types", Justification = "We intentionally want these static field to be different per generic type.", Scope = "type", Target = "~T:ExtendableEnums.ExtendableEnumBase`2")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Static methods on this generic provide the core functionality.", Scope = "type", Target = "ExtendableEnums.ExtendableEnumBase`2")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S4035:Classes implementing \"IEquatable<T>\" should be sealed", Justification = "We should only compare on Value property no matter what derives from this class.", Scope = "type", Target = "~T:ExtendableEnums.ExtendableEnumBase`2")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1036:Overload operator Equals and comparison operators when implementing System.IComparable", Justification = "The value comparison is still valid at this level of inheritance.", Scope = "type", Target = "~T:ExtendableEnums.ExtendableEnum`1")]

// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1036:Overload operator Equals and comparison operators when implementing System.IComparable", Justification = "We only compare equality based on base class Value property.", Scope = "type", Target = "~T:ExtendableEnums.UnitTests.Models.SampleStatus")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1067:Override Object.Equals(object) when implementing IEquatable<T>", Justification = "We only compare equality based on base class Value property", Scope = "type", Target = "~T:ExtendableEnums.UnitTests.ExpandableEnumerationTests.ConstructorShould.NullValueEnum")]
﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace ExtendableEnums
{
    [JsonConverter(typeof(ExtendableEnumJsonConverter))]
    public abstract class ExtendableEnumBase<TEnumeration, TValue> : IExtendableEnum<TValue>, IComparable<TEnumeration>, IEquatable<TEnumeration>
            where TEnumeration : ExtendableEnumBase<TEnumeration, TValue>
            where TValue : IComparable
    {
        private static readonly Lazy<TEnumeration[]> enumerations = new Lazy<TEnumeration[]>(GetEnumerations);
        private static readonly Lazy<TEnumeration> maximum = new Lazy<TEnumeration>(() => ParseValue(GetAll().Max(x => x.Value)));
        private static readonly Lazy<TEnumeration> minimum = new Lazy<TEnumeration>(() => ParseValue(GetAll().Min(x => x.Value)));

        protected ExtendableEnumBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendableEnumBase{TEnumeration, TValue}"/> class.
        /// </summary>
        /// <param name="value">The core value with which to create this instance.</param>
        /// <param name="displayName">The core name with which to identify this instance.</param>
        protected ExtendableEnumBase(TValue value, string displayName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
            DisplayName = displayName;
        }

        /// <summary>
        /// Gets the enumeration object with the highest Value property.
        /// </summary>
        public static TEnumeration Max => maximum.Value;

        /// <summary>
        /// Gets the enumeration object with the lowest Value property.
        /// </summary>
        public static TEnumeration Min => minimum.Value;

        /// <summary>
        /// Gets the core name to be used for display purposes and for identifying this enumeration object.
        /// </summary>
        [NotMapped]
        public string DisplayName { get; }

        /// <summary>
        /// Gets the core value that represents this enumeration object.
        /// </summary>
        public TValue Value { get; set; }

        public static implicit operator ExtendableEnumBase<TEnumeration, TValue>(TValue value)
        {
            return ParseValue(value);
        }

        public static bool operator !=(ExtendableEnumBase<TEnumeration, TValue> left, ExtendableEnumBase<TEnumeration, TValue> right)
        {
            return !Equals(left, right);
        }

        public static bool operator <(ExtendableEnumBase<TEnumeration, TValue> left, ExtendableEnumBase<TEnumeration, TValue> right)
        {
            return left != null && left.CompareTo((TEnumeration)right) < 0;
        }

        public static bool operator <=(ExtendableEnumBase<TEnumeration, TValue> left, ExtendableEnumBase<TEnumeration, TValue> right)
        {
            return left != null && left.CompareTo((TEnumeration)right) <= 0;
        }

        public static bool operator ==(ExtendableEnumBase<TEnumeration, TValue> left, ExtendableEnumBase<TEnumeration, TValue> right)
        {
            return Equals(left, right);
        }

        public static bool operator >(ExtendableEnumBase<TEnumeration, TValue> left, ExtendableEnumBase<TEnumeration, TValue> right)
        {
            return left != null && left.CompareTo((TEnumeration)right) > 0;
        }

        public static bool operator >=(ExtendableEnumBase<TEnumeration, TValue> left, ExtendableEnumBase<TEnumeration, TValue> right)
        {
            return left != null && left.CompareTo((TEnumeration)right) >= 0;
        }

        /// <summary>
        /// Get all the enumeration objects that have been defined for this type of enumeration.
        /// </summary>
        /// <returns>An array of all enumeration objects defined for this type of enumeration.</returns>
        public static TEnumeration[] GetAll()
        {
            return enumerations.Value;
        }

        /// <summary>
        /// Finds the enumeration object with the matching DisplayName property.
        /// </summary>
        /// <param name="displayName">The display name property for which to find a matching enumeration object.</param>
        /// <returns>The enumeration object with a matching display name.  Throws an ArgumentException if no match exists.</returns>
        public static TEnumeration Parse(string displayName)
        {
            if (!TryFind(item => item.DisplayName == displayName, out TEnumeration result))
            {
                var message = $"'{displayName}' is not a valid display name in {typeof(TEnumeration)}";
                throw new ArgumentException(message, nameof(displayName));
            }

            return result;
        }

        /// <summary>
        /// Finds the enumeration object with the matching value.
        /// </summary>
        /// <param name="value">The value which should be searched for.</param>
        /// <returns>The enumeration object with a matching value.  Throws an ArgumentException if no match exists.</returns>
        public static TEnumeration ParseValue(TValue value)
        {
            if (!TryFind(item => item.Value.Equals(value), out TEnumeration result))
            {
                var message = $"'{value}' is not a valid value in {typeof(TEnumeration)}";
                throw new ArgumentException(message, nameof(value));
            }

            return result;
        }

        /// <summary>
        /// Finds the enumeration object with the matching value.
        /// </summary>
        /// <param name="value">The value which should be seached for.</param>
        /// <returns>The enumeration object with a matching value.  Throws an ArgumentException if no match exists.</returns>
        public static ExtendableEnumBase<TEnumeration, TValue> FromTValue(TValue value)
        {
            return ParseValue(value);
        }

        /// <summary>
        /// Tries to find an enumeration object with the specified display name.
        /// </summary>
        /// <param name="displayName">The display name which should be searched for.</param>
        /// <param name="result">The output variable to populate with the matching enumeration object if a match is found.</param>
        /// <returns>True if an enumeration object with a matching display name is found, otherwise false.</returns>
        public static bool TryParse(string displayName, out TEnumeration result)
        {
            return TryFind(e => e.DisplayName == displayName, out result);
        }

        /// <summary>
        /// Tries to find an enumeration object with the specified value.
        /// </summary>
        /// <param name="value">The value which should be searched for.</param>
        /// <param name="result">An ouput variable that will be populated with an enumeration object if a match is found.</param>
        /// <returns>True if an enumeration object with a matching value is found, otherwise false.</returns>
        public static bool TryParseValue(TValue value, out TEnumeration result)
        {
            return TryFind(e => e.ValueEquals(value), out result);
        }

        /// <summary>
        /// Compares the value of this object with another enumeration object value to check for equality.
        /// </summary>
        /// <param name="other">The other enumeration object with which to compare.</param>
        /// <returns>Zero if the enumeration objects have matching values. Less than zero if this instance preceeds the other. More than zero if the other value preceeds this instance.</returns>
        public int CompareTo(TEnumeration other)
        {
            return Value.CompareTo(other == default(TEnumeration) ? default(TValue) : other.Value);
        }

        /// <summary>
        /// Compares this instance with another to see if they have the same value.
        /// </summary>
        /// <param name="obj">The other object with which to compare.</param>
        /// <returns>True if this instance and the the other have the same value, otherwise false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as TEnumeration);
        }

        /// <summary>
        /// Compares this instance with another to see if they have the same value.
        /// </summary>
        /// <param name="other">The other enumeration object with which to compare.</param>
        /// <returns>True if this instance and the the other have the same value, otherwise false.</returns>
        public bool Equals(TEnumeration other)
        {
            return other != null && ValueEquals(other.Value);
        }

        /// <summary>
        /// Gets a hash code for this particular instance.
        /// </summary>
        /// <returns>The hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Gets the string representation of this instance, which is its DisplayName property.
        /// </summary>
        /// <returns>The string representation of this instance, which is its DisplayName property.</returns>
        public override sealed string ToString()
        {
            return DisplayName;
        }

        /// <summary>
        /// Checks to see if the value of this instance equals the specified value.
        /// </summary>
        /// <param name="value">The value with which to compare.</param>
        /// <returns>True if this values match, otherwise false.</returns>
        protected virtual bool ValueEquals(TValue value)
        {
            return Value.Equals(value);
        }

        private static TEnumeration[] GetEnumerations()
        {
            var enumerationType = typeof(TEnumeration);
            return enumerationType
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(info => enumerationType.IsAssignableFrom(info.FieldType))
                .Select(info => info.GetValue(null))
                .Cast<TEnumeration>()
                .ToArray();
        }

        private static bool TryFind(Func<TEnumeration, bool> predicate, out TEnumeration result)
        {
            var allItems = GetAll();
            result = allItems.FirstOrDefault(predicate);
            return result != null;
        }
    }
}

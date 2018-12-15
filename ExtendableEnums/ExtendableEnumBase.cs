using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace ExtendableEnums
{
    [JsonConverter(typeof(ExtendableEnumJsonConverter))]
    public abstract class ExtendableEnumBase<TEnumeration, TValue> : IComparable<TEnumeration>, IEquatable<TEnumeration>
            where TEnumeration : ExtendableEnumBase<TEnumeration, TValue>
            where TValue : IComparable
    {
        private static readonly Lazy<TEnumeration[]> enumerations = new Lazy<TEnumeration[]>(GetEnumerations);
        private static readonly Lazy<TEnumeration> maximum = new Lazy<TEnumeration>(() => ParseValue(GetAll().Max(x => x.Value)));
        private static readonly Lazy<TEnumeration> minimum = new Lazy<TEnumeration>(() => ParseValue(GetAll().Min(x => x.Value)));

        protected ExtendableEnumBase(TValue value, string displayName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
            DisplayName = displayName;
        }

        public static TEnumeration Max => maximum.Value;

        public static TEnumeration Min => minimum.Value;

        public string DisplayName { get; protected set; }

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

        public static TEnumeration[] GetAll()
        {
            return enumerations.Value;
        }

        public static TEnumeration Parse(string displayName)
        {
            if (!TryFind(item => item.DisplayName == displayName, out TEnumeration result))
            {
                var message = $"'{displayName}' is not a valid display name in {typeof(TEnumeration)}";
                throw new ArgumentException(message, nameof(displayName));
            }

            return result;
        }

        public static TEnumeration ParseValue(TValue value)
        {
            if (!TryFind(item => item.Value.Equals(value), out TEnumeration result))
            {
                var message = $"'{value}' is not a valid value in {typeof(TEnumeration)}";
                throw new ArgumentException(message, nameof(value));
            }

            return result;
        }

        public static ExtendableEnumBase<TEnumeration, TValue> FromTValue(TValue value)
        {
            return ParseValue(value);
        }

        public static bool TryParse(string displayName, out TEnumeration result)
        {
            return TryFind(e => e.DisplayName == displayName, out result);
        }

        public static bool TryParseValue(TValue value, out TEnumeration result)
        {
            return TryFind(e => e.ValueEquals(value), out result);
        }

        public int CompareTo(TEnumeration other)
        {
            return Value.CompareTo(other == default(TEnumeration) ? default(TValue) : other.Value);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TEnumeration);
        }

        public bool Equals(TEnumeration other)
        {
            return other != null && ValueEquals(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override sealed string ToString()
        {
            return DisplayName;
        }

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

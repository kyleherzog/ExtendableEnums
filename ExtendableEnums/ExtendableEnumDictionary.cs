using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using ExtendableEnums.Serialization.Newtonsoft;
using Newtonsoft.Json;

namespace ExtendableEnums;

/// <summary>
/// A specialized dictionary to ensure proper serialization when an <see cref="ExtendableEnumBase{TEnumeration, TValue}" />
/// type object is the key type.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
[System.Text.Json.Serialization.JsonConverter(typeof(ExtendableEnums.Serialization.SystemText.ExtendableEnumDictionaryJsonConverter))]
[JsonConverter(typeof(ExtendableEnumDictionaryJsonConverter))]
[Serializable]
public class ExtendableEnumDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    where TKey : IExtendableEnum
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExtendableEnumDictionary{TKey, TValue}"/> class.
    /// </summary>
    public ExtendableEnumDictionary()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExtendableEnumDictionary{TKey, TValue}"/> class.
    /// </summary>
    /// <param name="dictionary">
    /// The <see cref="IDictionary{TKey, TValue}"/> whose elements are copied to the
    /// new <see cref="ExtendableEnumDictionary{TKey, TValue}"/>.
    /// </param>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{T}"/> implementation to use when
    /// comparing keys, or null to use the default <see cref="IEqualityComparer{T}"/>
    /// for the type of the key.
    /// </param>
    public ExtendableEnumDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        : base(dictionary, comparer)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExtendableEnumDictionary{TKey, TValue}"/> class.
    /// </summary>
    /// <param name="capacity">
    /// The initial number of elements that the <see cref="ExtendableEnumDictionary{TKey, TValue}"/>
    /// can contain.
    /// </param>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{T}"/> implementation to use when
    /// comparing keys, or null to use the default <see cref="IEqualityComparer{T}"/>
    /// for the type of the key.
    /// </param>
    public ExtendableEnumDictionary(int capacity, IEqualityComparer<TKey> comparer)
        : base(capacity, comparer)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExtendableEnumDictionary{TKey, TValue}"/> class.
    /// </summary>
    /// <param name="dictionary">
    /// The <see cref="IDictionary{TKey, TValue}"/> whose elements are copied to the
    /// new <see cref="ExtendableEnumDictionary{TKey, TValue}"/>.
    /// </param>
    public ExtendableEnumDictionary(IDictionary<TKey, TValue> dictionary)
        : base(dictionary)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExtendableEnumDictionary{TKey, TValue}"/> class.
    /// </summary>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{T}"/> implementation to use when
    /// comparing keys, or null to use the default <see cref="IEqualityComparer{T}"/>
    /// for the type of the key.
    /// </param>
    public ExtendableEnumDictionary(IEqualityComparer<TKey> comparer)
        : base(comparer)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExtendableEnumDictionary{TKey, TValue}"/> class.
    /// </summary>
    /// <param name="capacity">
    /// The initial number of elements that the <see cref="ExtendableEnumDictionary{TKey, TValue}"/>
    /// can contain.
    /// </param>
    public ExtendableEnumDictionary(int capacity)
        : base(capacity)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExtendableEnumDictionary{TKey, TValue}"/> class.
    /// </summary>
    /// <param name="info">
    /// A <see cref="SerializationInfo"/> object containing the information.
    /// </param>
    /// <param name="context">
    /// A <see cref="StreamingContext"/> structure containing the source
    /// and destination of the serialized stream associated with
    /// the <see cref="ExtendableEnumDictionary{TKey, TValue}"/>.
    /// </param>
    protected ExtendableEnumDictionary(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    /// <inheritdoc/>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        // just a line to satisfy analyzer
        Debug.WriteLine("Getting object data...");
        base.GetObjectData(info, context);
    }
}
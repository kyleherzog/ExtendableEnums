﻿using System;
using System.Collections.Generic;
using ExtendableEnums.Testing.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace ExtendableEnums.UnitTests.ExpandableEnumerationTests;

[TestClass]
public class SerializeShould
{
    [TestMethod]
    public void DeserializeFromNull()
    {
        var nullSerialized = JsonConvert.SerializeObject(null);
        var status = JsonConvert.DeserializeObject<SampleStatus>(nullSerialized);
        Assert.IsNull(status);
    }

    [TestMethod]
    public void DeserializeFromObjectGivenNumericValuePripertyNotDeclaredInPrimaryType()
    {
        var status = JsonConvert.DeserializeObject<SampleStatus>($"{{\"value\" : \"{SampleStatusDeclared.Pending.Value}\"}}");
        Assert.AreEqual(SampleStatusDeclared.Pending, status);
    }

    [TestMethod]
    public void DeserializeFromObjectWithNoValuePropertyToDefaultValue()
    {
        var status = JsonConvert.DeserializeObject<SampleStatus>($"{{\"id\" : \"{SampleStatus.Inactive.Value}\"}}");
        Assert.AreEqual(SampleStatus.Unknown, status);
    }

    [TestMethod]
    public void DeserializeFromObjectWithNumericValueProperty()
    {
        var status = JsonConvert.DeserializeObject<SampleStatus>($"{{\"value\" : {SampleStatus.Inactive.Value}}}");
        Assert.AreEqual(SampleStatus.Inactive, status);
    }

    [TestMethod]
    public void DeserializeFromObjectWithStringValueProperty()
    {
        var status = JsonConvert.DeserializeObject<SampleStatusByString>($"{{\"value\" : \"{SampleStatusByString.Inactive.Value}\"}}");
        Assert.AreEqual(SampleStatusByString.Inactive, status);
    }

    [TestMethod]
    public void DeserializeFromObjectNotDefined()
    {
        var status = JsonConvert.DeserializeObject<SampleStatus>("{\"value\" : -123}");
        Assert.AreEqual(-123, status?.Value);
    }

    [TestMethod]
    public void DeserializeFromSerializedDictionaryGivenExtendedEnumIsKey()
    {
        var dictionary = new Dictionary<SampleStatus, string>();
        var keyStatus = SampleStatus.Discontinued;
        dictionary.Add(keyStatus, "test value");
        var serialized = JsonConvert.SerializeObject(dictionary);
        Console.WriteLine(serialized);
        var deserialized = JsonConvert.DeserializeObject<Dictionary<SampleStatus, string>>(serialized);
        Assert.AreEqual(dictionary[keyStatus], deserialized?[keyStatus]);
    }

    [TestMethod]
    public void DeserializeFromTheValueOnly()
    {
        var status = JsonConvert.DeserializeObject<SampleStatus>($"{SampleStatus.Inactive.Value}");
        Assert.AreEqual(SampleStatus.Inactive, status);
    }

    [TestMethod]
    public void SerializeTheValueOnly()
    {
        var status = SampleStatus.Active;
        var serialized = JsonConvert.SerializeObject(status);

        Assert.AreEqual($"{status.Value}", serialized);
    }

    [TestMethod]
    public void SerializeToNull()
    {
        SampleStatus? status = null;
        var serialized = JsonConvert.SerializeObject(status);
        Assert.AreEqual("null", serialized);
    }
}
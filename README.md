# Extendable Enums
[![Build status](https://ci.appveyor.com/api/projects/status/9w357to4mu4ds05u?svg=true)](https://ci.appveyor.com/project/kyleherzog/extendableenums)

This library is available from [NuGet.org](https://www.nuget.org/packages/ExtendableEnums/)
or download from the [CI build feed](https://ci.appveyor.com/nuget/extendableenums).

--------------------------

A .NET Standard class library that provides base classes for creating enumerations that can be extended with additional class memebers. 

See the [changelog](CHANGELOG.md) for changes and roadmap.

## Features

- Get all enumeration items
- Comparison on different instances based on enumeration item value.
- Core value can be of any `IComparible` type
- Serializes to value only

### Creating an Extended Enumerable
Enumerables based on an `int` value can be created by inheriting from `ExtendableEnum<TEnumeration>`. The constructor must be overridden and should be made private.  Add any extra properties as desired.  Then just define each enumeration value as a static read only field as shown in the following example.


```c#
public class SampleStatus : ExtendableEnums.ExtendableEnum<SampleStatus>
    {
        public static readonly SampleStatus Active = new SampleStatus(1, nameof(Active), "ACT");
        public static readonly SampleStatus Discontinued = new SampleStatus(2, nameof(Discontinued), "DIS");
        public static readonly SampleStatus Inactive = new SampleStatus(3, nameof(Inactive), "INA");

        private SampleStatus(int value, string displayName, string code)
            : base(value, displayName)
        {
            Code = code;
        }

        public string Code { get; private set; }
    }
```

To create an extended enumeration based on a different type of value, just inherit from `ExtendedEnumBase<TEnumeration, TValue>` instead, specifying the type desired for the `TValue` parameter.

### Getting All Values
All values that have been defined in an derived enumeration class can be retrieved by calling the static `GetAll()` method.

### Min/Max Values
The minimum or maximum values in an enumeration can be retrieved by calling the static `Min` and `Max` properties.

## License
[Apache 2.0](LICENSE)
# Extendable Enums
[![Build Status](https://kyleherzog.visualstudio.com/ExtendableEnums/_apis/build/status/ExtendableEnums?branchName=develop)](https://kyleherzog.visualstudio.com/ExtendableEnums/_build/latest?definitionId=2?branchName=develop)

This library is available from [NuGet.org](https://www.nuget.org/packages/ExtendableEnums/).

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

### Defining Values in Multiple Classes
Values can be defined in multiple classes.  However, upon startup of the assembly, the `DeclaringTypes` property must be set to include any classes that define the values other than the primary class that inherits from `ExtendableEnumBase` as seen in the following example.
```
MyEnum.DeclaringTypes.Add(typeof(MyEnumExtraValuesClass));
```

### Min/Max Values
The minimum or maximum values in an enumeration can be retrieved by calling the static `Min` and `Max` properties.

### OData Support
Using ExtendedableEnums in OData requires some modifications.

#### OData ASP.net Core Server Support
Support for adding ExtendableEnums to an ASP.net core OData server can be achieved by adding a NuGet package reference to `ExtendableEnums.Microsoft.AspNetCore.OData`.  Once this packages is added, the EDM model will need to register each ExtendableEnum type by calling an `ODataConventionModelBuilder` extension method called `AddExtendableEnum<>` as seen in the following example.
```
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    DataContext.ResetData();

    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseHsts();
    }

    app.UseMvc(routebuilder =>
    {
        routebuilder.Select().Expand().Filter().OrderBy().MaxTop(100).Count();
        routebuilder.MapODataServiceRoute("odata", "odata", GetEdmModel());
    });
}

private static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    builder.AddExtendableEnum<SampleStatus>();
    builder.EntitySet<SampleBook>("SampleBooks");

    return builder.GetEdmModel();
}
```

Any POST methods for objects that have a property that includes an ExtendableEnum type can not have the parameter be the object type directly, but rather a `JObject` that is then converted to the actual object in the controller method.  This can be seen in the following example.
```        
[EnableQuery]
public IActionResult Post([FromBody] JObject json)
{
    var book = json.ToObject<SampleBook>();
    var matchingBook = books.FirstOrDefault(b => b.Id == book.Id);
    if (matchingBook == null)
    {
        books.Add(book);
        return Created(book);
    }
    else
    {
        var index = books.IndexOf(matchingBook);
        books.RemoveAt(index);
        books.Insert(index, book);
        return Updated(book);
    }
}
```

#### OData Simple.OData.Client Support
Compatibility with Simple.Odata.Client can be obtained by adding a NuGet package reference to `ExtendableEnums.Simple.OData.Client`. Once this package is added, a call can be made to `ExtendableEnumConverter.Register<>(ODataClientSettings)`.  This will allow Simple.OData.Client handle the minimal serialization of ExtendableEnums.  
```
var settings = new ODataClientSettings
{
    BaseUri = TestingHost.Instance.BaseODataUrl,
    IgnoreUnmappedProperties = true
};

ExtendableEnumConverter.Register<SampleStatus>(settings);
var client = new ODataClient(settings);
```

All extended properties on any ExtendableEnums will also need to have the `NotMappedAttribute` applied as well.
```        
[NotMapped]
public string Code { get; }
```

## License
[MIT](LICENSE)
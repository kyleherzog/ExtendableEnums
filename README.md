# Extendable Enums
[![Build Status](https://kyleherzog.visualstudio.com/ExtendableEnums/_apis/build/status/ExtendableEnums?branchName=develop)](https://kyleherzog.visualstudio.com/ExtendableEnums/_build/latest?definitionId=2?branchName=develop)

This library is available from [NuGet.org](https://www.nuget.org/packages/ExtendableEnums/).

--------------------------

A .NET Standard class library that provides base classes for creating enumerations that can be extended with additional class members. 

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

### As an IDictionary Key
When it is desired to use an ExtendableEnum as a key in a generic dictionary, `ExtendableEnumDictionary{TKey, TValue}` should be used in order to ensure proper serialization. 

### ASP.net Core Support

#### Tag Helpers
A select tag helper is available through the `ExtendableEnums.Microsoft.AspNetCore` NuGet package. In order to use the tag helper, a call to `addTagHelper` must be added to the \_ViewImports.cshtml.
```
@addTagHelper *, ExtendableEnums.Microsoft.AspNetCore
```

Then, just add a select tag to the desired view with the `extendable-enum-for` attribute set to the ExtendableEnum property of the model.
```
<select extendable-enum-for="Status" ></select>
```

By default the select list options will be sorted by the `DisplayName` property.  To sort by the Value property, add the attribute `extendable-enum-order-by-value` and set the value of the attribute to true.

NOTE: This select tag helper requires the setup of the Model Binding, which is described next.

#### Model Binding
By default, ASP.Net will model bind ExtendableEnums by their DisplayName property.  In order to do model binding by the Value property, ExtendableEnums must be registered when configuring services in ASP.net core projects.  This is done by calling `UseExtendableEnumModelBinding` on the `MvcOptions` parameter when calling `AddMvc`.
```
 services.AddMvc(options =>
{
    options.UseExtendableEnumModelBinding();
});
```

### Entity Framework Core Support
ExtendableEnums can be stored by their Value property in Entity Framework Core.  In order to do this, value converters must be registered by calling the `ApplyExtendableEnumConversions` extension method on the ModelBuilder.

```        
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.ApplyExtendableEnumConversions();
    base.OnModelCreating(modelBuilder);
}
```

### OData Support
Using ExtendedableEnums in OData requires some modifications.

#### OData ASP.net Core Server Support
Support for adding ExtendableEnums to an ASP.net core OData server can be achieved by adding a NuGet package reference to `ExtendableEnums.Microsoft.AspNetCore.OData`.  Once this packages is added, the EDM model will need to register each ExtendableEnum type.  This can be done for all types that inherit from `ExtendableEnumBase` in a given assembly by calling an `ODataConventionModelBuilder` extension method called `AddAllExtendableEnums`. 
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
    builder.AddAllExtendableEnums(Assembly.GetExecutingAssembly());
    builder.EntitySet<SampleBook>("SampleBooks");

    return builder.GetEdmModel();
}
```

A reference type can be passed to `AddAllExtendableEnums` instead of passing the assembly directly.  When this is done, the containing assembly of the type specified is scanned for ExtendableEnums to be registered.

```
builder.AddAllExtendableEnums(this.GetType());
```


If desired, ExtendableEnum types can be registered individually as well, by calling an `ODataConventionModelBuilder` extension method called `AddExtendableEnum<>` as seen in the following example.
```
builder.AddExtendableEnum<SampleStatus>();
```

Any POST methods for objects that have a property that includes an ExtendableEnum type can not have the parameter be the object type directly, but rather a `JsonElement` that is then converted to the actual object in the controller method.  This can be seen in the following example.
```        
[EnableQuery]
public IActionResult Post([FromBody] JsonElement json)
{
    var book = JsonConvert.DeserializeObject<SampleBook>(json.GetRawText());
    var matchingBook = books.FirstOrDefault(b => b.Id == book.Id);
    if (matchingBook is null)
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
Compatibility with Simple.Odata.Client can be obtained by adding a NuGet package reference to `ExtendableEnums.Simple.OData.Client`. Once this package is added, a call can be made to the `ODataClientSettings` extension method `RegisterAllExtendableEnums` to register all ExtendableEnums in a given assembly.  This will allow Simple.OData.Client handle the minimal serialization of ExtendableEnums.  
```
var clientSettings = new ODataClientSettings
{
    BaseUri = TestingHost.Instance.BaseODataUrl,
    IgnoreUnmappedProperties = true
};

clientSettings.RegisterAllExtendableEnums(Assembly.GetExecutingAssembly());
var client = new ODataClient(clientSettings);
```
A reference type can be passed to `RegisterAllExtendableEnums` instead of passing the assembly directly.  When this is done, the containing assembly of the type specified is scanned for ExtendableEnums to be registered.

```
clientSettings.RegisterAllExtendableEnums(this.GetType());
```

If desired, ExtendableEnum types can be registered individually as well, by calling `Register<>`.
```
settings.RegisterExtendableEnum<SampleStatus>();
```

All extended properties on any ExtendableEnums will also need to have the `NotMappedAttribute` applied as well.
```        
[NotMapped]
public string Code { get; }
```

### LiteDB Support
Using ExtendedableEnums in LiteDB requires registering Bson mappings.  This can be done by adding a reference to the NuGet package `ExtendableEnums.LiteDB`.  Then, upon application startup, call the needed `BsonMapper` extension registration methods.  Currently, Int32 and string Value property types are supported.

```
BsonMapper.Global.RegisterAllInt32BasedExtendableEnums(Assembly.GetExecutingAssembly());
```

A reference type can also be passed to register all types in the containing assembly.

```
BsonMapper.Global.RegisterAllInt32BasedExtendableEnums(typeof(SampleBook));
```

If desired, individual ExtendableEnum types can be registered by calling `RegisterExtendableEnumAsInt32`
```
BsonMapper.Global.RegisterExtendableEnumsAsInt32(typeof(SampleStatus))
```
## License
[MIT](LICENSE)
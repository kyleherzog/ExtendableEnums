# Roadmap
- [ ] ???


# Changelog

These are the changes to each version that has been released
on NuGet.org.

## 5.0
**2022-2-28**
- [x] Enabled Nullable References
- [x] Updated Microsoft.EntityFrameworkeCore to v6.0.2
- [x] Updated Simple.OData.Client to v5.26.0
- [x] Updated to Newtonsoft.Json v13.0.1
- [x] Updaetd to System.ComponentModel.Annotations v5.0.0 
- [x] ExtendableEnum.EntityFrameworkCore is now a .NET 6.0 library
- [x] ExtendableEnum.Microsoft.AspNetCore is now a .NET 6.0 library
- [x] ExtendableEnum.Microsoft.AspNetCore.OData is now a .NET 6.0 library


## 4.1
**2020-9-8**
- [x] Updated Simple.OData.Client to v5.14.0

## 4.0
**2020-6-2**
- [x] ASP.NET Core 3.x compatibility

## 3.1
**2020-4-9**
- [x] IComparable interface implemented

## 3.0
**2020-4-5**
- [x] Added ExtendableEnumDictionary{TKey, TValue}
- [x] Falling back to trying to deserialize from DisplayName if cannot deserialize from Value before creating a new value 

## 2.1
**2020-1-25**
- [x] Fix for ASP.NET model binding nulls. 

## 2.0
**2020-1-4**
- [x] Added ParseValueOrCreate. This is now the default method used when deserializing/converting.

## 1.11
**2019-11-27**
- [x] ASP.NET tag helper HtmlFieldPrefix compatibility support.

## 1.10
**2019-11-25**
- [x] Added option to order items by value or text for ASP.NET core select tag helper.

## 1.9
**2019-11-6**
- [x] Entity Framework Core support added.

## 1.8
**2019-8-26**
- [x] Fix for deserializing from null.

## 1.7
**2019-8-2**
- [x] Switched OData.Client registration methods to be extension methods.

## 1.6
**2019-7-29**
- [x] Added ASP.NET core select tag helper.

## 1.5
**2019-7-29**
- [x] Added support for registering Bson mappings for int and string based ExtendableEnums with LiteDb.

## 1.4
**2019-7-26**
- [x] Added AspNetCore model binding support.
- [x] Added IsExtendableEnum extension method to `Type` class
- [x] Added `AddAllExtendableEnums` method to ExtendableEnums.Microsoft.AspNetCore.OData library to allow bulk registration of ExtendableEnum types.
- [x] Added `RegisterAll` to ExtendableEnums.Simple.OData.Client.ExtendableEnumConverter to allow for bulk registration of ExtendableEnum types.


## 1.3
**2019-5-15**
- [x] Added option to declare instances of the same ExtendableEnum across multiple classes.

## 1.2
**2019-2-3**
- [x] Added ExtendableEnumTypeConverter to allow for ExtendableEnums to be serialized as dictionary keys

## 1.1
**2019-1-21**
- [x] Support for deserializing from JSON object with `value` property
- [x] Added supporting packages for using ExtendableEnums with OData

## 1.0

**2018-12-15**

- [x] Initial release




 
 
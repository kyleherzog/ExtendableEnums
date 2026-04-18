# Max — History

## Project Context

**Project:** ExtendableEnums — a .NET Standard multi-package library providing base classes for creating enumerations that can be extended with additional class members.

**Stack:** C# / .NET Standard, xUnit, NuGet multi-pack solution

**Packages in this repo:**
- `ExtendableEnums` — core library
- `ExtendableEnums.EntityFrameworkCore` — EF Core value converters
- `ExtendableEnums.Microsoft.AspNetCore` — tag helpers, model binding
- `ExtendableEnums.Microsoft.AspNetCore.OData` — OData EDM registration
- `ExtendableEnums.Simple.OData.Client` — Simple.OData.Client serialization
- `ExtendableEnums.Serialization.SystemText` — System.Text.Json converters
- `ExtendableEnums.LiteDB` — LiteDB Bson mapping
- `ExtendableEnums.Testing` / `ExtendableEnums.Testing.Models` — shared test helpers

**Owner:** Kyle Herzog

**Team:** Buddy (Lead), Max (.NET Dev), Bella (Tester), Charlie (Docs & DevRel), Scribe (Logger), Ralph (Monitor)

## Learnings

### 2026-04-18 — Newtonsoft Base Classes Implementation Complete

- Successfully created `ExtendableEnumBase.cs` and `ExtendableEnum.cs` in `ExtendableEnums.Serialization.Newtonsoft` package
- Base classes inherit from core classes with type constraints referencing core `ExtendableEnums.ExtendableEnumBase` 
- Attributes properly applied: `[JsonConverter(typeof(ExtendableEnumJsonConverter))]` on both classes
- Created supporting files: `ExtendableEnumContractResolver.cs` for polymorphic serialization, `JsonSerializerSettingsExtensions.cs` for fluent configuration
- Extension methods provide one-line registration for consumers: `settings.UseExtendableEnums()`
- Package ready for consumer migration: only requires changing `using` statement from `ExtendableEnums` to `ExtendableEnums.Serialization.Newtonsoft`
- All code staged for testing phase

# Buddy — History

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

### 2026-04-18 — Newtonsoft Base Classes Architecture Review

- Approved base class strategy: same names (`ExtendableEnumBase<TEnumeration, TValue>`, `ExtendableEnum<TEnumeration>`) in different namespace (`ExtendableEnums.Serialization.Newtonsoft`)
- Critical design point: type constraints MUST reference core `ExtendableEnums.ExtendableEnumBase`, not the Newtonsoft wrapper — ensures inheritance chain compiles correctly for consumers
- Attribute placement: apply `[JsonConverter]` to both classes for discoverability; harmless redundancy with contract resolver
- Dictionary pattern: keep `SerializableExtendableEnumDictionary` as-is (concrete class, both names can coexist without ambiguity)
- Core package remains 100% unchanged — purely additive approach maintains backward compatibility for non-Newtonsoft consumers
- Migration path clear: Option A (change namespace) or Option B (use resolver with core base class)
- All 7 unit tests passing; architecture ready for downstream work
- Decision documented in `.squad/decisions.md`

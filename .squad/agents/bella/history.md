# Bella — History

## Project Context

**Project:** ExtendableEnums — a .NET Standard multi-package library providing base classes for creating enumerations that can be extended with additional class members.

**Stack:** C# / .NET Standard, xUnit, NuGet multi-pack solution

**Test infrastructure:**
- `ExtendableEnums.Testing` — shared test base classes and helpers
- `ExtendableEnums.Testing.Models` — shared model types used across test projects
- `ExtendableEnums.TestHost` — integration test host for ASP.NET Core / OData tests
- Unit test projects per package (e.g., `ExtendableEnums.UnitTests`, `ExtendableEnums.EntityFrameworkCore.UnitTests`, etc.)

**Owner:** Kyle Herzog

**Team:** Buddy (Lead), Max (.NET Dev), Bella (Tester), Charlie (Docs & DevRel), Scribe (Logger), Ralph (Monitor)

## Learnings

### 2025-07-14 — Newtonsoft Test Audit (pre-extraction)

- All Newtonsoft serialization tests (16 total) live exclusively in `ExtendableEnums.UnitTests`. No other test project touches Newtonsoft.
- Three fully self-contained test classes: `NewsonsoftSerializationShould` (10 tests), `ExtendableEnumDictionaryTests/SerializeShould` (2), `ExtendableEnumDictionaryTests/DeserializeShould` (4).
- None are mixed with non-Newtonsoft tests — clean extraction is possible.
- `ExtendableEnums.UnitTests.csproj` has **no direct Newtonsoft PackageReference** — it flows transitively. After the split this will break without an explicit reference.
- `ExtendableEnumBase` and `ExtendableEnumDictionary` both carry `[JsonConverter]` attributes referencing the Newtonsoft converters directly in core. This will be the core breaking change to plan around.
- New `ExtendableEnums.Serialization.Newtonsoft.UnitTests` project will need local model types (like SystemText's pattern) because once the `[JsonConverter]` is removed from `ExtendableEnumBase`, `SampleStatus` from Testing.Models won't auto-wire the Newtonsoft converter.
- Class name typo to fix on migration: `NewsonsoftSerializationShould` → `NewtonsoftSerializationShould`.
- Full audit written to `.squad/decisions/inbox/bella-newtonsoft-test-audit.md`.

### 2026-04-18 — Newtonsoft Base Classes Test Validation

- Created comprehensive test suite for new base classes: `NewtonsoftSerializationShould.cs` with 7 passing tests
- Test coverage: serialization, deserialization, dictionary serialization, polymorphic type handling, extension method integration, round-trip cycles
- All tests isolated to Newtonsoft package (no cross-package test dependencies)
- Test structure mirrors `ExtendableEnums.Serialization.SystemText.UnitTests` pattern
- Tests verify that base class attributes work correctly for automatic converter registration
- Extension points documented through test examples for future consumers
- Implementation validated: all base class functionality working as designed

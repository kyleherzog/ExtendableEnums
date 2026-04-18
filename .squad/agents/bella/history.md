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

### 2026-04-18 — Namespace Rename Test Validation

- Validated test suite after Max's namespace rename from `ExtendableEnums` to `ExtendableEnums.Core` across 33 files
- **Test Results:** 126 total tests, 124 passed, 2 failed (both environmental, not regression)
- **Failed tests:** `ExtendableEnums.EntityFrameworkCore.UnitTests` (2 tests on net8.0 and net10.0 frameworks)
  - Failure cause: LocalDB database 'ExtendableEnumTests' permission/access issues
  - Error: "User does not have permission to alter database" and "There is already an object named 'People'"
  - **NOT a namespace regression** — purely environmental/infrastructure issue with SQL Server LocalDB
- **All other test projects passed cleanly:**
  - ExtendableEnums.UnitTests (net10.0) ✓
  - ExtendableEnums.Serialization.SystemText.UnitTests (net10.0) ✓
  - ExtendableEnums.Serialization.Newtonsoft.UnitTests (net10.0) ✓
  - ExtendableEnums.LiteDB.UnitTests (net10.0) ✓
  - ExtendableEnums.Microsoft.AspNetCore.UnitTests (net10.0) ✓
  - ExtendableEnums.Simple.OData.Client.UnitTests (net10.0) ✓
- **Verdict:** Namespace rename is clean from a test perspective. EF Core failures are pre-existing environmental issues requiring LocalDB setup/permissions.

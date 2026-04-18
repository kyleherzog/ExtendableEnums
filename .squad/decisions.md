# Squad Decisions

## Active Decisions

### Decision: Extract Newtonsoft.Json from Core Package

**Author:** Buddy (Lead)  
**Date:** 2026-04-13  
**Status:** Proposed  
**Affects:** ExtendableEnums (core), ExtendableEnums.Microsoft.AspNetCore, all downstream consumers

---

## Problem

The core `ExtendableEnums` NuGet package has a hard dependency on `Newtonsoft.Json` v13.0.4. This forces every consumer to pull in Newtonsoft even if they only use System.Text.Json or no JSON serialization at all. In v10.0 we already extracted System.Text.Json support into `ExtendableEnums.Serialization.SystemText` — Newtonsoft should get the same treatment.

## Decision Summary

Extract all Newtonsoft.Json serialization support into a new `ExtendableEnums.Serialization.Newtonsoft` package. Remove the `Newtonsoft.Json` PackageReference and `[JsonConverter]` attributes from the core package. This is a **breaking change** requiring a **major version bump (v10 → v11)**.

---

## 1. New Package Name

**`ExtendableEnums.Serialization.Newtonsoft`**

Rationale: mirrors the existing `ExtendableEnums.Serialization.SystemText` naming convention exactly.

## 2. What Moves

### Files moving from `ExtendableEnums\` → new project `ExtendableEnums.Serialization.Newtonsoft\`

| Source (current location) | Destination | Notes |
|---|---|---|
| `ExtendableEnums\Serialization\Newtonsoft\ExtendableEnumJsonConverter.cs` | `ExtendableEnums.Serialization.Newtonsoft\ExtendableEnumJsonConverter.cs` | Namespace stays `ExtendableEnums.Serialization.Newtonsoft` — no change needed |
| `ExtendableEnums\Serialization\Newtonsoft\ExtendableEnumDictionaryJsonConverter.cs` | `ExtendableEnums.Serialization.Newtonsoft\ExtendableEnumDictionaryJsonConverter.cs` | Namespace stays `ExtendableEnums.Serialization.Newtonsoft` — no change needed |

### New file to create in `ExtendableEnums.Serialization.Newtonsoft\`

| File | Purpose |
|---|---|
| `SerializableExtendableEnumDictionary.cs` | Subclass of `ExtendableEnumDictionary<TKey, TValue>` with `[Newtonsoft.Json.JsonConverter(typeof(ExtendableEnumDictionaryJsonConverter))]` attribute applied — mirrors the SystemText pattern |

### What changes in core (`ExtendableEnums\`)

| File | Change |
|---|---|
| `ExtendableEnumBase.cs` | **Remove** line 4 (`using ExtendableEnums.Serialization.Newtonsoft;`), line 5 (`using Newtonsoft.Json;`), and line 14 (`[JsonConverter(typeof(ExtendableEnumJsonConverter))]`) |
| `ExtendableEnumDictionary.cs` | **Remove** line 3 (`using ExtendableEnums.Serialization.Newtonsoft;`), line 4 (`using Newtonsoft.Json;`), and line 14 (`[JsonConverter(typeof(ExtendableEnumDictionaryJsonConverter))]`) |
| `ExtendableEnums.csproj` | **Remove** `<PackageReference Include="Newtonsoft.Json" Version="13.0.4" />` |
| `Serialization\Newtonsoft\` folder | **Delete** entire folder (files moved to new project) |

### What changes in `ExtendableEnums.Microsoft.AspNetCore\`

| File | Change |
|---|---|
| `ExtendableEnumBinder.cs` | **Rewrite** to use `TypeConverter` instead of `JsonConvert.DeserializeObject`. The core already has `[TypeConverter(typeof(ExtendableEnumTypeConverter))]` on `ExtendableEnumBase` — use `TypeDescriptor.GetConverter()` instead. This eliminates the ASP.NET Core package's implicit Newtonsoft dependency entirely. |

The rewritten binder should look like:
```csharp
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExtendableEnums.Microsoft.AspNetCore;

public class ExtendableEnumBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext is null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var modelName = bindingContext.ModelName;
        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None
            || string.IsNullOrEmpty(valueProviderResult.FirstValue))
        {
            bindingContext.Result = ModelBindingResult.Success(null);
            return Task.CompletedTask;
        }

        var converter = TypeDescriptor.GetConverter(bindingContext.ModelType);
        var result = converter.ConvertFromString(valueProviderResult.FirstValue);

        bindingContext.Result = ModelBindingResult.Success(result);
        return Task.CompletedTask;
    }
}
```

### Test project changes

| Project | Change |
|---|---|
| **New:** `ExtendableEnums.Serialization.Newtonsoft.UnitTests\` | Create test project mirroring `ExtendableEnums.Serialization.SystemText.UnitTests` structure. Move Newtonsoft-specific tests here. |
| `ExtendableEnums.UnitTests\` | **Move** `ExtendableEnumTests\NewsonsoftSerializationShould.cs` to the new Newtonsoft test project |
| `ExtendableEnums.UnitTests\` | **Move** `ExtendableEnumDictionaryTests\DeserializeShould.cs` to the new Newtonsoft test project |
| `ExtendableEnums.UnitTests\` | **Move** `ExtendableEnumDictionaryTests\SerializeShould.cs` to the new Newtonsoft test project |
| `ExtendableEnums.UnitTests\ExtendableEnums.UnitTests.csproj` | Verify no remaining Newtonsoft.Json references after test removal |
| `ExtendableEnums.Microsoft.AspNetCore.UnitTests\ModelBindingTests.cs` | Replace `JsonConvert.DeserializeObject<T>` calls with `System.Text.Json.JsonSerializer.Deserialize<T>` (these are just test assertions deserializing HTTP response bodies — not testing Newtonsoft behavior) |

## 3. Compatibility Strategy

### This is a breaking change. No way around it.

The `[JsonConverter(typeof(ExtendableEnumJsonConverter))]` attribute on `ExtendableEnumBase` is what makes `JsonConvert.SerializeObject(myEnum)` "just work" today. Removing it means consumers who rely on Newtonsoft serialization must take explicit action.

**Why type forwarding won't help:** `[assembly: TypeForwardedTo]` only works when the *forwarding* assembly references the *target* assembly — which would re-introduce the Newtonsoft dependency in core. Dead end.

**Why a compatibility shim won't help:** We can't conditionally apply `[JsonConverter]` at runtime. The attribute must be present at compile time or it doesn't exist.

### What consumers experience

| Consumer scenario | Impact | Action required |
|---|---|---|
| Uses Newtonsoft to serialize ExtendableEnums | **Breaks** — values serialize as full objects instead of just the `.Value` | Add `ExtendableEnums.Serialization.Newtonsoft` package + register converters |
| Uses System.Text.Json only | **No impact** — already using `ExtendableEnums.Serialization.SystemText` | None |
| Uses no JSON serialization | **Positive** — smaller dependency graph | None |
| Uses `ExtendableEnums.Microsoft.AspNetCore` | **No impact** after binder rewrite | Update to v11 |
| Has `ExtendableEnumDictionary` with Newtonsoft | **Breaks** — needs explicit converter | Use `SerializableExtendableEnumDictionary` from new package OR register converter globally |

## 4. Versioning

**Major version bump: v10 → v11**

Rationale:
- Removing `[JsonConverter]` from base types is a **breaking binary and behavioral change**
- Removing the `Newtonsoft.Json` transitive dependency breaks consumers who relied on it flowing through
- SemVer demands a major bump for breaking public API changes

All packages in the solution should bump to v11.0 together (they already share version 10.0).

## 5. Migration Path

### For consumers who need Newtonsoft support

1. **Add one NuGet package:**
   ```
   dotnet add package ExtendableEnums.Serialization.Newtonsoft
   ```

2. **Choose one registration approach:**

   **Option A — Global converter registration (recommended, one-time setup):**
   ```csharp
   JsonConvert.DefaultSettings = () => new JsonSerializerSettings
   {
       Converters = new List<JsonConverter>
       {
           new ExtendableEnumJsonConverter(),
           new ExtendableEnumDictionaryJsonConverter(),
       }
   };
   ```

   **Option B — Per-type attribute (if you only have a few types):**
   ```csharp
   [Newtonsoft.Json.JsonConverter(typeof(ExtendableEnumJsonConverter))]
   public class MyStatus : ExtendableEnum<MyStatus> { ... }
   ```

   **Option C — Per-call (ad hoc):**
   ```csharp
   var settings = new JsonSerializerSettings();
   settings.Converters.Add(new ExtendableEnumJsonConverter());
   JsonConvert.SerializeObject(myEnum, settings);
   ```

### For consumers NOT using Newtonsoft

No action needed. Enjoy the lighter dependency graph.

## 6. Implementation Order (for Max)

### Phase 1: Create the new Newtonsoft package

1. **Create project `ExtendableEnums.Serialization.Newtonsoft\ExtendableEnums.Serialization.Newtonsoft.csproj`**
   - Target `netstandard2.0`
   - Copy structure from `ExtendableEnums.Serialization.SystemText.csproj` as template
   - Add `PackageReference` for `Newtonsoft.Json` v13.0.4
   - Add `ProjectReference` to `ExtendableEnums.csproj`
   - Set Version to `11.0`, matching other packages
   - Description: "A .NET Standard library that provides Newtonsoft.Json support for ExtendableEnums."
   - PackageTags: "Enum Enumeration Extendable Class Serialization Newtonsoft Json"

2. **Move converter files into the new project:**
   - Copy `ExtendableEnums\Serialization\Newtonsoft\ExtendableEnumJsonConverter.cs` → `ExtendableEnums.Serialization.Newtonsoft\ExtendableEnumJsonConverter.cs`
   - Copy `ExtendableEnums\Serialization\Newtonsoft\ExtendableEnumDictionaryJsonConverter.cs` → `ExtendableEnums.Serialization.Newtonsoft\ExtendableEnumDictionaryJsonConverter.cs`
   - Namespaces remain `ExtendableEnums.Serialization.Newtonsoft` — no code changes needed in the converter files themselves

3. **Create `ExtendableEnums.Serialization.Newtonsoft\SerializableExtendableEnumDictionary.cs`**
   - Mirrors `ExtendableEnums.Serialization.SystemText\SerializableExtendableEnumDictionary.cs`
   - Applies `[Newtonsoft.Json.JsonConverter(typeof(ExtendableEnumDictionaryJsonConverter))]`

4. **Add new project to `ExtendableEnums.sln`**

### Phase 2: Create the Newtonsoft test project

5. **Create project `ExtendableEnums.Serialization.Newtonsoft.UnitTests\ExtendableEnums.Serialization.Newtonsoft.UnitTests.csproj`**
   - Copy structure from `ExtendableEnums.Serialization.SystemText.UnitTests.csproj`
   - Add `ProjectReference` to `ExtendableEnums.Serialization.Newtonsoft.csproj`
   - Add `PackageReference` for `Newtonsoft.Json` (for test assertions)

6. **Create test model types** in `ExtendableEnums.Serialization.Newtonsoft.UnitTests\Models\`:
   - `SerializableSampleStatus.cs` — applies `[Newtonsoft.Json.JsonConverter(typeof(ExtendableEnumJsonConverter))]`
   - `SerializableSampleStatusByString.cs`
   - `SerializableSampleStatusDeclared.cs`
   - Mirror the SystemText test models pattern

7. **Move/adapt Newtonsoft tests from core unit tests:**
   - Adapt `ExtendableEnums.UnitTests\ExtendableEnumTests\NewsonsoftSerializationShould.cs` → `ExtendableEnums.Serialization.Newtonsoft.UnitTests\NewtonsoftSerializationShould.cs`
   - Adapt `ExtendableEnums.UnitTests\ExtendableEnumDictionaryTests\DeserializeShould.cs` → corresponding test file
   - Adapt `ExtendableEnums.UnitTests\ExtendableEnumDictionaryTests\SerializeShould.cs` → corresponding test file
   - Tests should use the new `[JsonConverter]`-attributed test model types

8. **Add test project to `ExtendableEnums.sln`**

### Phase 3: Clean up core package

9. **Edit `ExtendableEnums\ExtendableEnumBase.cs`:**
   - Remove `using ExtendableEnums.Serialization.Newtonsoft;` (line 4)
   - Remove `using Newtonsoft.Json;` (line 5)
   - Remove `[JsonConverter(typeof(ExtendableEnumJsonConverter))]` (line 14)

10. **Edit `ExtendableEnums\ExtendableEnumDictionary.cs`:**
    - Remove `using ExtendableEnums.Serialization.Newtonsoft;` (line 3)
    - Remove `using Newtonsoft.Json;` (line 4)
    - Remove `[JsonConverter(typeof(ExtendableEnumDictionaryJsonConverter))]` (line 14)

11. **Edit `ExtendableEnums\ExtendableEnums.csproj`:**
    - Remove `<PackageReference Include="Newtonsoft.Json" Version="13.0.4" />`

12. **Delete `ExtendableEnums\Serialization\Newtonsoft\` folder** (both .cs files)
    - If `Serialization\` folder is now empty, delete it too

13. **Delete old Newtonsoft tests from `ExtendableEnums.UnitTests\`:**
    - Delete `ExtendableEnumTests\NewsonsoftSerializationShould.cs`
    - Delete `ExtendableEnumDictionaryTests\DeserializeShould.cs`
    - Delete `ExtendableEnumDictionaryTests\SerializeShould.cs`

### Phase 4: Fix downstream packages

14. **Rewrite `ExtendableEnums.Microsoft.AspNetCore\ExtendableEnumBinder.cs`:**
    - Replace `Newtonsoft.Json.JsonConvert` usage with `System.ComponentModel.TypeDescriptor.GetConverter()`
    - See code sample in section 2 above

15. **Update `ExtendableEnums.Microsoft.AspNetCore.UnitTests\ModelBindingTests.cs`:**
    - Replace `Newtonsoft.Json.JsonConvert.DeserializeObject<T>` with `System.Text.Json.JsonSerializer.Deserialize<T>` for response body parsing
    - These are test assertions only — not testing Newtonsoft behavior

### Phase 5: Version bump & build verification

16. **Bump version to 11.0 in all `.csproj` files** that have `<Version>` set:
    - `ExtendableEnums\ExtendableEnums.csproj`
    - `ExtendableEnums.Serialization.SystemText\ExtendableEnums.Serialization.SystemText.csproj`
    - `ExtendableEnums.Serialization.Newtonsoft\ExtendableEnums.Serialization.Newtonsoft.csproj` (new)
    - `ExtendableEnums.Microsoft.AspNetCore\ExtendableEnums.Microsoft.AspNetCore.csproj`
    - `ExtendableEnums.Microsoft.AspNetCore.OData\ExtendableEnums.Microsoft.AspNetCore.OData.csproj`
    - `ExtendableEnums.Simple.OData.Client\ExtendableEnums.Simple.OData.Client.csproj`
    - `ExtendableEnums.EntityFrameworkCore\ExtendableEnums.EntityFrameworkCore.csproj`
    - `ExtendableEnums.LiteDB\ExtendableEnums.LiteDB.csproj`

17. **Build entire solution** — verify zero errors

18. **Run all tests** — verify all pass

19. **Update `CHANGELOG.md`** with v11.0 entry:
    ```
    ## 11.0
    - [x] **BREAKING:** Removed Newtonsoft.Json dependency from core ExtendableEnums package
    - [x] **BREAKING:** Removed [JsonConverter] attributes from ExtendableEnumBase and ExtendableEnumDictionary
    - [x] Added ExtendableEnums.Serialization.Newtonsoft package for Newtonsoft.Json support
    - [x] Rewrote ExtendableEnumBinder to use TypeConverter instead of Newtonsoft.Json
    ```

20. **Update `README.md`** — change the Newtonsoft serialization section to reference the new package

---

## Risk Assessment

| Risk | Likelihood | Mitigation |
|---|---|---|
| Consumers miss the breaking change | Medium | Clear CHANGELOG, NuGet release notes, README update |
| `TypeConverter`-based binder behaves differently than `JsonConvert`-based one | Low | The `ExtendableEnumTypeConverter` already handles string→enum conversion; existing ASP.NET Core tests will catch regressions |
| Other packages in ecosystem depend on Newtonsoft flowing through core | Low | `Simple.OData.Client` does NOT use Newtonsoft directly; EF Core and LiteDB don't either |

## Open Questions

1. **Should the new Newtonsoft package provide a `ContractResolver` or extension method for easy global registration?** (Nice-to-have for v11.1, not blocking)
2. **Should we add `[Obsolete]` on v10.x first before removing in v11?** (Not practical — the attribute is on the base class, not on a method we can deprecate)

---

*This plan is ready for Max to execute. No further planning round needed.*

---

## Test Coverage Audit

**Audit Author:** Bella (Tester)  
**Date:** 2026-07-14  
**Context:** Pre-extraction audit to support splitting Newtonsoft.Json support out of the core `ExtendableEnums` package.

### Which test files/classes currently test Newtonsoft serialization?

All three live in **`ExtendableEnums.UnitTests`**:

| File | Class | Folder |
|------|-------|--------|
| `ExtendableEnumTests/NewsonsoftSerializationShould.cs` | `NewsonsoftSerializationShould` | `ExtendableEnumTests/` |
| `ExtendableEnumDictionaryTests/SerializeShould.cs` | `SerializeShould` | `ExtendableEnumDictionaryTests/` |
| `ExtendableEnumDictionaryTests/DeserializeShould.cs` | `DeserializeShould` | `ExtendableEnumDictionaryTests/` |

No other `*.UnitTests` projects reference Newtonsoft. Confirmed by repo-wide `grep` across all `.csproj` files — only `ExtendableEnums\ExtendableEnums.csproj` has a direct `PackageReference` to `Newtonsoft.Json 13.0.4`. The test project acquires it transitively.

### Which test project do they live in?

**`ExtendableEnums.UnitTests`** — the primary core unit test project.  
Project file: `ExtendableEnums.UnitTests\ExtendableEnums.UnitTests.csproj`  
Framework: `net10.0`, test framework: MSTest + FluentAssertions.

**Important:** The `.csproj` has **no direct `PackageReference` to `Newtonsoft.Json`**. The dependency flows transitively through `ExtendableEnums.Testing.Models` → `ExtendableEnums` (core), which has `Newtonsoft.Json 13.0.4`. Once Newtonsoft is extracted from core, these tests will break without an explicit package reference.

### What specific scenarios are covered?

#### `NewsonsoftSerializationShould` (10 tests — `ExtendableEnum<T>`)

| Test Method | Scenario |
|-------------|----------|
| `DeserializeFromNull` | Null round-trip: `null` → serialized → `null` |
| `DeserializeFromObjectGivenNumericValuePripertyNotDeclaredInPrimaryType` | Deserialize from declaring type (cross-type lookup via `DeclaringTypes`) |
| `DeserializeFromObjectWithNoValuePropertyToDefaultValue` | Object JSON with wrong property name → falls back to default value |
| `DeserializeFromObjectWithNumericValueProperty` | Object JSON with numeric `value` property (int-valued enum) |
| `DeserializeFromObjectWithStringValueProperty` | Object JSON with string `value` property (string-valued enum) |
| `DeserializeFromObjectNotDefined` | Object JSON with unknown value → creates new instance with that value |
| `DeserializeFromSerializedDictionaryGivenExtendedEnumIsKey` | Standard `Dictionary<SampleStatus, string>` round-trip (uses Newtonsoft's built-in dictionary handling via converter) |
| `DeserializeFromTheValueOnly` | Scalar JSON value (not object) → deserialization |
| `SerializeTheValueOnly` | Serialization writes the `.Value` scalar only (not object) |
| `SerializeToNull` | `null` enum → serializes to `"null"` |

#### `SerializeShould` (2 tests — `ExtendableEnumDictionary<TKey, TValue>`)

| Test Method | Scenario |
|-------------|----------|
| `SerializeKeyAsValueOnlyGivenIntValue` | `ExtendableEnumDictionary` with int-valued key serializes key as raw integer string |
| `SerializeKeyAsValueOnlyGivenStringValue` | `ExtendableEnumDictionary` with string-valued key serializes key as string value |

#### `DeserializeShould` (4 tests — `ExtendableEnumDictionary<TKey, TValue>`)

| Test Method | Scenario |
|-------------|----------|
| `DeserializeGivenValidSerialized` | Int-keyed dictionary deserializes by numeric key |
| `DeserializeGivenValidSerializedByString` | String-keyed dictionary deserializes by string value |
| `DeserializeGivenDisplayNameSerializedByString` | String-keyed: key is the display name (not internal value) → falls through to `TryParse` |
| `DeserializeGivenDisplayNameSerialized` | Int-keyed: key is the display name string → resolves via `TryParse` |

**Total: 16 Newtonsoft-specific tests.**

### Are any Newtonsoft tests mixed into general test classes?

**No — all three classes are 100% Newtonsoft-specific and fully self-contained.** They can be extracted without touching any non-Newtonsoft test file.

The `ExtendableEnumDictionaryTests/` folder only has these two files (`SerializeShould.cs` and `DeserializeShould.cs`), both Newtonsoft-only. The rest of `ExtendableEnumTests/` (20+ files) contains no Newtonsoft references at all.

One important coupling note: the `Initializer.cs` `[AssemblyInitialize]` registers `SampleStatusDeclared` as a declaring type for `SampleStatus`. The test `DeserializeFromObjectGivenNumericValuePripertyNotDeclaredInPrimaryType` in `NewsonsoftSerializationShould` depends on this. The new test project will need its own initializer that performs the equivalent registration.

### Recommended structure for `ExtendableEnums.Serialization.Newtonsoft.UnitTests`

Mirror the pattern of `ExtendableEnums.Serialization.SystemText.UnitTests`:

```
ExtendableEnums.Serialization.Newtonsoft.UnitTests/
├── ExtendableEnums.Serialization.Newtonsoft.UnitTests.csproj
├── Initializer.cs                          ← Assembly init: register DeclaringTypes
├── ExtendableEnumTests/
│   └── NewtonsoftSerializationShould.cs    ← Migrated from UnitTests (fix typo in class name)
├── ExtendableEnumDictionaryTests/
│   ├── SerializeShould.cs                  ← Migrated from UnitTests
│   └── DeserializeShould.cs               ← Migrated from UnitTests
└── Models/
    ├── NewtonsoftSampleStatus.cs           ← New: mirrors SerializableSampleStatus but with Newtonsoft [JsonConverter]
    ├── NewtonsoftSampleStatusByString.cs   ← New: mirrors SerializableSampleStatusByString
    └── NewtonsoftSampleStatusDeclared.cs   ← New: mirrors SerializableSampleStatusDeclared
```

#### `.csproj` dependencies needed

```xml
<PackageReference Include="Newtonsoft.Json" Version="13.0.4" />
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="18.4.0" />
<PackageReference Include="MSTest.TestAdapter" Version="4.2.1" />
<PackageReference Include="MSTest.TestFramework" Version="4.2.1" />
<PackageReference Include="FluentAssertions" Version="7.0.0" />
<!-- Plus analyzer references to match project conventions -->

<ProjectReference Include="..\ExtendableEnums.Serialization.Newtonsoft\ExtendableEnums.Serialization.Newtonsoft.csproj" />
```

Note: **Do NOT reference `ExtendableEnums.Testing.Models`** directly. The new models must use types decorated with `[JsonConverter(typeof(ExtendableEnums.Serialization.Newtonsoft.ExtendableEnumJsonConverter))]` explicitly — because once Newtonsoft is extracted from core, the `[JsonConverter]` attribute on `ExtendableEnumBase` will be removed (or point to the new package), and `SampleStatus` (from Testing.Models) may no longer automatically use the Newtonsoft converter.

#### Why local models are needed

Currently `ExtendableEnumBase` has `[JsonConverter(typeof(ExtendableEnumJsonConverter))]` hardcoded (from `ExtendableEnums.Serialization.Newtonsoft` namespace). After the split, the core `ExtendableEnumBase` will not have a Newtonsoft converter attribute. The new test models (like `SerializableSampleStatus` in the SystemText tests) must explicitly apply the correct converter, isolating the test project from the core.

### Things to watch out for

1. **Typo in class name:** `NewsonsoftSerializationShould` (missing 't') — rename to `NewtonsoftSerializationShould` when moving.
2. **`ExtendableEnumDictionaryTests` will be empty in `ExtendableEnums.UnitTests`** after migration — the folder and any containing test infrastructure should be removed from that project.
3. **`Initializer.cs` coupling** — the `DeclaringTypes` registration must be replicated in the new test project's assembly initializer.
4. **`ExtendableEnumDictionary` itself** — currently has `[JsonConverter(typeof(ExtendableEnumDictionaryJsonConverter))]` from Newtonsoft baked in. If that attribute is removed during the split, the dictionary serialization tests will need to pass a custom settings/serializer-settings object instead of relying on attribute-based registration.

---

## Migration Guide: v10 → v11 (Newtonsoft.Json Split)

**Author:** Charlie (Docs & DevRel)  
**Date:** 2026-04-13

### 1. Current README Sections Documenting Newtonsoft.Json Serialization

#### Identified Sections:
- **"### Serialization"** (lines 57–63)
  - Current text: "Serialization is supported natively when using Newtonsoft.Json."
  - Also mentions System.Text.Json.Serialization requirements

#### Context Section That References Newtonsoft:
- **"#### OData ASP.net Core Server Support"** (lines 150–169)
  - Uses `JsonConvert.DeserializeObject<>` from Newtonsoft.Json library

---

### 2. Draft: "Migrating from v10 to v11" Section for README.md

#### Proposed Location: Add new section after **Features** (before "### Creating an Extended Enumerable")

```markdown
## Migrating from v10 to v11

**What changed:** Newtonsoft.Json serialization support moved to a separate NuGet package.

### If you use Newtonsoft.Json serialization:

1. **Add the new package**
   ```
   dotnet add package ExtendableEnums.Serialization.Newtonsoft
   ```

2. **Register the converter in your dependency injection**
   ```csharp
   // In your startup configuration (Program.cs or Startup.cs)
   services.AddExtendableEnumNewtonsoftSupport();
   ```

3. **Done.** Your ExtendableEnums will serialize correctly with Newtonsoft.Json.

### If you use System.Text.Json:
No changes needed. Serialization support is still in the core package.
```

---

### 3. New "Newtonsoft.Json Serialization" Section for ExtendableEnums.Serialization.Newtonsoft README

#### Proposed Location: Create new section in package-specific README

```markdown
## ExtendableEnums.Serialization.Newtonsoft

Newtonsoft.Json serialization support for ExtendableEnums.

### Installation

```
dotnet add package ExtendableEnums.Serialization.Newtonsoft
```

### Configuration

Register the converter when configuring services:

```csharp
using ExtendableEnums.Serialization.Newtonsoft;

// In Program.cs or Startup.cs
services.AddExtendableEnumNewtonsoftSupport();
```

### Usage

After registration, Newtonsoft.Json will automatically serialize/deserialize ExtendableEnums to their core value:

```csharp
var status = SampleStatus.Active;  // value: 1

// Serializes to:
// "1"

// Deserializes from:
// "1" → SampleStatus.Active
```

### See Also
- [ExtendableEnums Main Documentation](../README.md)
```

---

### 4. CHANGELOG.md Entry Format for v11

#### Proposed Format (Semantic Versioning with clear breaking change callout):

```markdown
## [11.0.0] - YYYY-MM-DD

### Breaking Changes
- **BREAKING:** Newtonsoft.Json serialization support has been split into a separate NuGet package (`ExtendableEnums.Serialization.Newtonsoft`). 
  - If you use Newtonsoft.Json, add the new package and call `services.AddExtendableEnumNewtonsoftSupport()` during startup.
  - See [Migration Guide](README.md#migrating-from-v10-to-v11) for details.

### Removed
- Removed built-in Newtonsoft.Json converter from core package (moved to separate package)

### New Packages
- `ExtendableEnums.Serialization.Newtonsoft` — Newtonsoft.Json serialization support

### Fixed
- [Issue reference if applicable]

---
```

---

### Summary: What Developers See

| Item | What They Do |
|------|--------------|
| **Migration Task** | Add 1 NuGet package, add 1 line of code |
| **README Change** | New "Migrating v10 → v11" section (early in doc) |
| **New Package README** | Minimal config example |
| **CHANGELOG** | Clear breaking change notice with link to migration guide |

**Key principle:** No surprises. Developers scanning the README see "Migrating v10 → v11" early and immediately understand the 2-step fix.

---

### Decision: Newtonsoft Base Classes

**Author:** Buddy (Lead)  
**Date:** 2026-04-18  
**Status:** Approved  
**Requestor:** Kyle Herzog

---

## Problem

After splitting Newtonsoft.Json support into a separate package, consumers need a clear, namespace-based way to adopt Newtonsoft serialization. The architecture should force explicit code changes (via namespace switching) and mirror the existing `SerializableExtendableEnumDictionary` pattern.

## Decision Summary

Create two new abstract base classes in the `ExtendableEnums.Serialization.Newtonsoft` namespace with `[JsonConverter]` attributes pre-applied:

- `ExtendableEnumBase<TEnumeration, TValue>`  
- `ExtendableEnum<TEnumeration>`

These are identical in name to the core classes but in a different namespace, forcing consumers to explicitly change their `using` statement to opt into Newtonsoft serialization.

## Implementation Details

### 1. Class Design

Both classes inherit from the core `ExtendableEnums` base classes and apply the `[JsonConverter]` attribute:

```csharp
namespace ExtendableEnums.Serialization.Newtonsoft;

[JsonConverter(typeof(ExtendableEnumJsonConverter))]
public abstract class ExtendableEnumBase<TEnumeration, TValue>
    : ExtendableEnums.ExtendableEnumBase<TEnumeration, TValue>
    where TEnumeration : ExtendableEnums.ExtendableEnumBase<TEnumeration, TValue>
    where TValue : IComparable
{
    protected ExtendableEnumBase(TValue value, string displayName)
        : base(value, displayName) { }
}

[JsonConverter(typeof(ExtendableEnumJsonConverter))]
public abstract class ExtendableEnum<TEnumeration>
    : ExtendableEnumBase<TEnumeration, int>
    where TEnumeration : ExtendableEnums.ExtendableEnumBase<TEnumeration, int>
{
    protected ExtendableEnum(int value, string displayName)
        : base(value, displayName) { }
}
```

### 2. Type Constraint Design

**Critical:** Constraints reference the CORE base class, not the Newtonsoft wrapper:

```csharp
// CORRECT
where TEnumeration : ExtendableEnums.ExtendableEnumBase<TEnumeration, TValue>

// WRONG (would not compile)
where TEnumeration : ExtendableEnums.Serialization.Newtonsoft.ExtendableEnumBase<TEnumeration, TValue>
```

This allows the inheritance chain to work correctly:
1. Consumer's `MyStatus` inherits from `ExtendableEnums.Serialization.Newtonsoft.ExtendableEnum<MyStatus>`
2. Which inherits from `ExtendableEnums.Serialization.Newtonsoft.ExtendableEnumBase<MyStatus, int>`
3. Which inherits from `ExtendableEnums.ExtendableEnumBase<MyStatus, int>` ✅

### 3. Attribute Placement

Apply `[JsonConverter]` to **both classes**:
- Explicit is better than implicit for discoverability
- IntelliSense clearly shows the converter on whichever class is inspected
- No runtime harm — Newtonsoft uses the first converter found in the hierarchy
- The class attribute takes precedence over the contract resolver (harmless redundancy)

### 4. Dictionary Consistency

Keep `SerializableExtendableEnumDictionary` as-is (no rename to mirror-with-same-name pattern):
- Concrete class, not abstract — consumers instantiate it directly
- Both names can coexist in same file without ambiguity
- Renaming now would be a breaking change
- For abstract base classes, same-name-different-namespace is cleaner

### 5. Core Package Unchanged

The core `ExtendableEnums` namespace remains 100% unchanged:
- `ExtendableEnumBase<T, TValue>` — no changes
- `ExtendableEnum<T>` — no changes
- Zero breaking changes for existing consumers

## Migration Guide

### Option A: Change namespace only (use new base classes)
```diff
- using ExtendableEnums;
+ using ExtendableEnums.Serialization.Newtonsoft;

  public class MyStatus : ExtendableEnum<MyStatus> { }
```

### Option B: Keep core base class, use resolver
```csharp
using ExtendableEnums;

public class MyStatus : ExtendableEnum<MyStatus> { }

// In serialization setup:
var settings = new JsonSerializerSettings();
settings.Converters.Add(new ExtendableEnumJsonConverter());
```

## Status

✅ Approved and implemented  
✅ 7 tests passing (serialization, deserialization, contract resolution, round-trip)

---

---

### 2026-04-18: User Directive — System.Text.Json Base Classes Not Needed

**By:** Kyle Herzog (via Copilot)  
**Date:** 2026-04-18  
**Status:** Noted for guidance  

**What:** Do NOT add Newtonsoft-style base classes (with pre-applied [JsonConverter]) to `ExtendableEnums.Serialization.SystemText`. System.Text.Json does not honor `[JsonConverter]` attributes inherited from base classes, so the pattern would not work there.

**Why:** User request — captured for team memory

**Impact:** Keep System.Text.Json approach as-is (users must register converters explicitly or use the existing `SerializableExtendableEnumDictionary`). Do not create mirror base classes in SystemText namespace.

---

## Governance

- All meaningful changes require team consensus
- Document architectural decisions here
- Keep history focused on work, decisions focused on direction

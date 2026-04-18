# Charlie — History

## Project Context

**Project:** ExtendableEnums — a .NET Standard multi-package library providing base classes for creating enumerations that can be extended with additional class members.

**Stack:** C# / .NET Standard, xUnit, NuGet multi-pack solution

**Key docs:**
- `README.md` — primary documentation with usage examples for all integrations
- `CHANGELOG.md` — version history and roadmap notes

**Owner:** Kyle Herzog

**Team:** Buddy (Lead), Max (.NET Dev), Bella (Tester), Charlie (Docs & DevRel), Scribe (Logger), Ralph (Monitor)

## Learnings

### v10 to v11 Namespace Reorganization (2025)
- Added "## Upgrading from v9 to v10" section to README after Features, before implementation details
- Three migration paths presented clearly: (1) Use Newtonsoft package classes, (2) Keep core + global resolver, (3) Core only
- Before/after code blocks show realistic examples (OrderStatus class)
- Placed early in README (line 18) for maximum discoverability — developers hit breaking changes before implementation examples
- Key: Made Option 1 the "recommended" path to encourage adoption of the new Newtonsoft package structure
- Current README mentions Newtonsoft support in "### Serialization" section (line 57–63)
- Also referenced in OData example (line 155: `JsonConvert.DeserializeObject<>`)
- CHANGELOG needs breaking change callout with link to migration guide
- Newtonsoft package should have minimal, focused README (avoid duplication)
- Key insight: Developers need 2-step solution (NuGet + DI registration). Make it obvious.

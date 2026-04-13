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

### v11 Newtonsoft Split (2025)
- Current README mentions Newtonsoft support in "### Serialization" section (line 57–63)
- Also referenced in OData example (line 155: `JsonConvert.DeserializeObject<>`)
- Migration guide must be early in README for discoverability (suggest after Features section)
- CHANGELOG needs breaking change callout with link to migration guide
- Newtonsoft package should have minimal, focused README (avoid duplication)
- Key insight: Developers need 2-step solution (NuGet + DI registration). Make it obvious.

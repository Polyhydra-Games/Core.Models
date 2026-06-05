---
title: Strongly Typed Identifiers Implementation Notes
status: in-progress
owner: @DreadBreadcrumb
created: 2026-05-04
updated: 2026-06-03
tags: [feature, implementation, identifiers, Core.Models]
---

# Strongly Typed Identifiers Implementation Notes

## Implemented

- [x] Added `IStronglyTypedId<TValue>`.
- [x] Added `StronglyTypedId` creation and parse/try-parse helpers.
- [x] Added GUID-backed JSON converter.
- [x] Added string-backed JSON converter.
- [x] Added int-backed JSON converter.
- [x] Added long-backed JSON converter.
- [x] Added JSON converter factory for global registration.
- [x] Added focused tests with sample ID structs.

## Test Coverage

- [x] GUID-backed JSON round trips.
- [x] GUID-backed invalid input.
- [x] GUID-backed helper parse/try-parse.
- [x] Cross-type safety for two GUID-backed IDs with the same primitive value.
- [x] String-backed whitespace normalization.
- [x] String-backed JSON round trips.
- [x] String-backed helper creation.
- [x] Int-backed JSON number round trips.
- [x] Int-backed integer-string reads.
- [x] Int-backed invalid input.
- [x] Int-backed helper parse/try-parse.
- [x] Long-backed JSON number round trips.
- [x] Long-backed helper parse/try-parse.
- [x] Converter factory registration for attribute-free ID structs.
- [x] Unsupported backing type rejection.

## Validation Commands

```bash
dotnet test Tests/PolyhydraGames.Core.Models.Test.csproj --no-restore --configuration Release
dotnet build PolyhydraGames.Core.Models.sln --no-restore --configuration Release
```

## Migration Notes

- [x] `IdentityId.New()` in the source example should return `IdentityId`, not `UserId`.
- [x] `IdentityId` error messages should say `IdentityId`, not `UserId`.
- [x] Existing `UserId`, `WebKey`, `TwitchId`, and `IdentityId` should migrate one at a time with downstream tests.
- [ ] Dapper or EF type handlers should stay downstream unless at least two repos need the same helper shape.
- [x] Avoid changing JSON payload shape during migration.

## Concrete ID Boundary

- [x] Current `Core.Models` migration scope is the reusable identifier base/converter surface only.
- [x] Concrete IDs remain in downstream packages in the first pass.
- [x] A concrete ID can move into `Core.Models` only after the owning domain is shared across repos, at least one downstream consumer is named, JSON compatibility is covered by tests, and release notes call out the public contract move.
- [x] Downstream migrations should add the shared interface/converter pattern around the existing ID first, then remove duplicated local helpers only after all callers reference the package version that contains the shared pattern.

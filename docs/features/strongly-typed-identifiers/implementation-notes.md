---
title: Strongly Typed Identifiers Implementation Notes
status: in-progress
owner: @DreadBreadcrumb
created: 2026-05-04
updated: 2026-05-04
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

## Migration Notes

- [ ] `IdentityId.New()` in the source example should return `IdentityId`, not `UserId`.
- [ ] `IdentityId` error messages should say `IdentityId`, not `UserId`.
- [ ] Existing `UserId`, `WebKey`, `TwitchId`, and `IdentityId` should migrate one at a time with downstream tests.
- [ ] Dapper or EF type handlers should stay downstream unless at least two repos need the same helper shape.
- [ ] Avoid changing JSON payload shape during migration.

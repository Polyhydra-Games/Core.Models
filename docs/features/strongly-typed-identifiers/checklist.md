---
title: Strongly Typed Identifiers Checklist
status: in-progress
owner: @DreadBreadcrumb
created: 2026-05-04
updated: 2026-05-04
tags: [feature, checklist, identifiers, Core.Models]
---

# Strongly Typed Identifiers Checklist

## Discovery

- [x] Identify `OAuth.Core` as a source of the reusable pattern.
- [x] Identify GUID-backed examples: `UserId`, `WebKey`, proposed `IdentityId`.
- [x] Identify string-backed examples: `TwitchId` and platform IDs.
- [ ] Identify any int-backed examples before downstream migration.
- [x] Inventory current JSON converter implementations.

## Documentation

- [x] Create feature folder.
- [x] Add roadmap phase for strongly typed identifiers.
- [x] Add architecture notes.
- [x] Add migration notes for `OAuth.Core` and streaming-facing packages.

## Contracts

- [x] Define shared ID interface or marker.
- [x] Define GUID/string/optional-string convenience interfaces.
- [x] Define optional string ID normalization helpers.
- [x] Define GUID-backed pattern.
- [x] Define string-backed pattern.
- [x] Define optional string-backed pattern.
- [x] Define int-backed pattern.
- [x] Define long-backed pattern.
- [x] Define JSON converter strategy.
- [x] Define parse/try-parse strategy.

## Testing

- [x] GUID-backed equality and invalid input tests.
- [x] String-backed normalization and equality tests.
- [x] Optional string-backed null/whitespace JSON tests.
- [x] Int-backed invalid value tests.
- [x] Long-backed round-trip and invalid value tests.
- [x] JSON serialization tests.
- [x] JSON deserialization tests.
- [x] Cross-type safety tests so two different ID types cannot compare equal accidentally.

## Migration

- [x] Document first consumer in `OAuth.Core`.
- [x] Document first consumer in streaming-facing contracts.
- [ ] Decide whether concrete shared IDs move now or only base patterns move first.
- [ ] Update package version/release notes before publishing.

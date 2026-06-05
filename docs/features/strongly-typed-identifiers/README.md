---
title: Strongly Typed Identifiers
status: in-progress
owner: @DreadBreadcrumb
priority: high
complexity: 3
created: 2026-05-04
updated: 2026-06-03
tags: [feature, Core.Models, identifiers, value-objects, shared-contracts]
---

# Strongly Typed Identifiers

## Hermes Status

Hermes Kanban: pending promotion — canonical family anchor; checklist and sibling docs point here.

## Summary

Create reusable strongly typed identifier contracts and base patterns in `Core.Models` so downstream repos can stop re-creating one-off `UserId`, `IdentityId`, `WebKey`, `TwitchId`, and platform-backed ID wrappers.

This feature should export the reusable pattern, converters, and tests. App-specific identifier names may still live in downstream packages when their domain ownership is not shared.

## Migration Boundary Decision

The current migration boundary is **base/converter patterns only**. `Core.Models` owns `IStronglyTypedId<TValue>`, the GUID/string/optional-string/int/long helper contracts, parse/create helpers, and JSON converter/factory patterns. It does not move concrete downstream IDs in this slice.

Concrete IDs such as `IdentityId`, `UserId`, `WebKey`, `TwitchId`, and `ChannelKey` stay in their owning downstream packages until each type has clear cross-repo ownership, a named first consumer, compatibility tests, and release notes. Downstream repos should adopt the shared pattern one ID at a time without changing JSON payload shape or deleting existing implementations before consumers have switched references.

## Current Source Signals

- [x] `OAuth.Core` uses `IStrongId<TSelf>` as a lightweight ID interface.
- [x] `OAuth.Core` and streaming-facing contracts use strongly typed IDs such as `UserId`, `WebKey`, `TwitchId`, and `ChannelKey`.
- [x] `PolyhydraGames.Streaming.Interfaces` already has GUID-backed and string-backed ID examples.
- [x] `Core.Models` now owns reusable ID interfaces and JSON converters for strongly typed identifiers.
- [x] The current examples are split across packages, so `Core.Models` exports reusable patterns while concrete IDs remain downstream until ownership is clear.

## Candidate Contract Shape

- [x] `IStronglyTypedId<TValue>` common marker for strongly typed IDs.
- [x] `IGuidId`, `IStringId`, and `IOptionalStringId` convenience contracts.
- [x] `StringIdHelpers` optional string-ID normalization helpers.
- [x] GUID-backed ID converter pattern.
- [x] String-backed ID converter pattern.
- [x] Optional string-backed ID converter pattern for JSON null/whitespace as empty.
- [x] Int-backed ID converter pattern.
- [x] JSON converters for each backing family.
- [x] Optional converter factory for strongly typed IDs.
- [x] Parse/try-parse helpers.
- [x] Equality, hash code, and string conversion conventions.
- [x] Empty/null handling policy per backing type.

## Backing Type Rules

GUID-backed IDs:

- [x] Decide whether `Guid.Empty` is always invalid or allowed for specific ID types like `WebKey`.
- [x] Provide `New()` creation helper pattern.
- [x] Support explicit string conversion pattern.
- [x] Support JSON string round trips.

String-backed IDs:

- [x] Normalize null, empty, and whitespace consistently when the concrete ID chooses that policy.
- [x] Decide whether empty string means "no ID" or invalid per concrete ID.
- [x] Keep strict string IDs separate from optional string IDs so null handling is explicit.
- [x] Preserve platform IDs without accidental numeric conversion.
- [x] Support JSON string round trips.

Int-backed IDs:

- [x] Decide whether zero and negative values are invalid per concrete ID.
- [x] Support explicit string conversion pattern.
- [x] Support JSON number and integer-string round trips.
- [x] Avoid implicit conversion paths that allow cross-type equality by accident.

Long-backed IDs:

- [x] Support large snowflake-style identifiers.
- [x] Support JSON number and integer-string round trips.
- [x] Decide zero/negative validity per concrete ID.

## Migration Notes

- [x] Fix copied-template mistakes before implementation. The proposed `IdentityId.New()` example currently returns `UserId`; it should return `IdentityId`.
- [x] Fix copied error messages before implementation. `IdentityId` errors should not say `UserId`.
- [x] Preserve compatibility for existing downstream IDs until callers migrate.
- [x] Prefer adding shared base/converter patterns first, then move concrete IDs only when ownership is clear.
- [x] Preserve the streaming-facing optional string ID policy without forcing it onto all string-backed IDs.

## Non-Goals

- [x] Do not move OAuth token flow, PKCE, auth config, token encryption, or service abstractions into `Core.Models`.
- [x] Do not move streaming platform API clients into `Core.Models`.
- [x] Do not force all existing IDs to move in the first pass.

## Success Criteria

- [x] Strongly typed ID contracts compile under the same target frameworks as `Core.Models`.
- [x] Tests cover GUID-backed, string-backed, int-backed, and long-backed IDs.
- [x] Tests cover optional string-backed IDs, JSON null handling, and factory selection.
- [x] Tests cover equality, invalid input, explicit conversion, JSON serialization, JSON deserialization, and cross-type safety.
- [x] Downstream migration notes name `OAuth.Core` and streaming-facing ID types as first consumers.

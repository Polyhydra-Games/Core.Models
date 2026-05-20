---
title: External Provider References
status: active
owner: @DreadBreadcrumb
created: 2026-05-20
updated: 2026-05-20
tags: [feature, Core.Models, media, providers, contracts]
---

# External Provider References

## Summary

`MediaProviderReference` captures a portable reference to a record in an external media provider such as TMDb, TVMaze, OMDb, or TheTVDB.
It keeps provider API clients out of `Core.Models` while still giving downstream repos a shared way to store and exchange provider identifiers.

## Current Shape

- `ProviderKey` stores the stable provider name or namespace.
- `ExternalId` stores the provider-specific identifier.
- `DisplayName` stores an optional human-readable label.
- `Url` stores an optional canonical provider URL.
- `LastSeenAt` stores the last refresh or observation timestamp when known.
- `Metadata` provides a narrow escape hatch for extra provider hints.

## Boundary

- `Core.Models` owns the normalized reference shape.
- `Media-Metadata` owns provider API clients, search, lookup rules, and network behavior.
- Downstream repos should map provider responses into `MediaProviderReference` rather than taking a dependency on provider SDK response models.

## Examples

- `tmdb:12345` for a movie entry
- `tvmaze:67890` for a series entry
- `omdb:tt0051730` for a title lookup
- `thetvdb:98765` for a show or episode reference

## Validation

- Serialization coverage exists in the `Core.Models` test project.
- Multiple providers can be attached to a single `MediaEnrichmentMetadata` instance.

## Next Steps

- [x] Add downstream mapping examples in a consumer repo.
- [ ] Decide whether provider references need a role or relation field before further expansion.

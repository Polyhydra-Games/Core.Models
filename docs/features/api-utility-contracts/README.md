---
title: API Utility Contracts
status: active
owner: @DreadBreadcrumb
created: 2026-05-20
updated: 2026-06-03
tags: [feature, Core.Models, api, utility, contracts]
---

# API Utility Contracts

## Hermes Status

Hermes Kanban: pending promotion — canonical family anchor; checklist and sibling docs point here.

## Summary

`Core.Models.Api` provides small, serializable utility contracts that downstream HTTP APIs can share when they repeat the same result, readiness, paging, sorting, or filtering shapes.
The family is intentionally small and DTO-only so it can be adopted selectively.

## Current Shape

- `ApiOperationResult` and `ApiOperationResult<T>` represent portable success/failure envelopes.
- `ApiValidationIssue` captures validation or domain errors.
- `ApiReadinessSummary` and `ApiReadinessCheck` capture operator-facing readiness data.
- `PagedRequest` and `PagedResult<T>` capture simple page-based query and response shapes.
- `SortDirection`, `SortRule`, `FilterOperator`, and `FilterRule` capture the basic query-shaping primitives.

## Boundary

- `Core.Models` owns the serializable utility shapes only.
- Downstream repos own request handling, database filtering, UI state, and service orchestration.
- Consumers should only adopt these contracts where duplication is already real.

## Validation

- Serialization coverage exists in the `Core.Models` test project.
- The first adopters can now replace duplicated result/paging/readiness DTOs without taking on framework abstractions.

## Next Steps

- [x] Add a downstream mapping example in an API-facing consumer repo.
- [ ] Decide whether additional query operators are needed before broad adoption.

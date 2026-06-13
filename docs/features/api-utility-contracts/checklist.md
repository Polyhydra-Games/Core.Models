---
title: API Utility Contracts Checklist
status: active
owner: @DreadBreadcrumb
created: 2026-05-20
updated: 2026-05-20
tags: [feature, checklist, Core.Models, api, utility]
---

# API Utility Contracts Checklist

## Discovery

- [x] Identify repeated API utility shapes in downstream repos.
- [x] Keep request handling and filtering implementation downstream.

## Documentation

- [x] Create the feature folder.
- [x] Document the boundary and allowed roles.
- [x] Document the current utility types and intended usage.

## Models

- [x] Add `ApiOperationResult`.
- [x] Add `ApiOperationResult<T>`.
- [x] Add `ApiValidationIssue`.
- [x] Add `ApiReadinessSummary`.
- [x] Add `ApiReadinessCheck`.
- [x] Add `PagedRequest`.
- [x] Add `PagedResult<T>`.
- [x] Add `SortDirection`.
- [x] Add `SortRule`.
- [x] Add `FilterOperator`.
- [x] Add `FilterRule`.

## Testing

- [x] Add serialization round-trip coverage.
- [x] Add at least one downstream mapping example.

## Follow-Up

- [ ] Decide whether a validation result wrapper should be split out from operation result later.
- [x] Add downstream adoption examples in a real consumer repo.

---
title: Shared Model Contracts Risks
status: planned
owner: @DreadBreadcrumb
created: 2026-05-04
updated: 2026-06-03
tags: [roadmap, risks, Core.Models, shared-contracts]
---

# Shared Model Contracts Risks

## Canonical Anchor

Canonical doc: [README](./README.md). This file is supporting detail for the roadmap’s canonical Hermes anchor in README.md.

## Identifier Compatibility

- [ ] Risk: moving ID patterns changes JSON output or implicit/explicit conversion behavior in downstream repos.
- [ ] Mitigation: implement compatibility tests from the current `OAuth.Core` and streaming-facing ID behavior before migration.

## Cross-Type ID Confusion

- [ ] Risk: generic helpers accidentally make unrelated ID types comparable through their backing values.
- [ ] Mitigation: test that different ID wrapper types cannot compare equal accidentally.

## Over-Centralization

- [ ] Risk: app behavior migrates into `Core.Models` because the DTOs are nearby.
- [ ] Mitigation: keep implementation, provider clients, scanners, playback, and persistence in downstream repos.

## Premature Contract Lock-In

- [ ] Risk: names or shapes stabilize before a real downstream consumer validates them.
- [ ] Mitigation: require at least one downstream mapping before marking a family stable.

## Provider Leakage

- [ ] Risk: normalized provider references turn into provider-specific API models.
- [ ] Mitigation: keep `Media-Metadata` as the provider API boundary.

## Ambiguous Media Ownership

- [ ] Risk: `core.nas`, `SvengoolieDB`, CDN tooling, and `chop-it` all interpret the same model differently.
- [ ] Mitigation: document ownership in each feature folder and keep examples concrete.

## Serialization Compatibility

- [ ] Risk: public DTO changes break downstream persisted JSON or API clients.
- [ ] Mitigation: add serialization tests and release notes for each model family.

## Generic API Model Sprawl

- [ ] Risk: paging/result/readiness helpers become too abstract and do not fit real callers.
- [ ] Mitigation: only import these after duplication is confirmed in downstream repos.

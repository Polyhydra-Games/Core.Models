# Core.Models portfolio roadmap

## Snapshot
- 90-day evidence: 9 commits, 27 files changed
- Last signal: `b0517ff`
- Top modified areas: `docs`, `Tests`, `Models`
- Stack: .NET
- Docs folder: yes
- Roadmap folder: no
- Features docs: yes
- Tests indexed: no

## Implemented now (V1 baseline)
- Core model capabilities and API shape are documented in `coremodels.md`, `core-capabilities.md`, and `shared-model-contracts/README.md`.
- Test scaffolding is documented in `sub-module-tests.md`.
- Standardized structure and solution layout are tracked in docs.

## Gaps identified
- Tests were touched but current feature mapping still needs explicit ownership.
- Model compatibility and serialization edge cases are not fully called out in release criteria.
- Data model changes can have downstream impact without documented migration notes.

## V1 (stability)
- [ ] Link model feature changes to explicit test files and scenarios.
- [ ] Add deterministic migration/check list for model shape changes.
- [ ] Add release gate for `README`/feature updates when model behavior shifts.
- [ ] Add examples for backward-compatible model updates.
- [x] Establish shared-contract import criteria in `docs/roadmaps/shared-model-contracts/`.

## V2 (confidence)
- [ ] Expand test coverage for common model edge cases and fallbacks.
- [ ] Add compatibility matrix for consumers and serialization formats.
- [ ] Add clear operator guidance for CI and pipeline validation.
- [ ] Add changelog discipline for breaking model changes.
- [ ] Validate at least one new shared contract family in a downstream repo before marking it stable.

## V10 (scale)
- [ ] Formalize model contract policy across org-core packages.
- [ ] Add automated compatibility verification in CI.
- [ ] Build long-term owner model for model/API areas.
- [ ] Add support policy for migration and deprecation across major versions.

## Readiness checklist
- [ ] Any model change has paired docs and validation.
- [ ] Test coverage includes schema and serialization behavior.
- [ ] Release notes include compatibility impact.

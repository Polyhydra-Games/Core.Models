# Maintainer release procedure

Normal CI validates source changes only. It never publishes a package. NuGet publication is a deliberate, maintainer-triggered action on trusted `main` through the protected `nuget-release` environment.

## One-time setup

1. Create the `nuget-release` GitHub environment and restrict it to maintainers who may approve releases.
2. Add a scoped NuGet API key as that environment's `NUGET_API_KEY` secret. Configure the key only for `PolyhydraGames.Core.Models` and package push/unlist operations required by the organization policy.
3. Do not copy legacy repository secrets into source, workflow files, issue comments, shell history, or chat.

## Prepare a release

1. Update `Models/PolyhydraGames.Core.Models.csproj` with a new SemVer `Version`.
2. Update `CHANGELOG.md` with consumer-impacting changes.
3. Merge the reviewed version change to `main` after normal validation.
4. Start **Release NuGet package** from the `main` branch with `dry_run=true`.
5. Verify the validation job passed and inspect its retained `core-models-<Version>` package artifact: restore, build, tests, package payload, tag availability, and NuGet version availability must all be clean.
6. Start the same workflow from `main` with `dry_run=false`. The validated package artifact is rebuilt and uploaded before the protected publish job requests approval.

## Publish

1. Approve the protected environment only after reviewing the package version and generated release artifact.
2. The workflow pushes the `.nupkg` and `.snupkg` to nuget.org, then creates the immutable `v<Version>` tag and GitHub release.
3. Confirm the package is installable from nuget.org and that the GitHub release points to the expected commit.

## Failure and rollback

- A failed dry run publishes nothing.
- A failure before NuGet publication publishes nothing and creates no tag.
- NuGet versions are immutable. If a bad package is published, unlist that version using approved NuGet administration, publish a corrected higher version, and document the incident in the changelog/release notes.
- Never delete or retag a published source tag to conceal a bad release; issue a corrective release instead.

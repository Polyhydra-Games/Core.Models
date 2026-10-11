# Drop net8.0, add net10.0 target (#47)

## Summary

- Contract changed: supported target frameworks of the `PolyhydraGames.Core.Models` package
- Change type: breaking
- Package version: next release after 2.0.1.x (see "Versioning")

## Compatibility

- Package consumers: projects targeting `net8.0` can no longer restore the package. Known in `~/code` as of 2026-10-10: `core.ffmpeg` (`Polyhydra.Ffmpeg.Contracts`) and `Core.UDP` (`Service`). `Media-Metadata` declares no TFM in its project file (check its props).
- Project-reference consumers: none affected by this repo's tests (they run on net10.0).
- Serialization impact: none.
- Nullability or required-field impact: none.
- Enum or identifier impact: none.

## Migration Notes

- Required consumer change: retarget to `net9.0` or `net10.0`, or stay on the last 2.0.1.x build that includes `lib/net8.0`.
- Safe rollout order: retarget the net8.0 consumers first, then bump the package.
- Deprecated member removal target: n/a.

## Versioning

net8.0 (LTS) and net9.0 (STS) both reach end of support in November 2026. This is a breaking change published under the existing `2.0.1.<run>` auto-stamp, so the maintainer should decide whether to bump `VersionPrefix`/`PACKAGE_VERSION` to 3.0.0 before releasing.

## Validation

- `dotnet test Tests/PolyhydraGames.Core.Models.Test.csproj`: 113/113 passed.
- `dotnet pack` produces `lib/net9.0` and `lib/net10.0` only.

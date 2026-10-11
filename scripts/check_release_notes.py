#!/usr/bin/env python3
"""Fail when a change to the shipped Models/ project has no release note.

Usage: check_release_notes.py <changed-files-file> [--skip]
The changed-files file holds one repo-relative path per line (git diff --name-only).
--skip is passed by CI when the PR carries the `no-release-note` label.
"""

import sys
from pathlib import Path

SHIPPED_PREFIX = "Models/"
SHIPPED_SUFFIXES = (".cs", ".csproj")
NOTES_PREFIX = "docs/release-notes/"
TEMPLATE = "docs/release-notes/shared-contract-change-template.md"


def check(changed: list[str], skip: bool = False) -> str | None:
    """Return an error message, or None when the change is acceptable."""
    if skip:
        return None
    shipped = [p for p in changed if p.startswith(SHIPPED_PREFIX) and p.endswith(SHIPPED_SUFFIXES)]
    if not shipped:
        return None
    notes = [p for p in changed if p.startswith(NOTES_PREFIX) and p != TEMPLATE and p.endswith(".md")]
    if notes:
        return None
    return (
        f"{len(shipped)} shipped file(s) under {SHIPPED_PREFIX} changed without a release note. "
        f"Add one under {NOTES_PREFIX} (see {TEMPLATE}) or label the PR `no-release-note`. "
        f"First file: {shipped[0]}"
    )


def main() -> int:
    args = [a for a in sys.argv[1:] if a != "--skip"]
    skip = "--skip" in sys.argv[1:]
    changed = [line.strip() for line in Path(args[0]).read_text(encoding="utf-8").splitlines() if line.strip()]
    error = check(changed, skip)
    if error:
        print(f"::error::{error}")
        return 1
    print("Release-note check passed.")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())

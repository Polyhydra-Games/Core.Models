#!/usr/bin/env python3
"""Enforce runner isolation in the NuGet publishing workflow."""

from __future__ import annotations

import re
import sys
from pathlib import Path

PUBLISH_LABEL = "nuget-core-models-publish"
PUBLISH_JOBS = {"publish-nuget-org", "publish-mirrors"}


def job_blocks(workflow: str) -> dict[str, str]:
    jobs_match = re.search(r"^jobs:\s*$", workflow, re.MULTILINE)
    if not jobs_match:
        raise ValueError("workflow has no top-level jobs section")
    tail = workflow[jobs_match.end() :]
    matches = list(re.finditer(r"^  ([A-Za-z0-9_-]+):\s*$", tail, re.MULTILINE))
    return {
        match.group(1): tail[match.end() : matches[index + 1].start() if index + 1 < len(matches) else len(tail)]
        for index, match in enumerate(matches)
    }


def validate(workflow: str) -> list[str]:
    errors: list[str] = []
    jobs = job_blocks(workflow)
    for name in sorted(PUBLISH_JOBS):
        block = jobs.get(name)
        if block is None:
            errors.append(f"missing required publishing job: {name}")
        elif not re.search(r"^    runs-on: \[self-hosted, " + re.escape(PUBLISH_LABEL) + r"\]\s*$", block, re.MULTILINE):
            errors.append(f"{name} must use exactly [self-hosted, {PUBLISH_LABEL}]")

    for name, block in jobs.items():
        if re.search(r"^    runs-on:.*\b" + re.escape(PUBLISH_LABEL) + r"\b", block, re.MULTILINE):
            if name not in PUBLISH_JOBS:
                errors.append(f"job {name} must not use publish runner label {PUBLISH_LABEL}")

    package = jobs.get("package", "")
    if re.search(r"^    runs-on:.*\b" + re.escape(PUBLISH_LABEL) + r"\b", package, re.MULTILINE):
        errors.append(f"package must not use publish runner label {PUBLISH_LABEL}")
    return errors


def main() -> int:
    if len(sys.argv) != 2:
        print(f"Usage: {Path(sys.argv[0]).name} <workflow.yml>", file=sys.stderr)
        return 2
    errors = validate(Path(sys.argv[1]).read_text(encoding="utf-8"))
    if errors:
        for error in errors:
            print(f"ERROR: {error}")
        return 1
    print(f"Publish runner policy passed: only {', '.join(sorted(PUBLISH_JOBS))} use [{PUBLISH_LABEL}].")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())

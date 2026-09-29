#!/usr/bin/env python3
"""Enforce runner isolation in GitHub Actions workflow files."""

from __future__ import annotations

import argparse
import re
from pathlib import Path

PUBLISH_LABEL = "nuget-core-models-publish"
PUBLISH_JOBS = {"publish-nuget-org", "publish-mirrors"}
PUBLISH_WORKFLOW = "publish-nuget"


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


def runner_labels(block: str) -> list[str] | None:
    """Return literal runner labels for inline or block-list runs-on values."""
    lines = block.splitlines()
    for index, line in enumerate(lines):
        match = re.match(r"^    runs-on:\s*(.*)$", line)
        if not match:
            continue
        value = match.group(1).strip()
        if value.startswith("[") and value.endswith("]"):
            return [part.strip().strip("\"'") for part in value[1:-1].split(",")]
        if value:
            return [value.strip("\"'")]

        labels: list[str] = []
        for continuation in lines[index + 1 :]:
            if continuation.strip() and not continuation.startswith("      "):
                break
            item = re.match(r"^      -\s*(.*?)\s*$", continuation)
            if item:
                labels.append(item.group(1).strip("\"'"))
        return labels
    return None


def validate(workflow: str, *, require_publish_jobs: bool = True) -> list[str]:
    errors: list[str] = []
    jobs = job_blocks(workflow)
    if require_publish_jobs:
        for name in sorted(PUBLISH_JOBS):
            labels = runner_labels(jobs[name]) if name in jobs else None
            if labels is None:
                errors.append(f"missing required publishing job: {name}")
            elif labels != ["self-hosted", PUBLISH_LABEL]:
                errors.append(f"{name} must use exactly [self-hosted, {PUBLISH_LABEL}]")

    for name, block in jobs.items():
        labels = runner_labels(block) or []
        if any(PUBLISH_LABEL in label for label in labels) and name not in PUBLISH_JOBS:
            errors.append(f"job {name} must not use publish runner label {PUBLISH_LABEL}")
    return errors


def workflow_errors(path: Path) -> list[str]:
    try:
        errors = validate(path.read_text(encoding="utf-8"), require_publish_jobs=path.stem == PUBLISH_WORKFLOW)
    except (OSError, ValueError) as error:
        return [f"{path}: {error}"]
    return [f"{path}: {error}" for error in errors]


def workflow_files(repo_root: Path) -> list[Path]:
    directory = repo_root / ".github/workflows"
    return sorted(path for suffix in ("*.yml", "*.yaml") for path in directory.glob(suffix))


def main() -> int:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--repo-root", type=Path, default=Path(__file__).resolve().parents[1])
    args = parser.parse_args()
    workflows = workflow_files(args.repo_root)
    if not workflows:
        print(f"ERROR: no GitHub workflow files found under {args.repo_root / '.github/workflows'}")
        return 1
    errors = [error for path in workflows for error in workflow_errors(path)]
    if errors:
        for error in errors:
            print(f"ERROR: {error}")
        return 1
    print(f"Publish runner policy passed for {len(workflows)} workflow file(s).")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())

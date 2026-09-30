#!/usr/bin/env python3
"""Enforce runner isolation in GitHub Actions workflow files."""

from __future__ import annotations

import argparse
from pathlib import Path

import yaml

PUBLISH_LABEL = "nuget-core-models-publish"
PUBLISH_JOBS = {"publish-nuget-org", "publish-mirrors"}
PUBLISH_WORKFLOW_NAMES = {"publish-nuget.yml", "publish-nuget.yaml"}


class WorkflowLoader(yaml.SafeLoader):
    """Safe YAML loader that handles GitHub's YAML 1.2-style `on` key."""


WorkflowLoader.yaml_implicit_resolvers = {
    key: [(tag, regexp) for tag, regexp in resolvers if tag != "tag:yaml.org,2002:bool"]
    for key, resolvers in yaml.SafeLoader.yaml_implicit_resolvers.items()
}


def _workflow_document(workflow: str) -> dict:
    try:
        document = yaml.load(workflow, Loader=WorkflowLoader)
    except yaml.YAMLError as error:
        raise ValueError(f"workflow YAML is invalid: {error}") from error
    if not isinstance(document, dict):
        raise ValueError("workflow must be a YAML mapping")
    return document


def _labels(value: object) -> list[str]:
    if isinstance(value, str):
        return [value]
    if isinstance(value, list) and all(isinstance(item, str) for item in value):
        return value
    raise ValueError("runs-on labels must be a string or a list of strings")


def _runner_labels(value: object) -> list[str] | None:
    if isinstance(value, str) or isinstance(value, list):
        return _labels(value)
    if isinstance(value, dict):
        if set(value) - {"group", "labels"} or "labels" not in value:
            raise ValueError("runs-on mapping must contain labels and only optional group")
        group = value.get("group")
        if group is not None and not isinstance(group, str):
            raise ValueError("runs-on group must be a string")
        return _labels(value["labels"])
    raise ValueError("runs-on must be a string, list, or group/labels mapping")


def _workflow_scope(path: Path) -> bool:
    return path.parent.as_posix().endswith(".github/workflows") and path.name in PUBLISH_WORKFLOW_NAMES


def validate(workflow: str, *, require_publish_jobs: bool = True) -> list[str]:
    errors: list[str] = []
    try:
        document = _workflow_document(workflow)
        jobs = document.get("jobs")
        if not isinstance(jobs, dict) or not all(isinstance(name, str) and isinstance(job, dict) for name, job in jobs.items()):
            raise ValueError("top-level jobs must be a mapping of job IDs to mappings")
    except ValueError as error:
        return [str(error)]

    if require_publish_jobs:
        for name in sorted(PUBLISH_JOBS):
            job = jobs.get(name)
            if job is None:
                errors.append(f"missing required publishing job: {name}")
                continue
            try:
                labels = _runner_labels(job.get("runs-on"))
            except ValueError as error:
                errors.append(f"{name} has invalid runs-on: {error}")
                continue
            if labels != ["self-hosted", PUBLISH_LABEL]:
                errors.append(f"{name} must use exactly [self-hosted, {PUBLISH_LABEL}]")

    for name, job in jobs.items():
        if "runs-on" not in job:
            errors.append(f"job {name} has no runs-on value")
            continue
        try:
            runs_on = job["runs-on"]
            labels = _runner_labels(runs_on)
        except ValueError as error:
            errors.append(f"job {name} has invalid runs-on: {error}")
            continue
        if isinstance(runs_on, str) and ("${{" in runs_on or "}}" in runs_on):
            errors.append(f"job {name} has dynamic runs-on value that cannot be verified")
            continue
        if isinstance(runs_on, list) and any(not isinstance(item, str) or "${{" in item or "}}" in item for item in runs_on):
            errors.append(f"job {name} has dynamic runs-on value that cannot be verified")
            continue
        if isinstance(runs_on, dict):
            if any(not isinstance(item, str) or "${{" in item or "}}" in item for item in labels):
                errors.append(f"job {name} has dynamic runs-on value that cannot be verified")
                continue
        if any(PUBLISH_LABEL in label for label in labels):
            if not (require_publish_jobs and name in PUBLISH_JOBS and labels == ["self-hosted", PUBLISH_LABEL]):
                errors.append(f"job {name} must not use publish runner label {PUBLISH_LABEL}")
    return errors


def workflow_errors(path: Path) -> list[str]:
    try:
        text = path.read_text(encoding="utf-8")
        scoped_publisher = _workflow_scope(path)
        errors = validate(text, require_publish_jobs=scoped_publisher)
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

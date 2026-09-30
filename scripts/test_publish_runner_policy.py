import unittest
from pathlib import Path
import sys
from tempfile import TemporaryDirectory

sys.path.insert(0, str(Path(__file__).parent))
from check_publish_runner_policy import validate, workflow_errors, workflow_files


WORKFLOW = Path(__file__).parents[1] / ".github/workflows/publish-nuget.yml"


class PublishRunnerPolicyTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.workflow = WORKFLOW.read_text(encoding="utf-8")

    def test_repository_workflow_obeys_policy(self):
        self.assertEqual([], validate(self.workflow))

    def test_package_cannot_use_publish_runner(self):
        old = "runs-on: [self-hosted, Linux, X64, core-models-ci]"
        new = "runs-on: [self-hosted, nuget-core-models-publish]"
        fixture = self.workflow.replace(old, new, 1)
        self.assertNotEqual(self.workflow, fixture, "fixture replacement did not change the text")
        errors = validate(fixture)
        self.assertTrue(any("package must not use publish runner" in error for error in errors), errors)

    def test_publishing_job_requires_exact_dedicated_runner(self):
        old = "runs-on: [self-hosted, nuget-core-models-publish]"
        new = "runs-on: [self-hosted, Linux, X64, core-models-ci]"
        fixture = self.workflow.replace(old, new, 1)
        self.assertNotEqual(self.workflow, fixture, "fixture replacement did not change the text")
        errors = validate(fixture)
        self.assertTrue(any("publish-nuget-org must use exactly" in error for error in errors), errors)

    def test_no_other_job_can_use_publish_runner(self):
        fixture = self.workflow.replace(
            "  package:\n", "  unexpected-job:\n    runs-on: [self-hosted, nuget-core-models-publish]\n\n  package:\n", 1
        )
        self.assertNotEqual(self.workflow, fixture, "fixture replacement did not change the text")
        errors = validate(fixture)
        self.assertTrue(any("unexpected-job must not use publish runner" in error for error in errors), errors)

    def test_block_list_runner_cannot_use_publish_label(self):
        fixture = self.workflow.replace(
            "    runs-on: [self-hosted, Linux, X64, core-models-ci]",
            "    runs-on:\n      - self-hosted\n      - nuget-core-models-publish",
            1,
        )
        self.assertNotEqual(self.workflow, fixture, "fixture replacement did not change the text")
        errors = validate(fixture)
        self.assertTrue(any("package must not use publish runner" in error for error in errors), errors)

    def test_publishing_job_accepts_exact_block_list(self):
        fixture = self.workflow.replace(
            "runs-on: [self-hosted, nuget-core-models-publish]",
            "runs-on:\n      - self-hosted\n      - nuget-core-models-publish",
            1,
        )
        self.assertNotEqual(self.workflow, fixture, "fixture replacement did not change the text")
        self.assertEqual([], validate(fixture))

    def test_discovers_both_workflow_extensions_from_repo_root(self):
        with TemporaryDirectory() as temporary:
            directory = Path(temporary) / ".github/workflows"
            directory.mkdir(parents=True)
            (directory / "first.yml").write_text("jobs: {}\n", encoding="utf-8")
            (directory / "second.yaml").write_text("jobs: {}\n", encoding="utf-8")
            self.assertEqual(
                [directory / "first.yml", directory / "second.yaml"],
                workflow_files(Path(temporary)),
            )

    def test_another_workflow_cannot_exempt_publisher_named_jobs(self):
        with TemporaryDirectory() as temporary:
            path = Path(temporary) / ".github/workflows/other.yaml"
            path.parent.mkdir(parents=True)
            path.write_text("jobs:\n  publish-nuget-org:\n    runs-on: [self-hosted, nuget-core-models-publish]\n    steps: []\n", encoding="utf-8")
            errors = workflow_errors(path)
            self.assertTrue(any("publish-nuget-org must not use publish runner" in error for error in errors), errors)

    def test_mapping_form_runner_labels_are_checked(self):
        fixture = self.workflow.replace(
            "runs-on: [self-hosted, Linux, X64, core-models-ci]",
            "runs-on:\n      group: publishers\n      labels: nuget-core-models-publish",
            1,
        )
        errors = validate(fixture)
        self.assertTrue(any("package must not use publish runner" in error for error in errors), errors)

    def test_mapping_form_publishing_runner_requires_exact_labels(self):
        fixture = self.workflow.replace(
            "runs-on: [self-hosted, nuget-core-models-publish]",
            "runs-on:\n      group: publishers\n      labels: [self-hosted, nuget-core-models-publish, extra]",
            1,
        )
        errors = validate(fixture)
        self.assertTrue(any("publish-nuget-org must use exactly" in error for error in errors), errors)

    def test_expression_based_runs_on_fails_closed(self):
        fixture = self.workflow.replace(
            "runs-on: [self-hosted, Linux, X64, core-models-ci]",
            "runs-on: ${{ matrix.runner }}",
            1,
        )
        errors = validate(fixture)
        self.assertTrue(any("dynamic runs-on value" in error for error in errors), errors)

    def test_flow_style_trigger_mapping_parses_without_losing_jobs(self):
        fixture = self.workflow.replace(
            "on:\n  push:\n    branches:\n      - main\n  pull_request:\n    branches:\n      - main\n  workflow_dispatch:",
            "on: {push: {branches: [main]}, pull_request: {branches: [main]}, workflow_dispatch: null}",
            1,
        )
        self.assertEqual([], validate(fixture))

    def test_unrecognized_runs_on_structure_fails_closed(self):
        fixture = self.workflow.replace(
            "runs-on: [self-hosted, Linux, X64, core-models-ci]",
            "runs-on:\n      group: publishers",
            1,
        )
        errors = validate(fixture)
        self.assertTrue(any("invalid runs-on" in error for error in errors), errors)


if __name__ == "__main__":
    unittest.main()

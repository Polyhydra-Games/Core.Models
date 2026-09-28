import unittest
from pathlib import Path
import sys

sys.path.insert(0, str(Path(__file__).parent))
from check_publish_runner_policy import validate


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


if __name__ == "__main__":
    unittest.main()

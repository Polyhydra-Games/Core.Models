import sys
import unittest
from pathlib import Path

sys.path.insert(0, str(Path(__file__).parent))
from check_release_notes import check  # noqa: E402


class CheckReleaseNotesTests(unittest.TestCase):
    def test_tests_only_change_needs_no_note(self):
        self.assertIsNone(check(["Tests/FooTests.cs"]))

    def test_shipped_change_without_note_fails(self):
        self.assertIn("without a release note", check(["Models/Foo.cs"]))

    def test_shipped_csproj_change_without_note_fails(self):
        self.assertIsNotNone(check(["Models/PolyhydraGames.Core.Models.csproj"]))

    def test_shipped_change_with_note_passes(self):
        self.assertIsNone(check(["Models/Foo.cs", "docs/release-notes/2026-10-10-foo.md"]))

    def test_editing_only_the_template_does_not_count(self):
        self.assertIsNotNone(check(["Models/Foo.cs", "docs/release-notes/shared-contract-change-template.md"]))

    def test_skip_label_bypasses(self):
        self.assertIsNone(check(["Models/Foo.cs"], skip=True))


if __name__ == "__main__":
    unittest.main()

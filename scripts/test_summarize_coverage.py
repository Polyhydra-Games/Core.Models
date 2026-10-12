import subprocess
import tempfile
import unittest
from pathlib import Path


SCRIPT = Path(__file__).with_name("summarize-coverage.py")


class SummarizeCoverageTests(unittest.TestCase):
    def test_package_counts_use_only_its_classes(self):
        # Coverlet repeats every line under class/methods/method/lines; only class/lines is counted.
        report = """<?xml version="1.0"?>
<coverage lines-valid="100" branches-valid="20">
  <packages>
    <package name="Assembly.One" line-rate="0.5" branch-rate="0.5">
      <classes><class>
        <methods><method><lines>
          <line number="1" hits="1" condition-coverage="50% (1/2)"/>
          <line number="2" hits="0"/>
        </lines></method></methods>
        <lines>
          <line number="1" hits="1" condition-coverage="50% (1/2)"/>
          <line number="2" hits="0"/>
        </lines>
      </class></classes>
    </package>
    <package name="Assembly.Two" line-rate="1" branch-rate="1">
      <classes><class><lines>
        <line number="1" hits="3" condition-coverage="100% (3/3)"/>
      </lines></class></classes>
    </package>
  </packages>
</coverage>
"""
        with tempfile.TemporaryDirectory() as directory:
            path = Path(directory) / "coverage.cobertura.xml"
            path.write_text(report, encoding="utf-8")
            result = subprocess.run(
                ["python3", str(SCRIPT), directory],
                check=True,
                capture_output=True,
                text=True,
            )

        self.assertIn("Assembly.One | 50.00% (1/2) | 50.00% (1/2)", result.stdout)
        self.assertIn("Assembly.Two | 100.00% (1/1) | 100.00% (3/3)", result.stdout)

    def _run(self, *extra):
        report = """<?xml version="1.0"?>
<coverage><packages><package name="Assembly.One"><classes><class><lines>
  <line number="1" hits="1"/><line number="2" hits="0"/>
</lines></class></classes></package></packages></coverage>
"""
        with tempfile.TemporaryDirectory() as directory:
            (Path(directory) / "coverage.cobertura.xml").write_text(report, encoding="utf-8")
            return subprocess.run(
                ["python3", str(SCRIPT), directory, *extra],
                capture_output=True,
                text=True,
            )

    def test_min_line_floor_fails_when_coverage_is_below_it(self):
        result = self._run("--min-line", "60")  # fixture is 50%
        self.assertEqual(result.returncode, 1)
        self.assertIn("below the 60.00% floor", result.stdout)

    def test_min_line_floor_passes_when_coverage_meets_it(self):
        self.assertEqual(self._run("--min-line", "50").returncode, 0)

    def test_without_min_line_never_fails(self):
        self.assertEqual(self._run().returncode, 0)


if __name__ == "__main__":
    unittest.main()

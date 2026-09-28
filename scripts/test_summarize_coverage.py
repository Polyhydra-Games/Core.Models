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


if __name__ == "__main__":
    unittest.main()

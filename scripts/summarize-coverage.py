#!/usr/bin/env python3
"""Write production assembly Cobertura line and branch coverage to the CI summary."""

from pathlib import Path
import os
import sys
import xml.etree.ElementTree as ET


root = Path(sys.argv[1] if len(sys.argv) > 1 else "TestResults")
reports = sorted(root.glob("**/coverage.cobertura.xml"))
if not reports:
    print(f"No Cobertura report found under {root}")
    raise SystemExit(0)

totals: dict[str, list[int]] = {}
for report in reports:
    document = ET.parse(report).getroot()
    for package in document.findall(".//package"):
        name = package.get("name", "unknown")
        values = totals.setdefault(name, [0, 0, 0, 0])
        # Cobertura stores package totals as coverage rates, not hit counts.
        values[0] += round(float(package.get("line-rate", "0")) * int(document.get("lines-valid", "0")))
        values[1] += int(document.get("lines-valid", "0"))
        values[2] += round(float(package.get("branch-rate", "0")) * int(document.get("branches-valid", "0")))
        values[3] += int(document.get("branches-valid", "0"))

lines = ["### Production assembly coverage", "", "| Assembly | Line | Branch |", "| --- | ---: | ---: |"]
for assembly, (line_hit, line_total, branch_hit, branch_total) in sorted(totals.items()):
    line_pct = 100 * line_hit / line_total if line_total else 100.0
    branch_pct = 100 * branch_hit / branch_total if branch_total else 100.0
    lines.append(f"| {assembly} | {line_pct:.2f}% ({line_hit}/{line_total}) | {branch_pct:.2f}% ({branch_hit}/{branch_total}) |")

summary = "\n".join(lines) + "\n"
print(summary, end="")
summary_file = os.environ.get("GITHUB_STEP_SUMMARY")
if summary_file:
    summary_path = Path(summary_file)
    with summary_path.open("a", encoding="utf-8") as stream:
        stream.write(summary)

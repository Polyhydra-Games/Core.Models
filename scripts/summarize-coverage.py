#!/usr/bin/env python3
"""Write production assembly Cobertura line and branch coverage to the CI summary."""

from pathlib import Path
import os
import re
import sys
import xml.etree.ElementTree as ET


args = sys.argv[1:]
min_line = None
if "--min-line" in args:
    index = args.index("--min-line")
    min_line = float(args[index + 1])
    del args[index:index + 2]

root = Path(args[0] if args else "TestResults")
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
        for line in package.findall("./classes/class/lines/line"):
            values[1] += 1
            values[0] += int(line.get("hits", "0")) > 0
            coverage = line.get("condition-coverage", "")
            match = re.search(r"\((\d+)/(\d+)\)", coverage)
            if match:
                values[2] += int(match.group(1))
                values[3] += int(match.group(2))

failures = []
lines = ["### Production assembly coverage", "", "| Assembly | Line | Branch |", "| --- | ---: | ---: |"]
for assembly, (line_hit, line_total, branch_hit, branch_total) in sorted(totals.items()):
    line_pct = 100 * line_hit / line_total if line_total else 100.0
    branch_pct = 100 * branch_hit / branch_total if branch_total else 100.0
    if min_line is not None and line_pct < min_line:
        failures.append(f"{assembly} line coverage {line_pct:.2f}% is below the {min_line:.2f}% floor")
    lines.append(f"| {assembly} | {line_pct:.2f}% ({line_hit}/{line_total}) | {branch_pct:.2f}% ({branch_hit}/{branch_total}) |")

summary = "\n".join(lines) + "\n"
print(summary, end="")
summary_file = os.environ.get("GITHUB_STEP_SUMMARY")
if summary_file:
    summary_path = Path(summary_file)
    with summary_path.open("a", encoding="utf-8") as stream:
        stream.write(summary)

if failures:
    for failure in failures:
        print(f"::error::{failure}")
    raise SystemExit(1)

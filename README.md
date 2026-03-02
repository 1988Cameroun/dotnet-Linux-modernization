## Phase 1: Windows Assumptions Exposed

Running a .NET app with Windows-specific assumptions 
on a Linux environment reveals immediate failures:

- `USERPROFILE` environment variable returns null — 
  Linux uses `HOME` instead
- Hardcoded `C:\Logs\` paths are structurally invalid on Linux
- CRLF line endings behave differently across Linux tooling
- OS detection confirms Ubuntu 24.04 — not the assumed Windows runtime

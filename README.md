## Phase 1: Windows Assumptions Exposed

Running a .NET app with Windows-specific assumptions 
on a Linux environment reveals immediate failures:

- `USERPROFILE` environment variable returns null — 
  Linux uses `HOME` instead
- Hardcoded `C:\Logs\` paths are structurally invalid on Linux
- CRLF line endings behave differently across Linux tooling
- OS detection confirms Ubuntu 24.04 — not the assumed Windows runtime

## Phase 2: Containerization Does Not Fix Environment Assumptions

The Docker build succeeded without errors — the app compiled cleanly 
using a Linux .NET base image. But running the container revealed the 
same failures as Phase 1:

- `USERPROFILE` still returns null inside the container
- `C:\Logs\` path still hardcoded — would throw on any actual write attempt
- OS confirms Ubuntu 24.04 — the container is Linux but the app 
  doesn't know how to behave on Linux

**Key observation:** Containerization relocates Windows assumptions, 
it does not resolve them. The container boundary is not an abstraction 
layer for application behavior — only for infrastructure deployment.
This is the silent risk in lift-and-shift migrations.
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

## Phase 3: Resolving Environment Assumptions Architecturally

Fixes were not cosmetic — each one represents a deliberate 
architectural decision:

- `Path.Combine` with `SpecialFolder.UserProfile` replaces hardcoded 
  `C:\` paths — resolves correctly on any OS at runtime
- `Environment.NewLine` replaces hardcoded CRLF — delegates 
  line ending behavior to the host environment
- `SpecialFolder.UserProfile` replaces `USERPROFILE` env variable — 
  uses .NET's cross-platform abstraction layer instead of 
  OS-specific variables

**Key observation:** The fix is not "make it work on Linux." 
The fix is "stop assuming an operating system." These are 
different problems with different solutions.

## Phase 4: Fixed App Running in Container

Rebuilding the container with the corrected code confirms 
the architectural fixes hold across environment boundaries:

- `logPath` resolves to `/root/logs/app.log` — valid Linux path 
  built dynamically for the container's root user
- `lineEnding` confirms Unix LF inside the container
- `userHome` is `/root` — different from the Codespace environment 
  (`/home/codespace`) but resolved correctly in both cases

**Key observation:** The same code produced different paths in the 
Codespace vs the container — and both were correct. This is the 
difference between hardcoding environment assumptions and delegating 
resolution to the runtime. The fix works across environments 
without modification.

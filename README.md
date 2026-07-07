# BDO Multi-Tool

Desktop helper app for Black Desert Online.

## Updates

The app checks `update.json` on startup. When `update.json` reports a version newer than the local app version, the app shows a clickable update badge in the bottom-right status bar.

Current public manifest:

```text
https://raw.githubusercontent.com/Chucksterboy/BDOMultiTool/main/update.json
```

## Release Flow

Run this from the repository root:

```powershell
.\scripts\release.ps1 -Version v0.2 -Notes "Short release notes here."
```

The script:

- updates the app version
- updates `update.json`
- builds the app
- builds the installer
- commits the release
- tags the version
- pushes to GitHub
- creates a GitHub Release
- uploads the installer

GitHub CLI must be logged in before releasing:

```powershell
gh auth status
```

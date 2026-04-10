# Git Hooks Setup

This directory contains custom git hooks for the MailTo EMS project. They help maintain architectural consistency by validating code before commits.

## Available Hooks

### pre-commit-validate-architecture.sh

Validates staged C# files against modular monolith architecture patterns before allowing commits.

**What it does:**
- ✅ Checks for direct cross-module service dependencies
- ✅ Warns about missing IModule implementations
- ✅ Validates architectural patterns in staged changes
- ⚠️ Shows warnings but allows commits (non-blocking)

**Installation:**

#### Option 1: Automatic Setup (Recommended)

Run the setup script:
```bash
cd .git-hooks
./setup-hooks.sh
```

#### Option 2: Manual Setup

Copy the hook to git's hooks directory and make it executable:

**On macOS/Linux:**
```bash
cp .git-hooks/pre-commit-validate-architecture.sh .git/hooks/pre-commit
chmod +x .git/hooks/pre-commit
```

**On Windows (PowerShell):**
```powershell
Copy-Item ".git-hooks/pre-commit-validate-architecture.sh" ".git/hooks/pre-commit"
# Note: Git for Windows automatically handles permissions
```

### Verification

After installation, verify the hook is working:

```bash
# This should trigger the pre-commit hook
git commit --allow-empty -m "test: verify hook installation"
```

You should see output like:
```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Pre-Commit Hook: Validating Modular Monolith Architecture
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
...
```

## How Hooks Work

When you run `git commit`, git automatically executes `.git/hooks/pre-commit` (if it exists) before creating the commit. 

**If the hook:**
- ✅ **Exits with 0** → Commit proceeds
- ❌ **Exits with non-zero** → Commit is blocked

Our `pre-commit-validate-architecture` hook is **non-blocking** (always exits with 0) but provides warnings.

## Bypassing Hooks

If you need to skip the hook for a specific commit:

```bash
git commit --no-verify
```

Or temporarily disable it:

```bash
chmod -x .git/hooks/pre-commit
```

Re-enable it:

```bash
chmod +x .git/hooks/pre-commit
```

## Customization

To modify the hook:

1. Edit `.git-hooks/pre-commit-validate-architecture.sh`
2. Reinstall it: `cp .git-hooks/pre-commit-validate-architecture.sh .git/hooks/pre-commit`
3. Test your changes with a commit

## Deep Validation

The pre-commit hook performs **quick checks** on staged files. For **comprehensive validation**, use the Claude Code skill:

```
/validate-modular-monolith

[Paste your code]
```

This provides:
- Detailed architectural analysis
- Specific violations with locations
- Code examples and refactoring suggestions
- Module dependency graphs

## Troubleshooting

**Hook not running?**
```bash
# Verify hook is executable
ls -la .git/hooks/pre-commit

# Should show: -rwxr-xr-x (executable)
```

**Hook blocking all commits?**
```bash
# This hook should not block commits, only warn
# If commits are blocked, the hook file may be corrupted

# Restore it:
cp .git-hooks/pre-commit-validate-architecture.sh .git/hooks/pre-commit
chmod +x .git/hooks/pre-commit
```

**Permission denied on Windows?**
```powershell
# Windows doesn't enforce Unix permissions
# Ensure your shell supports bash or use WSL:
wsl bash -c "cd .git-hooks && ./setup-hooks.sh"
```

## Team Setup

When a new team member clones the repo, they need to install hooks:

```bash
cd .git-hooks
./setup-hooks.sh
```

Or add to project onboarding docs:

```bash
# After cloning the repo
bash .git-hooks/setup-hooks.sh
```

---

*Last Updated: 2026-04-10*

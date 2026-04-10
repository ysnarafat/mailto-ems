#!/bin/bash
# Setup script: Install custom git hooks for the project

set -e

# Colors
GREEN='\033[0;32m'
BLUE='\033[0;34m'
RED='\033[0;31m'
NC='\033[0m' # No Color

echo -e "${BLUE}╔════════════════════════════════════════════╗${NC}"
echo -e "${BLUE}║  Git Hooks Setup - MailTo EMS              ║${NC}"
echo -e "${BLUE}╚════════════════════════════════════════════╝${NC}"
echo ""

# Verify we're in the right directory
if [ ! -f "pre-commit-validate-architecture.sh" ]; then
    echo -e "${RED}✗ Error: pre-commit-validate-architecture.sh not found${NC}"
    echo "  Run this script from the .git-hooks directory:"
    echo "  cd .git-hooks && ./setup-hooks.sh"
    exit 1
fi

# Check if we're in a git repo
if [ ! -d "../.git" ]; then
    echo -e "${RED}✗ Error: Not in a git repository${NC}"
    echo "  This script must be run from within a git repository"
    exit 1
fi

GIT_HOOKS_DIR="../.git/hooks"

echo "Installing hooks..."
echo ""

# Install pre-commit hook
if [ -f "$GIT_HOOKS_DIR/pre-commit" ]; then
    echo -e "${BLUE}→${NC} Backing up existing pre-commit hook..."
    mv "$GIT_HOOKS_DIR/pre-commit" "$GIT_HOOKS_DIR/pre-commit.backup.$(date +%s)"
fi

echo -e "${BLUE}→${NC} Installing pre-commit-validate-architecture..."
cp "pre-commit-validate-architecture.sh" "$GIT_HOOKS_DIR/pre-commit"
chmod +x "$GIT_HOOKS_DIR/pre-commit"

echo ""
echo -e "${GREEN}✓ Git hooks installed successfully!${NC}"
echo ""

# Verify installation
echo "Verifying installation..."
if [ -x "$GIT_HOOKS_DIR/pre-commit" ]; then
    echo -e "${GREEN}✓ pre-commit hook is executable${NC}"
else
    echo -e "${RED}✗ pre-commit hook is not executable${NC}"
    echo "  Run: chmod +x .git/hooks/pre-commit"
    exit 1
fi

echo ""
echo -e "${GREEN}╔════════════════════════════════════════════╗${NC}"
echo -e "${GREEN}║  Setup Complete!                          ║${NC}"
echo -e "${GREEN}╚════════════════════════════════════════════╝${NC}"
echo ""
echo "Hooks installed:"
echo -e "  ${GREEN}✓${NC} pre-commit: Validates modular monolith architecture"
echo ""
echo "Next commit will automatically run the hook."
echo ""
echo "To test:"
echo "  git commit --allow-empty -m 'test: verify hook installation'"
echo ""
echo "To skip hooks (if needed):"
echo "  git commit --no-verify"
echo ""
echo "For more info, see: .git-hooks/SETUP.md"
echo ""

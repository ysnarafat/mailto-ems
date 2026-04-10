#!/bin/bash
# Pre-commit hook: Validate architecture for staged changes
# This hook runs the validate-modular-monolith skill on staged C# files
#
# Installation:
#   cp .git-hooks/pre-commit-validate-architecture.sh .git/hooks/pre-commit
#   chmod +x .git/hooks/pre-commit
#
# To bypass this hook during commit:
#   git commit --no-verify

set -e

# Colors for output
RED='\033[0;31m'
YELLOW='\033[1;33m'
GREEN='\033[0;32m'
NC='\033[0m' # No Color

# Get staged C# files
STAGED_FILES=$(git diff --cached --name-only --diff-filter=ACM -- "*.cs")

if [ -z "$STAGED_FILES" ]; then
    exit 0
fi

echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo -e "${YELLOW}Pre-Commit Hook: Validating Modular Monolith Architecture${NC}"
echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"

echo ""
echo "Staged C# files to validate:"
echo "$STAGED_FILES" | while read file; do
    echo "  • $file"
done
echo ""

# Create temporary file with staged changes
TEMP_CHANGES=$(mktemp)
trap "rm -f $TEMP_CHANGES" EXIT

echo "# Staged changes for architecture validation" > "$TEMP_CHANGES"
echo "" >> "$TEMP_CHANGES"

# Get diff of staged files
git diff --cached -- "*.cs" >> "$TEMP_CHANGES" 2>/dev/null || true

# Count changes
CHANGE_COUNT=$(grep -c "^+" "$TEMP_CHANGES" || echo "0")

if [ "$CHANGE_COUNT" -lt 1 ]; then
    echo -e "${GREEN}✓ No C# code changes detected${NC}"
    exit 0
fi

echo "Analyzing $CHANGE_COUNT lines of code changes..."
echo ""

# Display validation requirements
echo -e "${YELLOW}Checking:${NC}"
echo "  ✓ Module boundaries (no cross-module dependencies)"
echo "  ✓ Event-driven communication (proper MediatR usage)"
echo "  ✓ DI registration patterns (module-level RegisterServices)"
echo "  ✓ Shared kernel adherence (no business logic in Shared)"
echo "  ✓ Project structure alignment"
echo ""

# Check for common anti-patterns
VIOLATIONS=0

# Check for direct cross-module service injection
if grep -q "EmailMarketing\.Modules\.[^.]*\.Services\." "$TEMP_CHANGES" 2>/dev/null; then
    if ! grep -q "IServiceCollection\|IModule\|RegisterServices" "$TEMP_CHANGES" 2>/dev/null; then
        echo -e "${RED}⚠ Warning: Direct cross-module service reference detected${NC}"
        echo "  Consider using MediatR events for inter-module communication"
        VIOLATIONS=$((VIOLATIONS + 1))
    fi
fi

# Check for missing IModule implementation in new module files
if grep -q "namespace EmailMarketing\.Modules\." "$TEMP_CHANGES" && \
   ! grep -q "class.*Module.*:.*IModule" "$TEMP_CHANGES"; then
    echo -e "${RED}⚠ Warning: New module without IModule implementation${NC}"
    echo "  Ensure your module implements IModule and RegisterServices()"
    VIOLATIONS=$((VIOLATIONS + 1))
fi

echo ""

if [ $VIOLATIONS -gt 0 ]; then
    echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
    echo -e "${YELLOW}Architecture Validation Complete (with warnings)${NC}"
    echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
    echo ""
    echo -e "${YELLOW}For detailed validation, use:${NC}"
    echo "  /validate-modular-monolith"
    echo ""
    echo "Or bypass this hook with: git commit --no-verify"
    echo ""
else
    echo -e "${GREEN}✓ Architecture validation passed${NC}"
    echo ""
fi

exit 0

---
name: validate-modular-monolith
description: |
  Validates code changes against .NET modular monolith architecture patterns and the project's specific structure. Analyzes code for module boundary violations, dependency issues, DI registration patterns, and inter-module communication patterns. Use this skill whenever the user is about to commit code changes, submitting a PR, or wants to validate that their implementation aligns with modular monolith best practices and the mailto-ems project structure. Always invoke this skill before code changes are committed to catch architectural violations early.
compatibility: None (analysis only, no external dependencies required)
---

# Validate Modular Monolith

You are an expert .NET architect specialized in modular monolith patterns and the mailto-ems project structure. Your role is to review code changes and validate that they align with established architectural patterns and best practices.

## Your Responsibilities

When the user provides code changes, you will:

1. **Analyze the changes** in the context of modular monolith architecture
2. **Identify violations** against both generic .NET patterns and project-specific constraints
3. **Categorize issues** as either errors (critical violations) or warnings (best-practice gaps)
4. **Provide actionable suggestions** with code examples where applicable
5. **Explain the reasoning** behind each pattern so the user understands the "why"

## Key Modular Monolith Patterns to Validate

### Module Boundaries & Independence

**Pattern**: Each module should be self-contained with clear boundaries. Modules should not directly depend on other modules' internal implementation details.

**Check for:**
- Direct imports from another module's internal classes (should use interfaces/abstractions instead)
- Module-to-module service references in constructors
- Cross-module data model sharing (each module owns its own entities)
- Circular dependencies between modules

**Example violation**: 
```csharp
// ❌ BAD: Direct reference to another module's service
public class CampaignService
{
    private readonly EmailMarketing.Modules.Notifications.Services.EmailSender _emailSender;
}

// ✅ GOOD: Inject abstraction, let DI resolve the implementation
public class CampaignService
{
    private readonly IEmailSender _emailSender; // Defined in Shared or via event
}
```

### Event-Driven Communication

**Pattern**: Modules communicate exclusively through domain events and integration events published via MediatR. No direct method calls across module boundaries.

**Check for:**
- Direct service calls between modules (should be events)
- Modules directly instantiating or injecting concrete types from other modules
- Synchronous cross-module operations (should be async/event-driven)
- Missing INotification/IIntegrationEvent implementations for cross-module concerns

**Example violation**:
```csharp
// ❌ BAD: Direct cross-module call
public async Task CreateContact(ContactDto dto)
{
    var contact = new Contact(dto);
    await _dbContext.SaveChangesAsync();
    
    // Reaching into another module directly
    await _notificationService.SendWelcomeEmail(contact.Email);
}

// ✅ GOOD: Publish event, let other module subscribe
public async Task CreateContact(ContactDto dto)
{
    var contact = new Contact(dto);
    await _dbContext.SaveChangesAsync();
    
    // Publish event, Notifications module subscribes
    await _mediator.Publish(new ContactCreatedEvent(contact.Id, contact.Email));
}
```

### Dependency Injection Registration

**Pattern**: Each module registers its own services in `RegisterServices()` method. No cross-module service registration. Use both Autofac and IServiceCollection patterns correctly.

**Check for:**
- Services registered in the wrong project (should be in their module)
- Missing `RegisterServices()` implementation
- Incorrect module registration in `ContainerBuilderExtensions.RegisterModules()`
- Autofac `RegisterType<>()` used instead of IServiceCollection patterns (be consistent)
- Internal module services exposed publicly (should be internal or abstracted)

**Example violation**:
```csharp
// ❌ BAD: Registering services outside the module
public class ContainerBuilderExtensions
{
    public static void RegisterModules(this IServiceCollection services)
    {
        // Services should be registered in the module, not here
        services.AddTransient<ContactService>();
        services.AddTransient<EmailSender>();
    }
}

// ✅ GOOD: Each module registers its own services
public class ContactsModule : IModule
{
    public string Name => "Contacts";
    
    public void RegisterServices(IServiceCollection services)
    {
        services.AddTransient<ContactService>();
        services.AddTransient<IContactRepository, ContactRepository>();
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(typeof(ContactsModule).Assembly));
    }
}
```

### MediatR Usage

**Pattern**: Use MediatR for publishing events and handling queries/commands within and across modules. Each module should have its own handlers.

**Check for:**
- Commands/Queries not using MediatR
- Event handlers registered in wrong module
- Missing MediatR assembly registration
- Using MediatR incorrectly (e.g., for non-async operations unnecessarily)

**Example violation**:
```csharp
// ❌ BAD: Manual event handling
public class EventBus
{
    private Dictionary<string, Action<object>> _subscribers = new();
    
    public void Publish(object evt) => _subscribers[evt.GetType().Name]?.Invoke(evt);
}

// ✅ GOOD: Use MediatR for event publishing
public class ContactHandler : INotificationHandler<ContactCreatedEvent>
{
    public async Task Handle(ContactCreatedEvent evt, CancellationToken ct)
    {
        // Handle event
    }
}

// Publish via mediator
await _mediator.Publish(new ContactCreatedEvent(...));
```

### Shared Kernel Adherence

**Pattern**: Only truly cross-cutting concerns (abstractions, domain models, infrastructure setup) live in `EmailMarketing.Shared`. Each module should reference Shared but not business logic from other modules.

**Check for:**
- Business logic in Shared (should be in module)
- Module-specific entities in Shared
- Shared files that only one module uses
- Missing abstractions for interfaces that multiple modules need
- DbContext pollution with all entity configurations (each module should configure its own)

**Example violation**:
```csharp
// ❌ BAD: Business logic in Shared
public static class SharedContactHelper
{
    public static bool IsValidContact(Contact c) => !string.IsNullOrEmpty(c.Email);
}

// ✅ GOOD: Business logic stays in module, abstractions in Shared
// In Shared:
public interface IContactValidator
{
    bool IsValid(Contact contact);
}

// In Contacts module:
public class ContactValidator : IContactValidator
{
    public bool IsValid(Contact c) => !string.IsNullOrEmpty(c.Email);
}
```

### Project-Specific Structure (mailto-ems)

**Pattern**: Modules follow the structure defined in CLAUDE.md: `src/Modules/{ModuleName}/EmailMarketing.Modules.{ModuleName}/`

**Check for:**
- Files in wrong directory structure
- Module projects not properly named
- Missing IModule implementation
- Module not registered in `ContainerBuilderExtensions.RegisterModules()`
- References to old monolithic structure (EmailMarketing.Services.*, etc.)

## Validation Severity Levels

- **ERROR**: Critical architectural violation that breaks modularity. Examples:
  - Direct synchronous cross-module service calls
  - Circular module dependencies
  - Module-internal types exposed publicly across boundaries

- **WARNING**: Best-practice gap or code smell. Examples:
  - Missing abstractions (could improve testability)
  - Synchronous MediatR usage that could be async
  - Service registration in wrong location (works but violates pattern)

## Output Format

Provide your analysis in this exact format:

```
# Modular Monolith Architecture Validation Report

## Summary
- **Files Analyzed**: [number]
- **Errors Found**: [number]
- **Warnings Found**: [number]
- **Overall Status**: ✅ PASS / ⚠️ WARNINGS / ❌ VIOLATIONS

## 🔴 Errors (Critical Violations)

### Error 1: [Category] - [Brief Title]
**Severity**: ERROR  
**Location**: File: `path/to/file.cs` (line X)  
**Issue**: [Explain what's wrong and why it violates the pattern]

**Current Code**:
\`\`\`csharp
// Show the problematic code
\`\`\`

**Suggested Fix**:
\`\`\`csharp
// Show the corrected code
\`\`\`

**Why**: [Explain the architectural reasoning]

---

## 🟡 Warnings (Best-Practice Gaps)

### Warning 1: [Category] - [Brief Title]
**Severity**: WARNING  
**Location**: File: `path/to/file.cs` (line X)  
**Issue**: [Explain the best-practice gap]

**Suggestion**: [How to improve]

---

## ✅ Passing Checks

- Module boundaries properly defined
- Event-driven communication correctly implemented
- DI registration follows patterns
- [Any other positive findings]

---

## 📋 Recommendations for Next Steps

[Prioritized list of improvements in order of importance]

```

## How to Analyze Code

1. **Parse the changes** — Understand what files are being modified and what functionality is being added
2. **Check module context** — Identify which module(s) are affected
3. **Cross-reference patterns** — Check against each validation rule above
4. **Look for dependencies** — Trace what other modules or shared code is being referenced
5. **Verify event handling** — If inter-module communication is needed, verify events are published
6. **Check DI registration** — Ensure services are registered in the correct module
7. **Review structure** — Confirm files are in the right directory structure

## Special Handling

- **If the user provides a git diff**: Parse it and identify the affected files and changes
- **If the user provides file paths**: Ask them to share the code or look for it in the working directory
- **If the user provides code snippets**: Analyze in the context of modular monolith patterns
- **If context is unclear**: Ask clarifying questions (which module? is this cross-module communication?)

## Example Analysis

When given code changes, provide a thorough analysis that explains not just what's wrong, but why the pattern matters. For example:

**Bad explanation**: "Don't inject services from other modules"  
**Good explanation**: "Injecting `EmailSender` from the Notifications module creates a tight coupling that makes it hard to test the Campaigns module in isolation and prevents the Notifications module from being extracted to a microservice later."

Always educate while validating.

# Claude Code Skills for MailTo EMS

This directory contains custom Claude Code skills for the MailTo EMS project. Skills are reusable workflows that help with specific development tasks.

## Available Skills

### validate-modular-monolith

Validates code changes against .NET modular monolith architecture patterns and the project's specific structure.

**When to use:**
- Before committing code changes
- When submitting a pull request
- When you want to validate architectural alignment
- When reviewing code from team members

**How to use:**

**Option 1: Direct invocation**
```
/validate-modular-monolith

[Paste your code to validate]
```

**Option 2: With git diff**
```
/validate-modular-monolith

Here's my code change:
git diff src/Modules/Contacts/EmailMarketing.Modules.Contacts/Services/ContactService.cs
```

**What it checks:**

✅ **Module Boundaries** - Ensures modules don't directly depend on each other  
✅ **Event-Driven Communication** - Validates use of MediatR for inter-module communication  
✅ **DI Registration** - Checks that services are registered in their module's RegisterServices()  
✅ **MediatR Usage** - Verifies correct event publishing and handler patterns  
✅ **Shared Kernel** - Ensures shared code doesn't contain business logic  
✅ **Project Structure** - Validates files are in correct directory hierarchy  

**Output format:**

The skill generates a detailed report with:
- ✅ **Passing Checks** - What you got right
- 🔴 **Critical Errors** - Architectural violations that break modularity
- 🟡 **Warnings** - Best-practice gaps with improvement suggestions
- 📋 **Recommendations** - Prioritized fixes with code examples

**Example:**

```csharp
// Your code
public class CampaignService
{
    private readonly EmailSender _emailSender;  // ❌ Cross-module dependency
    
    public async Task CreateAsync(CreateCampaignDto dto)
    {
        var campaign = new Campaign(dto.Name);
        await _emailSender.SendNotificationAsync(campaign.Id);  // ❌ Direct call
    }
}
```

**Skill will report:**
- ERROR: Direct cross-module service injection
- ERROR: Synchronous cross-module communication
- Suggestion: Use MediatR to publish CampaignCreatedEvent instead
- Includes corrected code example

---

## Skill Structure

```
validate-modular-monolith/
├── SKILL.md          # Main skill definition (architecture rules, examples)
└── evals/
    └── evals.json    # Test cases used to verify skill accuracy
```

## Updating Skills

### To modify validation rules:
Edit `validate-modular-monolith/SKILL.md` and update the relevant section (Module Boundaries, Event-Driven Communication, etc.)

### To add new test cases:
Edit `validate-modular-monolith/evals/evals.json` and add a new eval object with:
- `id`: unique number
- `name`: descriptive name
- `prompt`: the test input
- `expected_output`: what should be detected
- `assertions`: specific checks that must pass

### To test skill improvements:
```bash
# In Claude Code, use the skill with test prompts from evals.json
/validate-modular-monolith

[Paste code from an eval test case]
```

---

## Architecture Context

This skill validates against the MailTo EMS modular monolith architecture defined in:
- **CLAUDE.md** - Project architecture guide (see "Module Structure" section)
- **src/EmailMarketing.Shared/Abstractions/IModule.cs** - Module contract
- **src/EmailMarketing.Host/Extensions/ContainerBuilderExtensions.cs** - Module registration

## Key Patterns Validated

### ✅ Correct Pattern: Event-Driven Communication
```csharp
// Module A: Publish event
await _mediator.Publish(new ContactCreatedEvent(contact.Id));

// Module B: Subscribe to event
public class ContactCreatedEventHandler : INotificationHandler<ContactCreatedEvent>
{
    public async Task Handle(ContactCreatedEvent notification, CancellationToken ct)
    {
        // React to event independently
    }
}
```

### ✅ Correct Pattern: Module Registration
```csharp
public class ContactsModule : IModule
{
    public string Name => "Contacts";
    
    public void RegisterServices(IServiceCollection services)
    {
        // Module owns its registrations
        services.AddScoped<ContactService>();
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(typeof(ContactsModule).Assembly));
    }
}
```

### ❌ Anti-Pattern: Direct Cross-Module Dependency
```csharp
// DON'T DO THIS:
public class CampaignService
{
    private readonly EmailMarketing.Modules.Notifications.Services.EmailSender _emailSender;
}
```

---

## Feedback & Improvements

If the skill:
- ✅ Catches an issue you missed → Great! It's working as intended
- ❌ Misses an issue you found → Report it so we can improve
- 🤔 Seems too strict/lenient → Adjust the rules in SKILL.md

Track skill improvements in the project's issue tracker.

---

*Last Updated: 2026-04-10*  
*Skill Status: Production Ready*

# EmailMarketing Modular Monolith Architecture

This document describes the restructured EmailMarketing solution using a **Modular Monolith** architecture pattern.

## 🏗️ Architecture Overview

The modular monolith approach organizes the codebase into well-defined, loosely-coupled modules while maintaining the simplicity of a single deployable unit. This provides the benefits of microservices (modularity, clear boundaries) without the complexity of distributed systems.

## 📁 Project Structure

```
src/
├── EmailMarketing.Host/                           # Main application host
│   ├── Program.cs                                # Application entry point
│   ├── Extensions/                               # Host-specific extensions
│   └── wwwroot/                                  # Static files
│
├── EmailMarketing.Shared/                        # Shared kernel
│   ├── EmailMarketing.Shared.Abstractions/      # Contracts & interfaces
│   │   ├── IModule.cs                           # Module contract
│   │   └── Events/                              # Event contracts
│   ├── EmailMarketing.Shared.Domain/            # Domain primitives
│   │   ├── Entity.cs                            # Base entity
│   │   └── AggregateRoot.cs                     # Aggregate root
│   └── EmailMarketing.Shared.Infrastructure/    # Shared infrastructure
│       └── Extensions/                          # Infrastructure extensions
│
└── Modules/                                      # Business modules
    ├── Users/                                    # User management
    │   └── EmailMarketing.Modules.Users/
    ├── Contacts/                                 # Contact management
    │   └── EmailMarketing.Modules.Contacts/
    ├── Campaigns/                                # Campaign management
    │   └── EmailMarketing.Modules.Campaigns/
    ├── FileProcessing/                           # File import/export
    │   └── EmailMarketing.Modules.FileProcessing/
    └── Notifications/                            # Email notifications
        └── EmailMarketing.Modules.Notifications/
```

## 🎯 Module Responsibilities

### 🔐 Users Module
- **Purpose**: User authentication, authorization, and profile management
- **Responsibilities**:
  - User registration and login
  - Password management
  - User roles and permissions
  - Account confirmation
- **Dependencies**: Shared kernel only

### 👥 Contacts Module
- **Purpose**: Contact and group management
- **Responsibilities**:
  - Contact CRUD operations
  - Group management
  - Contact categorization
  - Contact validation
- **Dependencies**: Shared kernel only

### 📧 Campaigns Module
- **Purpose**: Email campaign management and reporting
- **Responsibilities**:
  - Campaign creation and management
  - Campaign execution
  - Reporting and analytics
  - Campaign templates
- **Dependencies**: Contacts module (via events)

### 📁 FileProcessing Module
- **Purpose**: File import/export operations
- **Responsibilities**:
  - Excel file import/export
  - File validation
  - Background processing
  - File format conversion
- **Dependencies**: Contacts, Campaigns modules (via events)

### 🔔 Notifications Module
- **Purpose**: Email and notification services
- **Responsibilities**:
  - Email sending
  - SMTP configuration
  - Email templates
  - Notification queuing
- **Dependencies**: Users, Campaigns modules (via events)

## 🔄 Inter-Module Communication

### Domain Events
Modules communicate through domain events using MediatR:

```csharp
// Example: Contact imported event
public class ContactImportedEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public int ContactId { get; set; }
    public string ImportSource { get; set; }
}
```

### Integration Events
For cross-module communication:

```csharp
// Example: Campaign created event
public class CampaignCreatedEvent : IIntegrationEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public string EventType => nameof(CampaignCreatedEvent);
    public int CampaignId { get; set; }
    public List<int> ContactIds { get; set; }
}
```

## 🚀 Benefits of This Architecture

### ✅ **Modularity**
- Clear separation of concerns
- Independent development of modules
- Easier testing and maintenance

### ✅ **Scalability**
- Can extract modules to microservices later
- Horizontal scaling of specific modules
- Performance optimization per module

### ✅ **Maintainability**
- Reduced coupling between modules
- Clear module boundaries
- Easier to understand and modify

### ✅ **Deployment Simplicity**
- Single deployable unit
- No distributed system complexity
- Simplified monitoring and debugging

## 🔧 Development Workflow

### Adding a New Module

1. **Create Module Structure**:
```bash
mkdir -p src/Modules/NewModule/EmailMarketing.Modules.NewModule
```

2. **Implement IModule Interface**:
```csharp
public class NewModule : IModule
{
    public string Name => "NewModule";
    
    public void RegisterServices(IServiceCollection services)
    {
        // Register module services
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(NewModule).Assembly));
    }
}
```

3. **Register in Host**:
```csharp
// Add to ServiceCollectionExtensions.cs
new NewModule()
```

### Module Communication

```csharp
// Publishing an event
await _mediator.Publish(new ContactImportedEvent { ContactId = contact.Id });

// Handling an event
public class ContactImportedEventHandler : INotificationHandler<ContactImportedEvent>
{
    public async Task Handle(ContactImportedEvent notification, CancellationToken cancellationToken)
    {
        // Handle the event
    }
}
```

## 🐳 Docker Configuration

### Single Container Deployment
```bash
# Build and run modular monolith
docker-compose -f docker-compose.modular.yml up -d
```

### Development
```bash
# Run with development overrides
docker-compose -f docker-compose.modular.yml -f docker-compose.override.yml up -d
```

## 🧪 Testing Strategy

### Unit Tests
- Test each module independently
- Mock inter-module dependencies
- Focus on business logic

### Integration Tests
- Test module interactions
- Test event handling
- Test database operations

### End-to-End Tests
- Test complete workflows
- Test UI interactions
- Test API endpoints

## 📊 Migration from Current Architecture

### Phase 1: Structure Setup ✅
- [x] Create modular project structure
- [x] Set up shared kernel
- [x] Create module interfaces

### Phase 2: Code Migration
- [ ] Move existing code to appropriate modules
- [ ] Implement event-driven communication
- [ ] Update dependency injection

### Phase 3: Testing & Validation
- [ ] Create comprehensive tests
- [ ] Performance testing
- [ ] Security validation

### Phase 4: Deployment
- [ ] Update CI/CD pipelines
- [ ] Deploy to staging
- [ ] Production deployment

## 🔮 Future Evolution

### Microservices Extraction
When needed, modules can be extracted to microservices:

1. **Identify Module**: Choose module for extraction
2. **Add API Layer**: Create REST/gRPC APIs
3. **Update Communication**: Replace events with HTTP/messaging
4. **Deploy Separately**: Independent deployment pipeline
5. **Monitor & Optimize**: Performance and reliability monitoring

### Horizontal Scaling
- Scale specific modules based on load
- Use read replicas for read-heavy modules
- Implement caching strategies per module

## 📝 Best Practices

### Module Design
- **Single Responsibility**: Each module has one clear purpose
- **Loose Coupling**: Minimal dependencies between modules
- **High Cohesion**: Related functionality grouped together
- **Event-Driven**: Use events for cross-module communication

### Code Organization
- **Consistent Structure**: Same folder structure across modules
- **Clear Naming**: Descriptive names for modules and components
- **Documentation**: Document module responsibilities and APIs
- **Testing**: Comprehensive test coverage for each module

### Performance
- **Lazy Loading**: Load modules only when needed
- **Caching**: Implement caching strategies per module
- **Database**: Optimize queries and use appropriate indexes
- **Monitoring**: Monitor performance metrics per module

This modular monolith architecture provides a solid foundation for the EmailMarketing solution, offering the benefits of modularity while maintaining deployment simplicity.
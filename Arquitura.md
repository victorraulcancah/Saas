backend/
├── src/
│   ├── Backend.API/                  ← Proyecto principal ASP.NET
│   │   ├── Controllers/
│   │   │   ├── ERP/
│   │   │   │   ├── FinanceController.cs
│   │   │   │   ├── InventoryController.cs
│   │   │   │   └── PurchaseController.cs
│   │   │   ├── HR/
│   │   │   │   ├── EmployeeController.cs
│   │   │   │   ├── PayrollController.cs
│   │   │   │   └── VacationController.cs
│   │   │   └── CRM/
│   │   │       ├── CustomerController.cs
│   │   │       ├── PipelineController.cs
│   │   │       └── SupportController.cs
│   │   ├── Middleware/
│   │   │   ├── AuthMiddleware.cs
│   │   │   ├── ErrorHandlingMiddleware.cs
│   │   │   └── RateLimitMiddleware.cs
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   └── appsettings.Development.json
│   │
│   ├── Backend.Application/           ← Lógica de negocio (CQRS / Services)
│   │   ├── ERP/
│   │   │   ├── Commands/
│   │   │   ├── Queries/
│   │   │   └── Services/
│   │   ├── HR/
│   │   │   ├── Commands/
│   │   │   ├── Queries/
│   │   │   └── Services/
│   │   ├── CRM/
│   │   │   ├── Commands/
│   │   │   ├── Queries/
│   │   │   └── Services/
│   │   └── Common/
│   │       ├── Interfaces/
│   │       └── Behaviors/            ← Validación, logging, etc.
│   │
│   ├── Backend.Domain/                ← Entidades y reglas de dominio
│   │   ├── ERP/
│   │   │   ├── Entities/
│   │   │   └── Events/
│   │   ├── HR/
│   │   │   ├── Entities/
│   │   │   └── Events/
│   │   ├── CRM/
│   │   │   ├── Entities/
│   │   │   └── Events/
│   │   └── Common/
│   │       ├── BaseEntity.cs
│   │       └── ValueObjects/
│   │
│   └── Backend.Infrastructure/        ← Acceso a datos y servicios externos
│       ├── Persistence/
│       │   ├── PostgreSQL/            ← ERP + CRM
│       │   │   ├── AppDbContext.cs
│       │   │   ├── Migrations/
│       │   │   └── Repositories/
│       │   └── MongoDB/               ← HR
│       │       ├── MongoDbContext.cs
│       │       └── Repositories/
│       ├── Cache/
│       │   └── Redis/
│       │       ├── RedisCacheService.cs
│       │       └── RedisConnectionFactory.cs
│       ├── Messaging/                 ← RabbitMQ / Kafka
│       │   ├── Publishers/
│       │   └── Consumers/
│       └── DependencyInjection.cs
│
└── tests/
    ├── Backend.UnitTests/
    ├── Backend.IntegrationTests/
    └── Backend.ArchitectureTests/

    # Arquitectura Backend

---

## Clientes
`Web` · `Mobile` · `Third-party`

---

## API Gateway
> Auth · Rate limiting · Routing

- - - - - - - - - - - →  **Redis** *(Cache · Sesiones)*

---

## Servicios de negocio

| ERP Service | HR Service | CRM Service |
|---|---|---|
| Finanzas · Inventario · Compras | Empleados · Nómina · Vacaciones | Clientes · Pipeline · Soporte |

---

## Message Broker
> Comunicación asíncrona entre servicios

---

## Bases de datos

| PostgreSQL | MongoDB | PostgreSQL |
|---|---|---|
| ERP · transaccional | HR · documentos flexibles | CRM · relacional |

---

{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "AllowedOrigins": ["http://localhost:5173", "http://localhost:4173"],
  "ConnectionStrings": {
    "PostgreSQL": "Host=localhost;Port=5432;Database=saas_erp;Username=postgres;Password=",
    "Redis": "localhost:6379",
    "MongoDB": "mongodb://localhost:27017"
  },
  "JwtSettings": {
    "SecretKey": "SuperSecretKeyForJwtToken_ChangeInProduction_Min32Chars!",
    "Issuer": "BackendSaas",
    "Audience": "BackendSaasClient",
    "ExpirationHours": 8
  },
  "MongoDBSettings": {
    "DatabaseName": "saas_erp_logs"
  }
}

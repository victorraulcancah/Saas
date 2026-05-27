# 🏗️ Backend-Saas — Arquitectura por Capas

**Stack:** .NET + Entity Framework Core  
**Patrón:** Clean Architecture + Repository por Aggregate  
**Última actualización:** mayo 2026

---

## 📦 Proyectos de la Solución

```
Backend-Saas.slnx
├── Backend.SharedKernel      ← transversal, sin dependencias
├── Backend.Domain            ← reglas de negocio puras
├── Backend.Application       ← casos de uso, interfaces, DTOs
├── Backend.Infrastructure    ← EF Core, repositorios, BD
└── Backend.Api               ← HTTP, controllers, middleware, SignalR
```

### Flujo de dependencias

```
Backend.Api
     ↓
Backend.Infrastructure  →  Backend.Application  →  Backend.Domain
                                   ↓                      ↓
                         Backend.SharedKernel  ←──────────┘
```

> **Regla de oro:** las dependencias siempre apuntan hacia adentro.  
> Infrastructure conoce a Application. Application conoce a Domain.  
> **Nadie conoce a Infrastructure ni a Api.**

---

## 🔷 Backend.SharedKernel

**Responsabilidad:** Contratos y clases base que usan TODOS los proyectos sin crear ciclos de referencia.

**Depende de:** Ningún proyecto interno.

```
SharedKernel/
├── MultiTenancy/
│   ├── ITenantContext.cs       ← interfaz del tenant actual
│   └── TenantMiddleware.cs     ← extrae TenantId del JWT por request
├── BaseEntity.cs               ← Id, TenantId, CreatedAt, UpdatedAt, IsDeleted
└── BaseRepository.cs           ← filtro automático de tenant y soft-delete
```

### Qué hace cada archivo

| Archivo | Responsabilidad |
|---|---|
| `BaseEntity` | Propiedades comunes para las 200+ entidades del sistema. Garantiza que `TenantId` exista en todas las tablas sin tener que repetirlo. |
| `ITenantContext` | Contrato que expone el tenant activo. Infrastructure lo implementa; Application y Domain lo consumen. |
| `TenantMiddleware` | Se ejecuta en cada request HTTP. Lee el JWT, extrae el `TenantId` y lo inyecta. Rechaza requests sin tenant con 401 en un solo lugar. |
| `BaseRepository<T>` | Aplica `Where(TenantId)` y `Where(!IsDeleted)` automáticamente en todas las queries. Los repositorios concretos heredan esto — nunca pueden olvidar el filtro. |

### Principios aplicados

| Principio | Dónde |
|---|---|
| **DRY** | `BaseEntity` — Id, TenantId y fechas se definen una sola vez |
| **SRP** | Cada clase tiene una única razón de cambio |
| **DIP** | `ITenantContext` es interfaz — los servicios no dependen de la implementación concreta |
| **Security by Design** | El aislamiento multitenant es estructural, no opcional |
| **Convention over Config** | Las reglas se cumplen por herencia, no por disciplina del dev |

### Lo que NO debe tener

- Referencias a `Microsoft.EntityFrameworkCore`
- Referencias a `Microsoft.AspNetCore` (excepto `TenantMiddleware`)
- Lógica de negocio del ERP

---

## 🟦 Backend.Domain

**Responsabilidad:** El corazón del negocio. Contiene únicamente reglas de negocio puras, sin conocer EF, HTTP ni ninguna tecnología.

**Depende de:** `Backend.SharedKernel`

```
Domain/
├── Sistema0/Saas/
│   ├── Tenant.cs
│   ├── Subscription.cs
│   └── License.cs
├── Sistema1/ERP/
│   ├── Order.cs
│   └── Invoice.cs
├── Sistema2/POS/
│   ├── Sale.cs
│   └── CashRegister.cs
├── Sistema3/CRM/
│   └── Customer.cs
└── ... (un aggregate root por sistema)
```

### Qué va aquí

| Concepto | Descripción |
|---|---|
| **Entidades (Aggregate Roots)** | Clases con identidad e identidad propia: `Order`, `Sale`, `Tenant`. Contienen validaciones y comportamiento de negocio. Heredan de `BaseEntity`. |
| **Value Objects** | Objetos sin identidad: `Money`, `Address`, `TaxId`. Inmutables. Centralizan validaciones de formato. |
| **Domain Exceptions** | `InsufficientStockException`, `TenantSuspendedException`. Distinguen errores de negocio de errores técnicos. |
| **Domain Events** *(fase 2+)* | Señales de eventos importantes: `OrderPlaced`, `StockLow`. Desacoplan sistemas entre sí. |

### Principios aplicados

| Principio | Dónde |
|---|---|
| **SRP** | `Order` solo sabe de pedidos; `Invoice` solo de facturación |
| **Encapsulación** | Las propiedades son `private set` — el estado solo cambia por métodos validados |
| **Rich Domain Model** | La lógica vive en la entidad, no en servicios externos |
| **Ubiquitous Language** | Los nombres reflejan el vocabulario del negocio (`Order`, `Tenant`, `Invoice`) |
| **DDD — Aggregates** | `Order` controla sus `OrderItems`; nadie modifica `Items` directamente |

### Lo que NO debe tener

- Ningún `using Microsoft.EntityFrameworkCore`
- Llamadas a repositorios o servicios externos
- Referencias a DTOs o modelos de la API

---

## 🟩 Backend.Application

**Responsabilidad:** Orquesta casos de uso. Coordina entidades y repositorios. Define qué puede hacer el sistema sin preocuparse de cómo se persiste ni cómo llega la petición.

**Depende de:** `Backend.Domain` + `Backend.SharedKernel`

```
Application/
├── Interfaces/
│   ├── Repositories/
│   │   ├── ITenantRepository.cs
│   │   ├── IOrderRepository.cs
│   │   └── ISaleRepository.cs
│   └── Services/
│       ├── ILicenseGuard.cs
│       ├── IEmailService.cs
│       └── IPaymentGateway.cs
├── Sistema0/Saas/
│   ├── TenantService.cs
│   └── SubscriptionService.cs
├── Sistema2/POS/
│   └── SaleService.cs
└── DTOs/
    ├── CreateSaleDto.cs
    └── SaleResponseDto.cs
```

### Qué hace cada parte

| Parte | Responsabilidad |
|---|---|
| **Interfaces de Repositorios** | Definen el contrato que Infrastructure debe cumplir. Application sabe QUÉ necesita, no CÓMO se implementa. Permiten mocks en tests. |
| **Interfaces de Servicios** | `ILicenseGuard` valida que el tenant tenga activo el módulo. `IEmailService` e `IPaymentGateway` desacoplan proveedores externos. |
| **Application Services** | Orquestan el caso de uso completo: validan licencia → llaman dominio → persisten → retornan DTO. |
| **DTOs** | Objetos planos de entrada y salida. Desacoplan la API de las entidades internas. |

### Principios aplicados

| Principio | Dónde |
|---|---|
| **SRP** | `SaleService` solo maneja ventas POS — no factura ni gestiona inventario |
| **DIP** | Depende de `IOrderRepository`, nunca de EF ni SQL directamente |
| **ISP** | `IOrderRepository` no expone métodos de WMS ni CRM |
| **Fail Fast** | `ILicenseGuard` se valida primero — si no hay licencia, falla antes de tocar la BD |

### Lo que NO debe tener

- Ningún `using Microsoft.EntityFrameworkCore`
- Instancias de `DbContext` o repositorios concretos
- Retorno de entidades de dominio — siempre traducir a DTOs

---

## 🟧 Backend.Infrastructure

**Responsabilidad:** Implementa los contratos de Application usando tecnologías concretas: EF Core, SQL Server, SendGrid, etc. Es la única capa que puede cambiar de tecnología sin afectar al negocio.

**Depende de:** `Backend.Application` + `Backend.Domain` + `Backend.SharedKernel`

```
Infrastructure/
├── Persistence/
│   ├── AppDbContext.cs               ← DbContext con filtros globales
│   ├── TenantContext.cs              ← implementación de ITenantContext
│   ├── Repositories/
│   │   ├── TenantRepository.cs
│   │   ├── OrderRepository.cs
│   │   └── SaleRepository.cs
│   └── Configurations/
│       ├── OrderConfiguration.cs     ← Fluent API por entidad
│       └── SaleConfiguration.cs
├── Migrations/
└── Services/
    ├── LicenseGuard.cs               ← implementa ILicenseGuard
    └── EmailService.cs               ← implementa IEmailService
```

### Qué hace cada parte

| Parte | Responsabilidad |
|---|---|
| `AppDbContext` | Registra DbSets, aplica filtros globales de tenant e `IsDeleted`, actualiza `UpdatedAt` automáticamente en `SaveChangesAsync`. |
| `TenantContext` | Implementa `ITenantContext` leyendo el claim del `HttpContext` actual. Es scoped por request. |
| `Repositorios concretos` | Implementan las interfaces de Application usando EF Core. Heredan de `BaseRepository` — el filtro tenant ya viene incluido. |
| `Configurations` | Mapeo entidad → tabla con Fluent API. Índices, constraints y relaciones. Un archivo por entidad. |
| `Migrations` | Historial de cambios del esquema de base de datos. Generados por EF Core. |

### Principios aplicados

| Principio | Dónde |
|---|---|
| **DIP** | Los repositorios implementan interfaces de Application, no al revés |
| **SRP** | Un repositorio por aggregate — `OrderRepository` no gestiona inventario |
| **OCP** | Se extiende `BaseRepository` sin modificar la lógica base de multitenancy |
| **Security by Default** | Los filtros globales de EF hacen imposible hacer una query sin filtro de tenant |

### Lo que NO debe tener

- Lógica de negocio (un `if` de negocio pertenece a Domain o Application)
- Llamadas directas a controllers o servicios de la API

---

## 🔴 Backend.Api

**Responsabilidad:** Punto de entrada HTTP. Gestiona requests, autenticación, autorización, WebSockets (SignalR) y respuestas. No contiene lógica de negocio.

**Depende de:** `Backend.Application` + `Backend.Infrastructure` + `Backend.SharedKernel`

```
Backend.Api/
├── Controllers/               ← endpoints REST por sistema
├── DTOs/                      ← request/response bodies de la API (si difieren de Application)
├── Hubs/
│   └── SignalRRealtimeNotificationService  ← notificaciones en tiempo real
├── Middleware/                ← TenantMiddleware, ExceptionMiddleware
├── Authorization/             ← políticas y handlers de permisos
├── Services/                  ← servicios específicos de la capa HTTP
├── Properties/
├── appsettings.json
└── Program.cs                 ← registro de DI, middleware pipeline
```

### Qué hace cada parte

| Parte | Responsabilidad |
|---|---|
| **Controllers** | Reciben el request HTTP, llaman al servicio de Application correspondiente, retornan el DTO de respuesta. Sin lógica de negocio. |
| **Hubs (SignalR)** | Notificaciones en tiempo real al cliente: ventas confirmadas, stock bajo, pedidos actualizados. |
| **Middleware** | `TenantMiddleware` extrae el TenantId del JWT. `ExceptionMiddleware` convierte excepciones de dominio en respuestas HTTP consistentes. |
| **Authorization** | Políticas que combinan roles globales del Sistema 0 con permisos por módulo. |
| **Program.cs** | Registro de DI de todos los proyectos, pipeline de middleware, configuración de EF, Swagger, CORS. |

### Principios aplicados

| Principio | Dónde |
|---|---|
| **SRP** | Los controllers solo traducen HTTP ↔ Application — no tienen lógica de negocio |
| **Thin Controller** | Toda la lógica va a Application; el controller solo orquesta la llamada |
| **Fail Fast** | El middleware rechaza requests inválidos antes de llegar a los servicios |

### Lo que NO debe tener

- Lógica de negocio en controllers
- Llamadas directas a repositorios o DbContext
- Reglas de validación de negocio (eso es dominio)

---

## 🔄 Flujo completo de un request

Ejemplo: `POST /api/pos/sales` — crear una venta

```
1. TenantMiddleware (Api)          → extrae TenantId del JWT
2. PosController (Api)             → recibe CreateSaleDto, llama SaleService
3. SaleService (Application)       → valida licencia módulo POS con ILicenseGuard
4. SaleService (Application)       → llama Sale.Create() — reglas de negocio en Domain
5. SaleService (Application)       → llama ISaleRepository.AddAsync(sale)
6. SaleRepository (Infrastructure) → hereda Query con TenantId ya filtrado
7. AppDbContext (Infrastructure)   → persiste con filtro global activo
8. SaleService (Application)       → retorna SaleDto (nunca la entidad)
9. PosController (Api)             → responde 201 Created con el DTO
```

---

## ✅ Checklist antes de hacer un PR

- [ ] La entidad hereda de `BaseEntity` → tiene `TenantId` garantizado
- [ ] El repositorio hereda de `BaseRepository` → filtro tenant automático
- [ ] Application define la interfaz del repositorio antes de que Infrastructure la implemente
- [ ] El servicio de Application llama `ILicenseGuard` antes de cualquier operación
- [ ] Domain no tiene ningún `using Microsoft.EntityFrameworkCore`
- [ ] Los servicios retornan DTOs, nunca entidades de dominio
- [ ] La lógica de negocio está en Domain o Application, no en Infrastructure ni Api
- [ ] El controller no tiene `if`s de negocio — solo llama al servicio y retorna
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

---

## 🧭 Estado actual de construcción

Esta sección aterriza la arquitectura contra el avance real de la solución `Backend-Saas`.

### Proyectos reales en disco

```
Backend-Saas/
├── Backend.SharedKernel
├── Backend.Domain
├── Backend.Application
├── Backend.Infrastructure
└── Backend-Api
```

> Nota: en el documento se habla de `Backend.Api` como capa conceptual. En disco el proyecto se llama `Backend-Api`.

### Infraestructura base ya armada

| Componente | Estado | Observación |
|---|---:|---|
| PostgreSQL / EF Core | En construcción | `AppDbContext` ya concentra SaaS, ERP, POS, CRM y HR. |
| Identity / JWT | En construcción | Roles base: `SuperAdmin`, `AdminTenant`, `Usuario`. |
| Multi-tenant | En construcción | Filtros por `TenantId` e `IsDeleted` aplicados desde `AppDbContext`. |
| Catálogo SaaS | En construcción | Ya existen sistemas, módulos, submódulos, planes y licencias. |
| Control de acceso SaaS | En construcción | `ISaasAccessService` valida tenant, suscripción, sistema, módulo y submódulo. |
| Redis | Base creada | Cache para snapshot de licencias por tenant. |
| MongoDB | Base creada | Auditoría/logs mediante `MongoAuditService`. |
| SignalR | Base creada | Canal `/hubs/notifications` para eventos en tiempo real. |
| Swagger/OpenAPI | Base creada | Disponible en ambiente de desarrollo. |

### Sistemas con implementación inicial

| Sistema | Domain | Application | Infrastructure | Api |
|---|---:|---:|---:|---:|
| 0. Plataforma SaaS Central | Sí | Sí | Sí | Sí |
| 1. ERP | Sí | Sí | Sí | Sí |
| 2. POS | Sí | Sí | Sí | Sí |
| 3. CRM | Sí | Sí | Sí | Parcial |
| 4. RH / HR | Sí | Parcial | Parcial | Pendiente |
| 5. OMS | Pendiente | Pendiente | Pendiente | Pendiente |
| 6. WMS | Pendiente | Pendiente | Pendiente | Pendiente |
| 7. TMS | Carpeta base | Pendiente | Pendiente | Pendiente |
| 8. PIM | Pendiente | Pendiente | Pendiente | Pendiente |
| 9. SFA | Pendiente | Pendiente | Pendiente | Pendiente |
| 10. Help Desk | Incluido parcialmente en CRM | Parcial | Parcial | Pendiente |
| 11. Retail Analytics | Pendiente | Pendiente | Pendiente | Pendiente |
| 12. Prevención de Pérdidas | Pendiente | Pendiente | Pendiente | Pendiente |
| 13. BI | Pendiente | Pendiente | Pendiente | Pendiente |

---

## 🧩 Mapa estructural por los 14 sistemas

El archivo `doc/Sistemas-para-saas-reorganizado.md` define el mapa funcional. Esta arquitectura lo convierte en estructura de backend.

### Regla de carpetas por sistema

Cada sistema debe tener la misma forma en las capas principales:

```
Backend.Domain/
└── SistemaX/
    ├── Entities/
    ├── Events/
    ├── ValueObjects/
    └── Exceptions/

Backend.Application/
└── SistemaX/
    ├── Models/
    ├── Services/
    ├── Commands/
    └── Queries/

Backend.Infrastructure/
├── Services/
└── Persistence/PostgreSQL/
    ├── Configurations/
    └── Repositories/

Backend-Api/
└── Controllers/
    └── SistemaX/
```

### Convención de nombres

| Pieza | Convención | Ejemplo |
|---|---|---|
| Entidad de dominio | Nombre de negocio singular | `Product`, `PosSale`, `TenantSubscription` |
| Servicio de Application | `I{Sistema}{Proceso}Service` | `IErpInventoryService` |
| Implementación | `{Sistema}{Proceso}Service` | `ErpInventoryService` |
| Controller | `{Recurso}Controller` | `ProductsController` |
| Modelo de entrada | `{Accion}{Recurso}Request` | `CreateTenantRequest` |
| Modelo de salida | `{Recurso}Response` | `TenantResponse` |
| Permiso | `{system}.{module}.{submodule}.{action}` | `erp.inventory.products.read` |

---

## 🏛️ Sistema 0 — Plataforma SaaS Central

**Objetivo técnico:** controlar quién puede usar cada sistema, módulo y submódulo.

### Entidades base

| Entidad | Responsabilidad |
|---|---|
| `Tenant` | Cliente SaaS dueño de sus datos, usuarios y sucursales. |
| `SaasSystem` | Sistema comercial vendible: ERP, POS, CRM, etc. |
| `SaasModule` | Módulo vendible dentro de un sistema. |
| `SaasSubModule` | Funcionalidad granular activable por plan o add-on. |
| `SaasPlan` | Plan comercial: Starter, Professional, Enterprise o personalizado. |
| `TenantSubscription` | Suscripción activa, trial, vencida, suspendida o cancelada. |
| `TenantSystemLicense` | Habilita/deshabilita un sistema completo para un tenant. |
| `TenantModuleLicense` | Habilita/deshabilita un módulo específico. |
| `TenantSubModuleLicense` | Habilita/deshabilita una función granular. |
| `TenantCompanyProfile` | Datos fiscales y comerciales de la empresa. |
| `TenantBranch` | Sucursales del tenant. |
| `TenantSunatCredential` | Credenciales tributarias por tenant. |
| `InvoiceSeries` | Series de facturación por empresa/sucursal. |

### Flujo obligatorio antes de ejecutar cualquier módulo

```
1. JWT identifica usuario y tenant.
2. TenantMiddleware resuelve TenantId.
3. Authorization valida rol/permisos.
4. ISaasAccessService valida:
   - tenant activo
   - suscripción activa
   - sistema habilitado
   - módulo habilitado
   - submódulo habilitado, si aplica
5. Application ejecuta el caso de uso.
```

### Construcción pendiente recomendada

- Consolidar atributos de autorización por sistema/módulo/submódulo.
- Invalidar cache de licencias cuando cambie una suscripción o un add-on.
- Registrar auditoría por cambios de plan, licencias, usuarios y permisos.
- Agregar límites de uso por plan: usuarios, sucursales, documentos, API calls y almacenamiento.

---

## 📦 Sistema 1 — ERP

**Objetivo técnico:** núcleo financiero, compras, facturación e inventario central.

### Subdominios ERP

| Subdominio | Entidades principales | Servicios esperados |
|---|---|---|
| Catálogo | `Product`, `Category`, `Supplier` | `IErpCatalogService` |
| Inventario | `Warehouse`, `WarehouseLocation`, `ProductStock`, `InventoryMovement` | `IErpInventoryService` |
| Compras | `PurchaseOrder`, `PurchaseOrderItem`, `GoodsReceipt` | `IErpPurchasingService` |
| Transferencias | `WarehouseTransfer`, `WarehouseTransferItem`, `DispatchGuide` | `IErpInventoryService` |
| Facturación | `Invoice`, `InvoiceSeries` | `IErpBillingService` |

### Reglas críticas

- Todo movimiento de stock debe generar `InventoryMovement`.
- Ninguna venta, compra, transferencia o ajuste debe modificar stock sin registrar origen.
- Facturación debe validar serie activa, datos fiscales del tenant y correlativo.
- Los productos pertenecen al tenant, pero pueden exponerse a POS, OMS, PIM, WMS y BI.

### Construcción pendiente recomendada

- Separar comandos transaccionales: compra, recepción, transferencia, ajuste y facturación.
- Agregar eventos de dominio: `StockChanged`, `StockLow`, `InvoiceIssued`, `GoodsReceived`.
- Crear endpoints de consulta optimizados para stock por almacén/sucursal.
- Preparar integración tributaria real para SUNAT como servicio externo desacoplado.

---

## 🛍️ Sistema 2 — POS

**Objetivo técnico:** ventas de tienda física, caja, pagos y stock local.

### Entidades base

| Entidad | Responsabilidad |
|---|---|
| `PosSale` | Venta realizada en caja. |
| `PosSaleItem` | Ítems vendidos, cantidades, precios y descuentos. |
| `CashRegister` *(pendiente)* | Caja, apertura, cierre y arqueo. |
| `Payment` *(pendiente)* | Medios de pago: efectivo, tarjeta, Yape/Plin, mixto. |

### Integraciones obligatorias

- Consulta productos y precios desde ERP/PIM.
- Descuenta stock vía ERP Inventario.
- Emite comprobante vía ERP Facturación.
- Notifica ventas en tiempo real vía SignalR.
- Expone datos a BI y Retail Analytics.

### Construcción pendiente recomendada

- Implementar caja: apertura, cierre, arqueo, diferencias y responsable.
- Implementar pagos múltiples por venta.
- Agregar anulaciones/devoluciones con auditoría.
- Soportar operación offline con sincronización controlada.

---

## 🎯 Sistema 3 — CRM

**Objetivo técnico:** clientes, oportunidades, cotizaciones, pedidos comerciales y postventa.

### Entidades base

| Entidad | Responsabilidad |
|---|---|
| `Customer` | Cliente final o corporativo. |
| `Opportunity` | Oportunidad comercial en pipeline. |
| `SalesOrder` | Pedido comercial antes de despacho/facturación. |
| `SalesOrderItem` | Detalle del pedido. |
| `SupportTicket` | Ticket de atención o reclamo. |

### Construcción pendiente recomendada

- Agregar controllers para CRM si aún no están expuestos.
- Separar Help Desk como Sistema 10 si el volumen crece.
- Conectar `SalesOrder` con OMS para orquestar pedidos omnicanal.
- Agregar trazabilidad de campañas y origen del cliente.

---

## 👥 Sistema 4 — RH / HR

**Objetivo técnico:** empleados, asistencia, turnos, nómina y comisiones.

### Entidades base

| Entidad | Responsabilidad |
|---|---|
| `Employee` | Trabajador del tenant. |
| `Attendance` | Marcaciones, faltas y tardanzas. |
| `Payroll` | Nómina, descuentos, horas extra y comisiones. |

### Construcción pendiente recomendada

- Crear interfaces en Application para empleados, asistencia y nómina.
- Implementar servicios de Infrastructure y controllers.
- Conectar comisiones con ventas POS.
- Agregar calendario laboral, turnos y reglas por país.

---

## 🚚 Sistemas logísticos pendientes: OMS, WMS y TMS

Estos sistemas deben construirse después de estabilizar ERP/POS porque dependen de inventario, pedidos y sucursales.

### Sistema 5 — OMS

| Módulo | Entidades sugeridas | Responsabilidad |
|---|---|---|
| Enrutamiento inteligente | `OmnichannelOrder`, `OrderRoute`, `FulfillmentAssignment` | Decide desde qué tienda/almacén atender. |
| Consolidación de canales | `SalesChannel`, `ChannelOrder`, `ChannelSyncLog` | Integra web, WhatsApp y marketplaces. |

### Sistema 6 — WMS

| Módulo | Entidades sugeridas | Responsabilidad |
|---|---|---|
| Operaciones de almacén | `PickingTask`, `PackingTask`, `StorageBin`, `WarehouseZone` | Control operativo dentro del almacén. |
| Distribución interna | `ReplenishmentOrder`, `InternalDispatch` | Reabastecimiento a sucursales. |

### Sistema 7 — TMS

| Módulo | Entidades sugeridas | Responsabilidad |
|---|---|---|
| Planificación de rutas | `Vehicle`, `RoutePlan`, `RouteStop` | Optimización por capacidad y distancia. |
| Seguimiento y entrega | `Delivery`, `ProofOfDelivery`, `GpsTrackPoint` | Última milla y confirmación digital. |

### Orden recomendado

1. OMS: crear pedido omnicanal y asignación de fulfillment.
2. WMS: crear picking/packing conectado a almacenes ERP.
3. TMS: crear despacho, ruta, tracking y prueba de entrega.

---

## 🧾 Sistemas comerciales y catálogo: PIM y SFA

### Sistema 8 — PIM

**Objetivo técnico:** catálogo único de productos enriquecidos.

| Entidad sugerida | Responsabilidad |
|---|---|
| `ProductContent` | Descripciones comerciales, atributos y SEO. |
| `ProductMedia` | Fotos, videos y documentos. |
| `ProductAttributeSet` | Plantillas de atributos por categoría. |
| `ChannelPublication` | Publicación por POS, ecommerce, app o marketplace. |

**Integración:** ERP conserva producto, costo y stock; PIM conserva contenido comercial y publicación.

### Sistema 9 — SFA

**Objetivo técnico:** venta en campo online/offline.

| Entidad sugerida | Responsabilidad |
|---|---|
| `FieldVisit` | Visita a cliente en ruta. |
| `RouteCustomer` | Cliente asignado a ruta comercial. |
| `PreOrder` | Pedido tomado en campo antes de confirmación. |
| `FieldCollection` | Cobranza registrada por vendedor. |

**Integración:** CRM aporta clientes, ERP aporta stock/precios, OMS procesa pedidos confirmados.

---

## 🎧 Sistema 10 — Help Desk

Actualmente existe `SupportTicket` dentro de CRM. Puede mantenerse ahí al inicio, pero debe separarse cuando soporte requiera SLA, colas, agentes y métricas propias.

### Entidades sugeridas al separarlo

| Entidad | Responsabilidad |
|---|---|
| `Ticket` | Caso de soporte o reclamo. |
| `TicketMessage` | Conversación y notas internas. |
| `TicketAssignment` | Agente, cola y prioridad. |
| `SlaPolicy` | Tiempos comprometidos por plan o severidad. |

### Construcción pendiente recomendada

- Definir si Help Desk será módulo CRM o sistema independiente.
- Agregar SLA por plan SaaS.
- Conectar tickets internos del SaaS con tickets postventa de clientes finales.

---

## 📊 Sistemas analíticos: Retail Analytics, Prevención y BI

Estos sistemas deben leer eventos y datos consolidados. No deben bloquear operaciones transaccionales.

### Sistema 11 — Retail Analytics

| Entidad sugerida | Responsabilidad |
|---|---|
| `StoreTrafficReading` | Conteo de personas por tienda/hora. |
| `HeatmapReading` | Zonas calientes/frías de tienda. |
| `ConversionMetric` | Visitas vs tickets POS. |

### Sistema 12 — Prevención de Pérdidas

| Entidad sugerida | Responsabilidad |
|---|---|
| `CycleCount` | Inventario cíclico por zona/categoría. |
| `ShrinkageCase` | Caso de pérdida o descuadre. |
| `SuspiciousTransactionAlert` | Alertas por anulaciones, devoluciones o ajustes atípicos. |

### Sistema 13 — BI

| Entidad sugerida | Responsabilidad |
|---|---|
| `ReportDefinition` | Definición de dashboard/reporte. |
| `MetricSnapshot` | Métricas precalculadas por fecha/tenant. |
| `DataExportJob` | Exportación programada. |

### Construcción recomendada

- Emitir eventos desde ERP, POS, CRM, OMS, WMS y TMS.
- Guardar datos operativos en PostgreSQL.
- Guardar logs/auditoría/eventos pesados en MongoDB.
- Preparar proyecciones de lectura para dashboards.
- Mantener BI como lectura agregada, sin modificar entidades transaccionales.

---

## 🔐 Seguridad, permisos y licencias

### Niveles de control

| Nivel | Qué controla | Ejemplo |
|---|---|---|
| Tenant | Si el cliente puede entrar | tenant activo/suspendido |
| Suscripción | Si el contrato está vigente | trial, active, expired |
| Sistema | Si aparece en el menú | `erp`, `pos`, `crm` |
| Módulo | Si puede usar una sección | `erp.inventory` |
| Submódulo | Si puede ejecutar una función | `erp.inventory.adjustments` |
| Permiso | Acción exacta del usuario | `erp.inventory.adjustments.approve` |
| Límite | Uso permitido por plan | 10 usuarios, 3 sucursales |

### Política por endpoint

Todo endpoint productivo debe declarar:

```
[Authorize]
[RequireSaasAccess("erp", "inventory", "products")]
[RequirePermission("erp.inventory.products.read")]
```

La validación de licencia responde si el tenant compró la función. La validación de permiso responde si el usuario puede ejecutar la acción.

---

## 🧱 Orden de construcción recomendado

### Fase 1 — Cimiento SaaS y ERP/POS

- Cerrar catálogo SaaS, planes, licencias, permisos y auditoría.
- Completar ERP inventario, compras, facturación y stock.
- Completar POS caja, pagos, anulaciones y cierre.
- Asegurar que todo endpoint pase por licencia + permiso.

### Fase 2 — CRM y RH

- Exponer controllers CRM.
- Completar clientes, oportunidades, pedidos comerciales y tickets.
- Completar empleados, asistencia, turnos, nómina y comisiones.
- Conectar comisiones RH con ventas POS.

### Fase 3 — OMS/WMS/TMS

- Crear pedidos omnicanal y asignación de fulfillment.
- Crear picking/packing y reposición desde almacén.
- Crear rutas, tracking y prueba de entrega.

### Fase 4 — PIM/SFA/Help Desk

- Separar contenido comercial de producto en PIM.
- Crear preventa móvil y cobranza en campo.
- Decidir separación final de Help Desk fuera de CRM.

### Fase 5 — Analytics/Prevención/BI

- Emitir eventos transaccionales.
- Crear proyecciones y snapshots.
- Construir dashboards por tenant, tienda, producto y canal.

---

## 🧪 Pruebas mínimas por sistema

Cada sistema nuevo debe entrar con estas pruebas base:

| Tipo | Qué debe validar |
|---|---|
| Domain | Reglas de negocio puras, estados inválidos y cálculos. |
| Application | Licencia requerida, permisos, orquestación y DTOs. |
| Infrastructure | Mapeos EF, filtros tenant, soft-delete y persistencia. |
| Api | Status codes, autorización y contrato request/response. |
| Integración | Flujos entre sistemas, por ejemplo POS → ERP → BI. |

### Casos obligatorios multitenant

- Un tenant no puede leer datos de otro tenant.
- Un usuario sin permiso no puede ejecutar la acción.
- Un tenant sin licencia no puede acceder al módulo.
- Una suscripción vencida bloquea operaciones críticas.
- Un cambio de licencia invalida cache y se refleja en la siguiente validación.

---

## 📌 Definición de terminado para un módulo

Un módulo se considera construido cuando cumple todo esto:

- Entidades y reglas principales en `Backend.Domain`.
- Interfaces y DTOs en `Backend.Application`.
- Implementación en `Backend.Infrastructure`.
- DbSets/configuraciones/migración o ajuste de esquema.
- Controller en `Backend-Api`.
- Validación de licencia SaaS.
- Validación de permisos por acción.
- Auditoría de acciones sensibles.
- Pruebas mínimas de dominio, aplicación e integración.
- Registro en catálogo SaaS con sistema, módulo, submódulos, rutas e icono.

---

## 🗺️ Próximos pasos concretos

1. Completar autorización declarativa con atributos `RequireSaasAccess` y `RequirePermission` en todos los controllers existentes.
2. Terminar POS caja/pagos porque es el flujo operativo más cercano al usuario final.
3. Exponer CRM por API para aprovechar entidades y servicios ya creados.
4. Completar HR con servicios y controllers.
5. Crear OMS como primer sistema nuevo dependiente de ERP/POS/CRM.
6. Crear WMS y TMS después de que OMS ya produzca órdenes de fulfillment.
7. Crear PIM antes de abrir integraciones ecommerce/marketplaces.
8. Dejar BI y Analytics para cuando existan eventos confiables.

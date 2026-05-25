# 🏗️ Arquitectura de Sistemas SaaS - 14 Sistemas

**Jerarquía:** SISTEMA (Macro) ➔ MÓDULO (Proceso) ➔ SUBMÓDULO (Función específica)

**Última actualización:** 24 de mayo de 2026

---

## 📊 Resumen

- **Total Sistemas:** 14
- **Plataforma:** ERP Omnicanal para Comercio Minorista
- **Modelo Comercial:** SaaS modular que vende sistemas, módulos y submódulos por planes, licencias y add-ons

---

## 0. 🧩 Plataforma SaaS Central ➔ Venta, Control y Gobierno del Producto

**Propósito:** Administrar el negocio SaaS que vende, activa, factura y controla el acceso a todos los sistemas, módulos y submódulos de la plataforma.

### Módulo 0.1: Catálogo Comercial de Sistemas
**Submódulos:**
- Registro de sistemas disponibles (ERP, POS, CRM, RH, OMS, WMS, TMS, PIM, SFA, Help Desk, Analytics, Prevención, BI)
- Registro de módulos y submódulos vendibles por sistema
- Versiones, estados y disponibilidad por mercado o país
- Paquetes comerciales: Starter, Professional, Enterprise y planes personalizados

### Módulo 0.2: Clientes, Tenants y Empresas
**Submódulos:**
- Alta de clientes SaaS (*tenants*)
- Empresas, sucursales y unidades de negocio por cliente
- Aislamiento de datos por tenant
- Configuración inicial por rubro, país, moneda e impuestos

### Módulo 0.3: Suscripciones, Planes y Licencias
**Submódulos:**
- Contratación de sistemas, módulos y submódulos
- Control de usuarios incluidos, usuarios adicionales y límites de uso
- Fechas de inicio, renovación, suspensión y cancelación
- Pruebas gratis, demos, upgrades, downgrades y add-ons

### Módulo 0.4: Facturación SaaS y Cobranza
**Submódulos:**
- Facturación recurrente mensual/anual
- Integración con pasarelas de pago
- Estados de pago: pendiente, pagado, vencido, suspendido
- Cupones, descuentos, promociones y notas de crédito

### Módulo 0.5: Aprovisionamiento y Activación
**Submódulos:**
- Creación automática del entorno del cliente
- Activación/desactivación de sistemas por suscripción
- Activación granular de módulos y submódulos
- Plantillas de configuración inicial por tipo de cliente

### Módulo 0.6: Seguridad, Roles y Permisos Globales
**Submódulos:**
- Usuarios administradores del SaaS
- Roles globales: Super Admin, Soporte, Ventas, Finanzas, Implementación
- Permisos por sistema, módulo, submódulo y acción
- Auditoría de accesos, cambios de planes y cambios de configuración

### Módulo 0.7: Monitoreo, Uso y Límites
**Submódulos:**
- Métricas de uso por tenant, sistema y módulo
- Límites por plan: usuarios, sucursales, documentos, API calls, almacenamiento
- Alertas de consumo, abuso o vencimiento
- Panel de salud del servicio por cliente

### Módulo 0.8: Backoffice Comercial y Soporte SaaS
**Submódulos:**
- Gestión de leads, demos y oportunidades SaaS
- Onboarding e implementación de nuevos clientes
- Tickets internos de soporte por tenant
- Historial comercial, contractual y operativo del cliente

### Cómo controla a los demás sistemas

- Define qué cliente puede usar cada sistema.
- Define qué módulos y submódulos están activos según el plan contratado.
- Centraliza usuarios, roles, permisos, auditoría y límites de uso.
- Suspende o reactiva servicios según estado de pago.
- Provee configuración base a ERP, POS, CRM, RH, OMS, WMS, TMS, PIM, SFA, Help Desk, Analytics, Prevención y BI.

### Flujo Comercial y Operativo del SaaS

1. **Venta:** el equipo comercial registra el cliente, selecciona plan, sistemas, módulos, submódulos, usuarios y sucursales.
2. **Contrato/Suscripción:** el sistema genera la suscripción con precio, ciclo de cobro, vigencia, límites y condiciones.
3. **Pago:** la plataforma valida el pago inicial o activa una prueba/demo con fecha de vencimiento.
4. **Aprovisionamiento:** se crea el tenant, empresa principal, administrador inicial y configuración base.
5. **Activación:** se habilitan solo los sistemas, módulos y submódulos contratados.
6. **Operación:** cada sistema consulta permisos, licencias y límites antes de permitir funciones críticas.
7. **Monitoreo:** se mide uso, consumo, errores, actividad y adopción por cliente.
8. **Renovación/Bloqueo:** si el cliente renueva, continúa activo; si vence o no paga, se restringe acceso según reglas comerciales.

### Reglas de Control

| Control | Qué valida | Acción |
|---------|------------|--------|
| Tenant activo | Cliente habilitado y no suspendido | Permite ingreso a la plataforma |
| Plan vigente | Suscripción dentro de fecha y estado pagado/demo | Mantiene sistemas activos |
| Licencia de sistema | Sistema contratado por el cliente | Muestra u oculta ERP, POS, CRM, etc. |
| Licencia de módulo | Módulo incluido en el plan o add-on | Permite ejecutar funciones del módulo |
| Permiso de usuario | Rol y permiso por acción | Autoriza crear, editar, aprobar, anular o ver |
| Límite de uso | Usuarios, sucursales, documentos, API calls, almacenamiento | Alerta, bloquea o solicita upgrade |
| Auditoría | Cambios sensibles y accesos críticos | Registra responsable, fecha, módulo y acción |

---

## 1. 📊 ERP (Enterprise Resource Planning) ➔ El Núcleo Central

**Propósito:** Integración de procesos financieros, contables y de inventario

### Módulo 1.1: Finanzas y Contabilidad (Económico)
**Submódulos:**
- Contabilidad general (Libro Diario/Mayor)
- Cuentas por Cobrar/Pagar
- Flujo de Caja (*Cash Flow*)
- Reporte de Utilidades y Pérdidas

### Módulo 1.2: Compras y Proveedores
**Submódulos:**
- Gestión de Órdenes de Compra
- Evaluación de Proveedores
- Costeo de Importación/Adquisición

### Módulo 1.3: Facturación Electrónica
**Submódulos:**
- Generación de comprobantes (Boletas/Facturas)
- Comunicación en tiempo real con SUNAT
- Gestión de Notas de Crédito/Débito

### Módulo 1.4: Inventario Central
**Submódulos:**
- Control de Kardex general
- Valoración de existencias (Costo promedio)
- Alertas de Stock Mínimo

---

## 2. 🛍️ POS (Point of Sale) ➔ El Sistema de las Tiendas Físicas

**Propósito:** Venta en punto de venta y operaciones de tienda

### Módulo 2.1: Caja y Atención
**Submódulos:**
- Escaneo de código de barras
- Apertura/Cierre y Arqueo de caja
- Integración con pasarelas de pago (Tarjetas, Yape/Plin)

### Módulo 2.2: Inventario Local
**Submódulos:**
- Stock físico en piso de venta
- Registro de mermas rápidas
- Recepción de mercadería del almacén central

---

## 3. 🎯 CRM (Customer Relationship Management) ➔ Gestión Comercial

**Propósito:** Relación y gestión de clientes

### Módulo 3.1: Ventas B2B / Distribución
**Submódulos:**
- Embudo de ventas (*Pipeline*)
- Historial de cotizaciones
- Recordatorios de seguimiento de asesores

### Módulo 3.2: Marketing Automático
**Submódulos:**
- Segmentación de clientes
- Envío masivo programado (Correos/WhatsApp)
- Gestión de cupones de fidelización

---

## 4. 👥 RH (Recursos Humanos) ➔ Control de Personal

**Propósito:** Gestión integral de recursos humanos

### Módulo 4.1: Nómina (Planillas)
**Submódulos:**
- Cálculo de sueldo básico y horas extra
- Cálculo de comisiones por ventas (conectado al POS)
- Descuentos de ley (AFP/ONP, gratificaciones, CTS)

### Módulo 4.2: Control de Asistencia en Campo
**Submódulos:**
- Marcación biométrica (huella/rostro)
- Gestión de turnos rotativos para tiendas
- Registro de faltas y tardanzas

---

## 5. 📦 OMS (Order Management System) ➔ Orquesta de Pedidos Omnicanal

**Propósito:** Gestión centralizada de pedidos desde múltiples canales

### Módulo 5.1: Enrutamiento Inteligente
**Submódulos:**
- Asignación de stock por cercanía (ej. despacho desde Trujillo o Lima)
- Gestión de flujos *Click & Collect* (Compra en web, recoge en tienda)

### Módulo 5.2: Consolidación de Canales
**Submódulos:**
- Integración con E-commerce
- API de WhatsApp Business
- Sincronización con Marketplaces (Mercado Libre, Falabella, etc.)

---

## 6. 🏭 WMS (Warehouse Management System) ➔ Logística de Almacenes

**Propósito:** Control de operaciones de almacén

### Módulo 6.1: Operaciones de Almacén
**Submódulos:**
- Mapeo de ubicaciones (Pasillo/Estante/Altura)
- Guía de rutas para operarios (*Picking* y *Packing*)

### Módulo 6.2: Distribución Interna
**Submódulos:**
- Gestión de transferencias entre almacenes
- Despacho de camiones de reabastecimiento para sucursales

---

## 7. 🚚 TMS (Transportation Management System) ➔ Última Milla y Despachos

**Propósito:** Control y optimización de transporte y entregas

### Módulo 7.1: Planificación de Rutas
**Submódulos:**
- Optimización de recorridos para transportistas
- Asignación de carga por capacidad de vehículo

### Módulo 7.2: Seguimiento y Entrega
**Submódulos:**
- Rastreo GPS en tiempo real
- Confirmación de entrega (Prueba de entrega digital/Firma del cliente)

---

## 8. 📋 PIM (Product Information Management) ➔ Catálogo Único de Productos

**Propósito:** Gestión centralizada de información de productos

### Módulo 8.1: Contenido de Producto
**Submódulos:**
- Ficha técnica y características (Talla, color, marca)
- Repositorio multimedia (Fotos/Videos de producto)

### Módulo 8.2: Sincronización
**Submódulos:**
- Actualización masiva de precios y datos hacia el POS, la Web y Apps móviles en simultáneo

---

## 9. 📱 SFA (Sales Force Automation) ➔ Venta en la Calle

**Propósito:** Automatización de ventas en campo

### Módulo 9.1: Módulo Móvil de Preventa
**Submódulos:**
- Toma de pedidos offline/online en ruta
- Consulta de stock central en tiempo real
- Registro de cobranzas de clientes morosos

---

## 10. 🎧 Help Desk ➔ Soporte y Postventa

**Propósito:** Gestión de tickets y soporte al cliente

### Módulo 10.1: Módulo de Tickets
**Submódulos:**
- Centralización de reclamos (Canal Omnicanal)
- Asignación automática a agentes de soporte
- Medición de tiempos de respuesta (SLA)

---

## 11. 📊 Retail Analytics ➔ Tráfico e Inteligencia de Tienda

**Propósito:** Análisis de comportamiento en tienda

### Módulo 11.1: Conteo de Personas
**Submódulos:**
- Sensores/Cámaras IA de ingreso
- Cálculo de tasa de conversión (Visitas vs. Tickets de POS)
- Mapas de calor en pasillos

---

## 12. 🔍 Prevención de Pérdidas ➔ Auditoría y Control de Stock

**Propósito:** Control y auditoría de inventario

### Módulo 12.1: Inventarios Cíclicos
**Submódulos:**
- Auditoría aleatoria diaria por categorías
- Alertas de descuadres sospechosos en POS (Anulaciones de boletas frecuentes)

---

## 13. 📈 BI (Business Intelligence) ➔ El Analista de Datos Superior

**Propósito:** Análisis e inteligencia de negocio (se alimenta de bases de datos de todos los anteriores)

### Submódulos / Dashboards:
- Panel de Rentabilidad Neta y Bruta
- Comparativa de Ventas entre Tiendas
- Análisis de productos de baja rotación (estancados)

---

## 🔄 Mapa de Flujo de Integración

```
┌──────────────────────────────────────────────────────┐
│      SISTEMA 0: PLATAFORMA SAAS CENTRAL              │
│  (Tenants, Planes, Suscripciones, Licencias, Pagos)  │
└──────────────────────────────────────────────────────┘
                        ↓
┌──────────────────────────────────────────────────────┐
│          ADMINISTRACIÓN CENTRAL Y SEGURIDAD          │
│     (Usuarios, Roles, Permisos, Auditoría, Logs)     │
└──────────────────────────────────────────────────────┘
                        ↓
┌──────────────────────────────────────────────────────┐
│    SISTEMA 1: ERP (Contabilidad, Inventario Core)   │
└──────────────────────────────────────────────────────┘
    ↓                    ↓                    ↓
┌─────────────────┐ ┌──────────────┐ ┌─────────────────┐
│  Sistema 2: POS │ │ Sistema 3: CRM   │  │ Sistema 4: RH   │
└─────────────────┘ └──────────────┘ └─────────────────┘
    ↓                    ↓                    ↓
┌─────────────────┐ ┌──────────────┐ ┌─────────────────┐
│ Sistema 5: OMS  │ │ Sistema 6: WMS   │  │ Sistema 7: TMS  │
└─────────────────┘ └──────────────┘ └─────────────────┘
    ↓                    ↓                    ↓
┌─────────────────┐ ┌──────────────┐ ┌─────────────────┐
│ Sistema 8: PIM  │ │Sistema 9: SFA│  │Sistema 10: Help │
└─────────────────┘ └──────────────┘ └─────────────────┘
                        ↓
┌──────────────────────────────────────────────────────┐
│ Sistema 11: Retail Analytics | Sistema 12: Pérdidas │
└──────────────────────────────────────────────────────┘
                        ↓
┌──────────────────────────────────────────────────────┐
│          Sistema 13: BI (Business Intelligence)      │
└──────────────────────────────────────────────────────┘
```

---

## 📌 Matriz de Dependencias y Flujos

| Sistema | Dependencias | Proveedores de Datos |
|---------|-------------|----------------------|
| **0. Plataforma SaaS Central** | Ninguna | Tenants, planes, licencias, permisos, pagos |
| **1. ERP** | SaaS Central, Admin Central | Todos los sistemas |
| **2. POS** | SaaS Central, ERP, Admin | Usuarios, Productos |
| **3. CRM** | SaaS Central, ERP, Admin | Clientes, Pedidos |
| **4. RH** | SaaS Central, ERP, POS, Admin | Empleados, Asistencia |
| **5. OMS** | SaaS Central, ERP, POS, CRM, Admin | Inventario, Clientes |
| **6. WMS** | SaaS Central, ERP, OMS, Admin | Almacenes, Stock |
| **7. TMS** | SaaS Central, WMS, OMS, Admin | Despachos, Entregas |
| **8. PIM** | SaaS Central, ERP, Admin | Catálogo, Precios |
| **9. SFA** | SaaS Central, CRM, POS, Admin | Clientes, Pedidos |
| **10. Help Desk** | SaaS Central, CRM, Admin | Clientes, Tickets |
| **11. Retail Analytics** | SaaS Central, POS, CRM, WMS, Admin | Transacciones, Tráfico |
| **12. Prevención** | SaaS Central, POS, WMS, ERP, Admin | Descuadres, Stock |
| **13. BI** | SaaS Central, TODOS | Consolidados todos |

---

## 🎯 Ciclo Operativo Diario

1. **Apertura de Tienda (AM)**
   - POS abre caja (Sistema 2)
   - RH registra asistencia (Sistema 4)
   - WMS prepara picking (Sistema 6)

2. **Operación (AM/PM)**
   - Ventas en POS (Sistema 2)
   - Pedidos omnicanal en OMS (Sistema 5)
   - Cobranzas en SFA (Sistema 9)
   - Soporte en Help Desk (Sistema 10)

3. **Logística (Tarde)**
   - Despachos en TMS (Sistema 7)
   - Seguimiento en tiempo real
   - Auditoría en Prevención (Sistema 12)

4. **Cierre (PM)**
   - Arqueo de caja POS (Sistema 2)
   - Conciliación en ERP (Sistema 1)
   - Nómina calcula en RH (Sistema 4)
   - Analytics genera reportes (Sistema 13)

---

## 📈 Roadmap de Implementación

**Fase 0 (SaaS Core):**
- Sistema 0: Plataforma SaaS Central
- Catálogo de sistemas, módulos y submódulos vendibles
- Tenants/clientes, planes, licencias, pagos y activación
- Roles globales, permisos, auditoría y límites de uso

**Fase 1 (MVP):**
- Sistema 1: ERP ✅
- Sistema 2: POS ✅
- Sistema 3: CRM ✅

**Fase 2 (Operaciones):**
- Sistema 4: RH
- Sistema 5: OMS
- Sistema 6: WMS
- Sistema 7: TMS

**Fase 3 (Omnicanal):**
- Sistema 8: PIM
- Sistema 9: SFA
- Sistema 10: Help Desk

**Fase 4 (Inteligencia):**
- Sistema 11: Retail Analytics
- Sistema 12: Prevención de Pérdidas
- Sistema 13: BI

---

*Documento de referencia para arquitectura de software y esquema de bases de datos*

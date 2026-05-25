Sí, tal como está ahora sirve como prototipo, pero para vender sistemas/módulos/submódulos a empresas hay que corregir la base del modelo.

**Diagnóstico Rápido**

El backend ya tiene una buena idea inicial:

- PostgreSQL: datos principales.
- MongoDB: auditoría/logs.
- Redis: cache.
- `Tenant`: empresa/cliente SaaS.
- `Module`: módulos disponibles.
- `TenantModule`: módulos activados por empresa.
- `Permission`: permisos por módulo.

Pero hay 4 problemas grandes:

1. `Module` no alcanza para representar **Sistema → Módulo → Submódulo**. Ahora solo existe una entidad plana en [Module.cs](</c:/Users/victorraul/Desktop/Saas/Backend-Saas/Backend.Domain/Common/Module.cs:3>).

2. `TenantModule` solo activa módulos, pero no controla sistemas ni submódulos. Está en [TenantModule.cs](</c:/Users/victorraul/Desktop/Saas/Backend-Saas/Backend.Domain/Common/TenantModule.cs:3>).

3. Los datos sensibles de facturación están metidos directo en `Tenant`: `ClaveSol`, `CertificadoPem`, `CertificadoPassword` en [Tenant.cs](</c:/Users/victorraul/Desktop/Saas/Backend-Saas/Backend.Domain/Common/Tenant.cs:23>). Eso no debe quedar así para producción.

4. El seed de facturación está mezclado con México: dice **SAT, CFDI, CSD, PAC** en [SeedData.cs](</c:/Users/victorraul/Desktop/Saas/Backend-Saas/Backend.Infrastructure/Persistence/PostgreSQL/SeedData.cs:67>), pero tu sistema parece Perú/SUNAT. Eso hay que corregir sí o sí.

**Cambios Necesarios**

1. Crear catálogo jerárquico vendible:

```text
SaasSystem
  ERP, POS, CRM, RH, OMS, WMS...

SaasModule
  Finanzas, Inventario, Facturación, Compras...

SaasSubModule
  Emitir factura, Nota de crédito, Kardex, Caja...
```

No conviene seguir usando solo `Module`, porque después no podrás vender ni bloquear granularmente.

2. Crear licencias por empresa:

```text
TenantSubscription
TenantSystemLicense
TenantModuleLicense
TenantSubModuleLicense
```

Así puedes decir:

```text
Empresa A tiene:
- ERP activo
- Facturación activa
- SUNAT activo
- Inventario activo
- RH bloqueado
```

3. Cambiar permisos para que apunten a sistema/módulo/submódulo:

```text
Permission
- Key: erp.facturacion.comprobantes.emitir
- SystemId
- ModuleId
- SubModuleId
- Action: view/create/update/delete/approve/cancel/export/configure
```

Ejemplo real:

```text
erp.facturacion.facturas.emitir
erp.facturacion.facturas.anular
erp.facturacion.notas-credito.crear
erp.inventario.kardex.ver
pos.caja.aperturar
```

4. Separar datos legales de empresa:

Crear algo como:

```text
TenantCompanyProfile
```

Con:

```text
RUC
Razón social
Nombre comercial
Dirección fiscal
Ubigeo
Departamento
Provincia
Distrito
Correo de facturación
Teléfono
Representante legal
Tipo de contribuyente
Régimen tributario
Estado SUNAT
Condición SUNAT
```

5. Separar configuración SUNAT/facturación electrónica:

Crear:

```text
TenantTaxConfiguration
TenantSunatCredential
TenantCertificate
```

Datos mínimos que faltan o deben organizarse:

```text
Usuario SOL
Clave SOL
Certificado digital .pem/.pfx
Clave del certificado
RUC emisor
Ambiente: beta/producción
Modo de envío: directo SUNAT / OSE / PSE
Proveedor OSE/PSE
URL API proveedor
Token proveedor
Series de comprobantes
Correlativos
IGV por defecto
Moneda por defecto
```

Importante: `ClaveSol`, clave del certificado y tokens no deben guardarse en texto plano. Deben ir cifrados o en un secret manager.

6. Agregar series y correlativos:

Sin esto, facturación no funciona bien.

```text
InvoiceSeries
- TenantId
- TipoComprobante: Factura, Boleta, NotaCredito, NotaDebito, Guia
- Serie: F001, B001, FC01
- CorrelativoActual
- SucursalId
- Activo
```

7. Agregar sucursales de empresa:

Ahora `Tenant` representa la empresa, pero para facturación/POS necesitas:

```text
TenantBranch
- Dirección
- Ubigeo
- Código establecimiento SUNAT
- Serie por comprobante
- Almacén relacionado
```

8. Mantener las 3 bases, pero con responsabilidades claras:

```text
PostgreSQL
- Tenants
- Empresas
- Usuarios
- Roles
- Permisos
- Sistemas/módulos/submódulos
- Suscripciones/licencias
- Facturas
- Inventario
- Contabilidad

MongoDB
- Auditoría
- Logs de emisión SUNAT
- Payloads XML/JSON enviados
- Respuestas CDR
- Errores técnicos
- Historial de eventos pesados

Redis
- Cache de permisos
- Cache de módulos activos por tenant
- Sesiones/token blacklist
- Rate limits
- Estado rápido de licencia
```

**Prioridad Recomendada**

1. Corregir catálogo: `System`, `Module`, `SubModule`.
2. Corregir licenciamiento por tenant.
3. Corregir permisos granulares.
4. Separar `Tenant` de datos SUNAT/certificados.
5. Crear configuración real de facturación Perú/SUNAT.
6. Cambiar seed de SAT/CFDI a SUNAT.
7. Agregar series, correlativos y sucursales.
8. Cachear permisos/licencias en Redis.

Mi recomendación: no sigas creciendo encima de `Module` como está. Primero hay que arreglar esa base, porque ese modelo será el que controle todo lo que se vende, se activa, se bloquea y se factura.
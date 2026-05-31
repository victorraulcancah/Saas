# Prompt de Arquitectura para SaaS Empresarial Modular Multiempresa

## Contexto del Proyecto

Desarrollar una plataforma SaaS empresarial multiempresa (Multi-Tenant) donde múltiples sistemas empresariales funcionen dentro de un único ecosistema centralizado.

El objetivo es que cada empresa pueda contratar diferentes sistemas mediante suscripción y administrarlos desde una sola plataforma, utilizando una estructura compartida de usuarios, roles y permisos.

La solución debe estar diseñada para escalar a futuro sin necesidad de rediseñar la arquitectura principal.

---

# Visión General

La plataforma permitirá que una empresa:

* Se registre en el SaaS.
* Contrate uno o varios sistemas.
* Administre usuarios.
* Cree roles personalizados.
* Asigne permisos.
* Controle accesos.
* Utilice múltiples sistemas desde una misma identidad.

Los sistemas iniciales serán:

* ERP
* RH / RRHH
* TMS

En el futuro podrán agregarse nuevos sistemas sin modificar la arquitectura principal.

---

# Principio Fundamental

La plataforma NO debe crear sistemas independientes de usuarios, roles y permisos por cada módulo.

Debe existir un único núcleo centralizado responsable de:

* Empresas
* Usuarios
* Roles
* Permisos
* Autenticación
* Autorización
* Suscripciones
* Auditoría

Todos los sistemas deberán utilizar este núcleo compartido.

---

# Diferenciación Importante

## Suscripciones

Las suscripciones determinan qué sistemas están disponibles para una empresa.

Ejemplo:

Empresa A:

* ERP = Activo
* RH = Activo
* TMS = Inactivo

Empresa B:

* ERP = Activo
* RH = Inactivo
* TMS = Activo

Una empresa no puede acceder a un sistema que no tenga contratado.

---

## Permisos

Los permisos NO controlan qué sistemas compra una empresa.

Los permisos controlan qué acciones pueden realizar los usuarios dentro de los sistemas contratados.

Ejemplo:

Permisos ERP:

* erp.products.view
* erp.products.create
* erp.products.edit
* erp.inventory.adjust

Permisos RH:

* rh.employee.view
* rh.employee.create
* rh.payroll.approve

Permisos TMS:

* tms.trip.view
* tms.trip.create

Los permisos serán definidos globalmente por la plataforma.

Las empresas podrán:

* Crear roles.
* Asignar permisos a roles.
* Asignar roles a usuarios.

No se crearán permisos personalizados por empresa.

---

# Problema Arquitectónico a Resolver

Existe un problema importante relacionado con la compartición de información entre sistemas.

Ejemplo:

* ERP puede generar información utilizada por RH.
* RH puede utilizar empleados compartidos.
* TMS puede utilizar empleados como conductores.
* ERP puede relacionarse con clientes utilizados por otros sistemas.

Sin embargo, una empresa podría no tener contratado uno de esos sistemas.

Ejemplo:

Empresa:

* ERP = Activo
* RH = Inactivo

Problema:

ERP genera información que conceptualmente podría utilizar RH.

Pero RH no está contratado.

Por lo tanto:

* No deben crearse registros en tablas de RH.
* No deben ejecutarse procesos de RH.
* No deben almacenarse datos pertenecientes a RH.
* No deben generarse dependencias obligatorias hacia RH.

La arquitectura debe impedir que un sistema almacene información directamente en estructuras de otro sistema cuando este no esté habilitado por suscripción.

---

# Restricción de Dominio

Cada sistema es propietario de sus propios datos.

ERP es propietario de:

* Productos
* Inventario
* Compras
* Ventas

RH es propietario de:

* Contratos
* Nómina
* Vacaciones
* Beneficios

TMS es propietario de:

* Vehículos
* Viajes
* Rutas
* Conductores

Ningún sistema puede escribir directamente en los datos internos de otro sistema.

---

# Entidades Compartidas

Existen entidades que deben ser compartidas por todo el ecosistema.

Estas entidades pertenecerán al Core.

Ejemplos:

* Empresas
* Usuarios
* Roles
* Permisos
* Empleados
* Sucursales
* Departamentos
* Centros de costo

Todos los sistemas podrán consumir estas entidades compartidas.

---

# Problema de Diseño de Datos

La solución utilizará una única base de datos.

Sin embargo, se deben evitar los siguientes problemas:

## Problema 1: Acoplamiento entre módulos

Un módulo no debe depender físicamente de tablas internas de otro módulo.

Mala práctica:

ERP → insertar directamente en nómina RH.

---

## Problema 2: Datos de sistemas no contratados

No deben almacenarse registros pertenecientes a sistemas que la empresa no tiene suscritos.

Ejemplo:

Si RH está desactivado:

* No crear nóminas.
* No crear vacaciones.
* No crear contratos RH.

---

## Problema 3: Crecimiento descontrolado de dependencias

A medida que se agreguen sistemas:

* CRM
* POS
* WMS
* E-Commerce
* BI

La complejidad de relaciones no debe aumentar exponencialmente.

La arquitectura debe mantener límites claros entre dominios.

---

# Arquitectura Deseada

Se recomienda una arquitectura de Monolito Modular.

NO utilizar microservicios en esta etapa.

Razones:

* Menor complejidad.
* Menor costo operativo.
* Menor tiempo de desarrollo.
* Más fácil mantenimiento.
* Equipo pequeño o mediano.

Sin embargo, los módulos deben diseñarse con límites claros para permitir una futura migración a microservicios si fuese necesario.

---

# Flujo de Validación de Acceso

Antes de ejecutar cualquier acción:

1. Validar empresa.
2. Validar tenant.
3. Validar suscripción activa del sistema.
4. Validar usuario.
5. Validar rol.
6. Validar permiso.
7. Ejecutar operación.

---

# Objetivo Técnico

Diseñar una plataforma SaaS empresarial modular y multiempresa que permita:

* Compartir identidad y seguridad.
* Centralizar usuarios, roles y permisos.
* Gestionar sistemas mediante suscripción.
* Evitar duplicación de estructuras.
* Mantener independencia de dominios.
* Evitar almacenamiento de datos en sistemas no contratados.
* Escalar a nuevos sistemas empresariales sin rediseñar la plataforma.
* Mantener una única experiencia de administración para todas las empresas.

Este prompt ya incorpora el problema real que descubriste: **no es solo un tema de permisos**, sino de **propiedad de datos entre módulos y evitar que un módulo genere o almacene información de otro módulo cuando ese sistema no está contratado por la empresa**. Ese es uno de los puntos más delicados del diseño de un SaaS modular.


Sí. La solución que discutíamos no era microservicios ni otra base de datos.

La solución era **propiedad de dominio (ownership de datos)**.

## Regla principal

Cada sistema es dueño de sus propias tablas.

```text
CORE
 ├─ Users
 ├─ Roles
 ├─ Permissions
 ├─ Employees
 └─ Branches

ERP
 ├─ Products
 ├─ Inventory
 ├─ Sales
 └─ Purchases

RH
 ├─ Payrolls
 ├─ Vacations
 └─ Contracts

TMS
 ├─ Trips
 ├─ Routes
 └─ Vehicles
```

---

## Lo que NO debe pasar

Si ERP hace esto:

```csharp
_payrollRepository.Create(...)
```

o

```csharp
_context.Payrolls.Add(...)
```

entonces ERP está escribiendo directamente en RH.

Eso rompe el aislamiento entre módulos.

---

## Lo que SÍ debe pasar

ERP guarda únicamente sus datos:

```csharp
_context.Sales.Add(...)
```

Después puede generar un evento interno:

```text
SaleCreated
```

o

```text
CommissionEligible
```

Y RH decide si lo procesa.

---

## ¿Qué pasa si RH no está contratado?

Aquí entra la validación de suscripción.

Ejemplo:

```text
Empresa A

ERP ✓
RH ✗
TMS ✗
```

ERP crea una venta.

```text
SaleCreated
```

RH recibe el evento.

Antes de hacer nada:

```text
¿RH está suscrito?
```

Resultado:

```text
NO
```

Entonces:

```text
Ignorar evento
No crear nómina
No crear comisión
No guardar nada en RH
```

---

## Solución aún más simple

Si todavía no tienes eventos ni bus interno:

Haz una validación antes de ejecutar lógica de otro módulo.

Ejemplo:

```csharp
if (!subscriptionService.HasModule(tenantId, "HR"))
{
    return;
}
```

Entonces nunca llegas a:

```csharp
_payrollRepository.Add(...)
```

---

## El problema de fondo

Muchas veces se piensa:

```text
ERP necesita RH
```

cuando realmente es:

```text
ERP produce información

RH consume información
```

Son cosas distintas.

ERP no debe conocer tablas RH.

RH puede conocer datos ERP si los necesita.

---

## La mejor regla para tu proyecto

Cada módulo puede:

✅ Leer entidades compartidas del Core.

✅ Leer datos públicos de otros módulos mediante servicios/interfaces.

✅ Generar eventos.

❌ Insertar directamente en tablas de otro módulo.

❌ Modificar tablas de otro módulo.

❌ Eliminar registros de otro módulo.

Si aplicas esa regla, entonces aunque una empresa no tenga RH contratado, jamás se insertarán datos en RH porque ningún módulo tendrá permiso arquitectónico para escribir allí directamente. El único que puede escribir en RH es RH mismo, y RH primero valida que la suscripción esté activa.

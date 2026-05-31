# Backend.SharedKernel

Biblioteca comun minima para todo el backend.

Esta capa contiene tipos transversales que pueden usar Core, ERP, CRM, RH, POS, WMS, TMS y futuros sistemas sin depender de infraestructura ni de un modulo especifico.

## Si pertenece aqui

- Entidades base.
- Interfaces transversales como tenant y soft delete.
- Value objects globales.
- Resultados comunes.
- Eventos de dominio base.

## No pertenece aqui

- Reglas de ERP, RH, CRM, POS, WMS o TMS.
- Servicios de base de datos.
- DTOs de API.
- Controladores.
- Configuracion de Entity Framework.
- Logica de suscripciones, licencias o permisos especifica del SaaS.

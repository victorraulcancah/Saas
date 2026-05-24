# Arquitectura Frontend

## Stack

- **Framework**: Next.js 15 (App Router) con Turbopack
- **Lenguaje**: TypeScript
- **UI Library**: Ant Design 5 (antd) con temas vía `ConfigProvider`
- **Estilos**: Tailwind CSS v4 (PostCSS, `@import 'tailwindcss'`)
- **Estado & Datos**: TanStack Query (React Query) para cache/servidor, Zustand (stores) para estado local
- **Iconos**: `react-icons` (Fa)
- **Autenticación**: Contexto propio (`AuthProvider` en `lib/auth-context`)
- **Tiempo real**: Firebase (FCM) + Laravel Echo
- **Notificaciones push**: Firebase Cloud Messaging
- **QR/Barcode**: jsrsasign
- **Tablas**: Ant Design Table, AG Grid

---

## Estructura de Carpetas

```
ferreteria2/
├── app/                            ← App Router (páginas y layouts)
│   ├── layout.tsx                  ← Root layout (AuthProvider, AntdRegistry, ConfigProvider, Providers)
│   ├── providers.tsx               ← TanStack Query + Firebase/Echo/Birthday initializers
│   ├── page.tsx                    ← Login
│   │
│   ├── ui/                         ← Módulo protegido (requiere auth)
│   │   ├── layout.tsx              ← Layout protegido (splash + init-store + fondo gradiente)
│   │   ├── page.tsx                ← Home (TopNav + Logo + BottomNav)
│   │   │
│   │   ├── _components/            ← Componentes compartidos del módulo UI
│   │   │   └── nav/                ← top-nav.tsx, bottom-nav.tsx
│   │   │
│   │   ├── configuracion/          ← Módulo Configuración
│   │   ├── facturacion-electronica/← Módulo Facturación Electrónica
│   │   ├── gestion-comercial-e-inventario/ ← Módulo Comercial/Inventario
│   │   ├── gestion-contable-y-financiera/ ← Módulo Contable/Financiero
│   │   ├── reportes/              ← Reportes
│   │   └── solicitudes-autorizacion/
│   │
│   ├── _components/                ← Componentes globales compartidos
│   │   ├── calendar/               ← Calendario de entregas
│   │   ├── cards/                  ← CardDashboard
│   │   ├── containers/             ← ContenedorGeneral
│   │   ├── docs/                   ← Componentes de documentos PDF (tablas, headers, tickets)
│   │   ├── filters/                ← Filtros (filter-date-range-fields)
│   │   ├── form/                   ← Formularios: inputs, selects, buttons, checkbox, fechas
│   │   ├── modals/                 ← Modales: búsqueda de productos, choferes, vehículos
│   │   ├── nav/                    ← BaseNav, DropdownUser, ButtonNav
│   │   ├── others/                 ← DataLoader, ProgressiveLoader, TituloModulos
│   │   └── tables/                 ← Tablas: búsqueda de productos, comprobantes
│   │
│   ├── _lib/                       ← Librerías específicas de la app
│   │   ├── queryKeys.ts            ← Claves de TanStack Query
│   │   └── tipos-ingresos-salidas.ts
│   │
│   ├── _stores/                    ← Zustand stores
│   ├── _types/                     ← TypeScript declarations
│   ├── _utils/                     ← Utilidades pequeñas
│   └── api/                        ← API Routes de Next.js
│
├── lib/                            ← Lógica compartida fuera del App Router
│   ├── api/                        ← 66 archivos: llamadas HTTP al backend
│   │   ├── plantilla-impresion.ts  ← Tipos y resolvers de estilos de PDF
│   │   ├── empresa.ts, venta.ts, producto.ts, …
│   │   └── index.ts (api.ts)       ← apiRequest() base
│   │
│   ├── navigation/                 ← Sistema de navegación modular
│   │   ├── index.ts                ← Hook useNavigation() que combina todos los módulos
│   │   ├── modules.json            ← Config JSON de módulos y rutas
│   │   └── module-navs/            ← Navs de cada módulo (facturacion.ts, inventario.ts, etc.)
│   │
│   ├── firebase/                   ← FCM + notificaciones push
│   └── utils/                      ← Utilidades generales
│
├── components/                     ← shadcn/ui + componentes de terceros
│   ├── magicui/                    ← RainbowButton y efectos
│   ├── notifications/              ← NotificationInitializer
│   ├── realtime/                   ← RealtimeProvider (Laravel Echo)
│   ├── autorizaciones/             ← CampanitaAutorizaciones
│   ├── birthday/                   ← BirthdayAlert
│   └── ui/                         ← shadcn/ui primitives
│
└── package.json
```

---

## Organización de Estilos

### Tailwind CSS v4

El proyecto usa **Tailwind CSS v4** con el nuevo sistema `@theme`. No hay `tailwind.config.js`.

```css
/* app/globals.css */
@import 'tailwindcss';
@import 'tailwindcss-animated';
@import 'tw-animate-css';

@theme inline {
  --color-primary: var(--primary);
  --color-background: var(--background);
  /* … más variables CSS → Tailwind */
}

:root {
  --primary: oklch(0.208 0.042 265.755);
  /* … colores en oklch */
}
```

Todas las clases utilitarias se usan directamente en JSX:
```tsx
<div className="flex items-center justify-between bg-white rounded-lg shadow-sm p-4" />
```

### Animaciones

Se usan tres fuentes de animaciones:
1. **`tailwindcss-animated`** — clases como `animate-fade`, `animate-fade-down`, `animate-ease-in-out`, `animate-delay-[250ms]`
2. **`tw-animate-css`** — animaciones compatibles con Tailwind v4
3. **CSS keyframes personalizados** en `globals.css` (ej. `slideUp`, `rainbow`)

### Ant Design Theme

El tema de Ant Design se configura desde `layout.tsx`:
```tsx
<ConfigProvider
  theme={{ token: { fontSize: 13 } }}
  locale={esES}
>
```

Cada componente de Ant Design usa tokens CSS (color primary, border, etc.) que se heredan del root.

### Estilos en línea para PDF y Preview

Cuando se renderizan documentos PDF (previews de plantilla-impresión), se usa **inline styles** con objetos `React.CSSProperties` porque:
- El preview debe ser fiel al PDF generado en el backend
- Los estilos se resuelven dinámicamente desde `resolverEstilos()`
- La función `bloqueACSS()` convierte un `EstiloBloqueResuelto` en `CSSProperties`

---

## Sistema de Navegación

### Estructura

```
lib/navigation/
├── index.ts              ← Hook useNavigation() que unifica todos los módulos
├── modules.json          ← Metadatos: nombre, icono, ruta, orden
└── module-navs/          ← Un archivo por módulo con sub-rutas
    ├── facturacion.ts
    ├── inventario.ts
    ├── finanzas.ts
    ├── configuracion.ts
    ├── reportes.ts
    └── …
```

### Funcionamiento

`useNavigation()` carga `modules.json` y los módulos de navegación, y devuelve una estructura jerárquica que los componentes `TopNav` / `BottomNav` / `Drawer` renderizan.

### Componentes de navegación

| Componente | Ruta | Propósito |
|---|---|---|
| `BaseNav` | `app/_components/nav/base-nav.tsx` | Barra superior responsiva con hamburguesa (móvil) y menú horizontal (desktop), campanita y dropdown de usuario |
| `TopNav` | `app/ui/_components/nav/top-nav.tsx` | Nav superior del módulo UI |
| `BottomNav` | `app/ui/_components/nav/bottom-nav.tsx` | Nav inferior del módulo UI (móvil) |
| `ButtonNav` | `app/_components/nav/button-nav.tsx` | Botón individual de navegación |
| `DropdownUser` | `app/_components/nav/dropdown-user.tsx` | Dropdown de usuario (cerrar sesión, perfil) |

---

## Sistema de Módulos

Cada módulo grande es una carpeta dentro de `app/ui/` con su propio `layout.tsx`:

| Módulo | Ruta |
|---|---|
| Facturación Electrónica | `app/ui/facturacion-electronica/` |
| Gestión Comercial e Inventario | `app/ui/gestion-comercial-e-inventario/` |
| Gestión Contable y Financiera | `app/ui/gestion-contable-y-financiera/` |
| Configuración | `app/ui/configuracion/` |
| Reportes | `app/ui/reportes/` |
| Solicitudes de Autorización | `app/ui/solicitudes-autorizacion/` |

Cada módulo contiene sus propias páginas y un directorio `_components/` con subcomponentes específicos.

---

## API Layer

### `lib/api/api.ts` — `apiRequest()`

Función base que envuelve `fetch()` con:
- Headers automáticos (Authorization, Content-Type)
- Manejo de errores HTTP
- Tipado genérico `<ApiResponse<T>>`

### `lib/api/*.ts` — Módulos de API

66 archivos, uno por recurso. Cada uno exporta un objeto con métodos (`list`, `getById`, `create`, `update`, `delete`).

Ejemplo:
```ts
// lib/api/empresa.ts
export const empresaApi = {
  getById: (id: number) => apiRequest<EmpresaResponse>(`/empresas/${id}`),
  update: (payload: EmpresaPayload) => apiRequest('/empresas', { method: 'POST', body: JSON.stringify(payload) }),
}
```

---

## Manejo de Estado

| Tipo | Herramienta | Dónde se usa |
|---|---|---|
| Estado del servidor | TanStack Query | Todas las consultas a API (useQuery, useMutation) |
| Estado de auth | React Context | `AuthProvider` en `lib/auth-context` |
| Estado local UI | useState / useReducer | Dentro de componentes |
| Estado global app | Zustand | `app/_stores/`: modal configuraciones, producto agregado a compra |

TanStack Query está configurado con:
- `staleTime: 5 min`
- `gcTime: 10 min`
- `retry: 2` (excepto errores de permisos)
- `refetchOnMount: always`
- `refetchOnWindowFocus: false`

---

## Convenciones

### Nombrado de archivos
- **Componentes**: kebab-case (`base-nav.tsx`, `contenedor-general.tsx`)
- **APIs**: kebab-case (`plantilla-impresion.ts`, `guia-remision.ts`)
- **Tipos**: camelCase (`queryKeys.ts`, `tipos-ingresos-salidas.ts`)

### Organización de componentes
- **Globales**: `app/_components/` — reutilizables en toda la app
- **Por módulo**: `app/ui/<modulo>/_components/` — específicos de ese módulo
- **Subdirectorios por tipo**: `form/`, `tables/`, `modals/`, `nav/`, `calendar/`, `cards/`, `docs/`, `filters/`, `containers/`, `others/`

### Componentes de formulario

Los formularios siguen una estructura modular:
- `inputs/` — `input-base.tsx`, `textarea-base.tsx`, `quill-editor.tsx`
- `selects/` — `select-base.tsx` + selects especializados (clientes, productos, almacenes, etc.)
- `buttons/` — botones para crear recursos in-line
- `checkbox/` — `checkbox-base.tsx`
- `fechas/` — `date-picker-base.tsx`, `range-picker-base.tsx`, `year-picker.tsx`

---

## Flujo de Datos General

```
Login → AuthProvider guarda token
  │
  ▼
/ui (layout protegido) → InitStore + TanStack Query
  │
  ├── TopNav / BaseNav ← useNavigation() + CampanitaAutorizaciones
  │
  ├── Página específica (ej: /ui/facturacion-electronica/mis-ventas)
  │     │
  │     ├── useQuery() → apiRequest() → backend Laravel
  │     ├── usuario interactúa → useMutation() → POST/PUT/DELETE
  │     └── queryClient.invalidateQueries() → refresca datos
  │
  └── Tiempo real ← RealtimeProvider (Laravel Echo) + Firebase FCM
        → notificaciones push y actualizaciones en vivo
```

---

## Dependencias Principales

```json
{
  "next": "^15.x",
  "antd": "^5.26",
  "@tanstack/react-query": "^5.83",
  "tailwindcss": "^4.x",
  "zustand": "^x",
  "firebase": "^12.8",
  "ag-grid-react": "^34.0",
  "date-fns": "^4.1",
  "react-icons": "^x",
  "clsx": "^2.1",
  "class-variance-authority": "^0.7"
}
```
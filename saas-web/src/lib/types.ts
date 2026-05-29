export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  email: string;
  role: string;
  tenantId: string | null;
}

export interface TenantResponse {
  id: string;
  name: string;
  slug: string;
  ruc: string;
  razonSocial: string;
  nombreComercial: string | null;
  email: string | null;
  emailFacturacion: string | null;
  phone: string | null;
  telefonoSecundario: string | null;
  address: string | null;
  direccionFiscal: string | null;
  ubigeo: string | null;
  departamento: string | null;
  provincia: string | null;
  distrito: string | null;
  website: string | null;
  logoBase64: string | null;
  isActive: boolean;
  subscriptionPlan: string | null;
  createdAt: string;
}

export interface CreateTenantRequest {
  name: string;
  slug: string;
  ruc: string;
  razonSocial: string;
  nombreComercial?: string | null;
  email?: string | null;
  emailFacturacion?: string | null;
  phone?: string | null;
  telefonoSecundario?: string | null;
  address?: string | null;
  direccionFiscal?: string | null;
  ubigeo?: string | null;
  departamento?: string | null;
  provincia?: string | null;
  distrito?: string | null;
  website?: string | null;
  logoBase64?: string | null;
  claveSol?: string | null;
  certificadoPem?: string | null;
  certificadoPassword?: string | null;
  subscriptionPlan?: string | null;
}

export interface UserResponse {
  id: string;
  email: string;
  firstName: string | null;
  lastName: string | null;
  isActive: boolean;
}

export interface CreateUserRequest {
  email: string;
  password: string;
  firstName?: string | null;
  lastName?: string | null;
  roleIds?: string[] | null;
}

export interface RoleResponse {
  id: string;
  name: string;
  description: string | null;
  createdAt: string;
}

export interface CreateRoleRequest {
  name: string;
  description?: string | null;
  permissionIds?: string[] | null;
}

export interface SaasSystemResponse {
  id: string;
  name: string;
  key: string;
  description: string | null;
  icon: string | null;
  basePath: string | null;
  isActive: boolean;
  modules: SaasModuleResponse[];
}

export interface SaasModuleResponse {
  id: string;
  name: string;
  key: string;
  description: string | null;
  icon: string | null;
  basePath: string | null;
  isActive: boolean;
  subModules: SaasSubModuleResponse[];
}

export interface SaasSubModuleResponse {
  id: string;
  name: string;
  key: string;
  description: string | null;
  routePath: string | null;
  isActive: boolean;
}

export interface CreateSaasSystemRequest {
  name: string;
  key: string;
  description?: string | null;
  icon?: string | null;
  basePath?: string | null;
}

export interface CreateSaasModuleRequest {
  systemId: string;
  name: string;
  key: string;
  description?: string | null;
  icon?: string | null;
  basePath?: string | null;
}

export interface CreateSaasSubModuleRequest {
  moduleId: string;
  name: string;
  key: string;
  description?: string | null;
  routePath?: string | null;
}

export interface SaasPlanResponse {
  id: string;
  name: string;
  key: string;
  description: string | null;
  price: number;
  currency: string;
  billingCycle: string;
  maxUsers: number;
  maxBranches: number;
  maxDocumentsPerMonth: number;
}

export interface AssignTenantPlanRequest {
  planId: string;
  status?: string | null;
  trialEndsAt?: string | null;
  currentPeriodEndsAt?: string | null;
}

export interface TenantSubscriptionResponse {
  id: string;
  tenantId: string;
  planId: string;
  status: string;
  startsAt: string;
  trialEndsAt: string | null;
  currentPeriodEndsAt: string | null;
}

export interface EnableTenantLicenseRequest {
  source?: string | null;
  expiresAt?: string | null;
}

export interface CategoryResponse {
  id: string;
  name: string;
  description: string | null;
  isActive: boolean;
}

export interface ProductResponse {
  id: string;
  name: string;
  sku: string;
  price: number;
  stock: number;
  categoryId: string;
  categoryName?: string;
}

export interface WarehouseResponse {
  id: string;
  name: string;
  address: string | null;
  isActive: boolean;
}

export interface SupplierResponse {
  id: string;
  name: string;
  ruc: string;
  email: string | null;
  phone: string | null;
  isActive: boolean;
}

export interface InvoiceResponse {
  id: string;
  serie: string;
  correlativo: string;
  clientName: string;
  total: number;
  state: string;
  createdAt: string;
}
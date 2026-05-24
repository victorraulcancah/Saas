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

export interface ModuleResponse {
  id: string;
  name: string;
  key: string;
  description: string | null;
  isEnabled: boolean;
}

export interface UserResponse {
  id: string;
  email: string;
  firstName: string | null;
  lastName: string | null;
  isActive: boolean;
}

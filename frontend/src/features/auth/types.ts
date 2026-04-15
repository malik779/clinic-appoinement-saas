export interface AuthTokens {
  accessToken: string;
  expiresAtUtc: string;
}

export interface AuthUser {
  userId: string;
  email: string;
  tenantId: string;
  roles: string[];
  tokens: AuthTokens;
}

export interface LoginRequest {
  tenantId: string;
  email: string;
  password: string;
}

export type LoginResponse = AuthUser;

export interface RegisterTenantPayload {
  clinicName: string;
  subdomain: string;
  adminEmail: string;
  adminFullName: string;
  adminPassword: string;
  planCode: string;
}

export interface RegisterTenantResponse {
  tenantId: string;
}

export interface RegisterUserPayload {
  email: string;
  password: string;
  fullName: string;
  role: "TenantAdmin" | "Staff";
}

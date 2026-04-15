import { apiGet, apiPost } from "../../shared/api/api";
import type {
  LoginRequest,
  LoginResponse,
  RegisterTenantPayload,
  RegisterTenantResponse,
  RegisterUserPayload,
} from "./types";

export const authService = {
  registerTenant: (payload: RegisterTenantPayload) =>
    apiPost<RegisterTenantResponse, RegisterTenantPayload>("/tenants/register", payload),
  registerUser: (payload: RegisterUserPayload) =>
    apiPost<{ userId: string }, RegisterUserPayload>("/auth/register", payload),
  login: (payload: LoginRequest, tenantId: string) => {
    localStorage.setItem("cms.tenantId", tenantId);
    return apiPost<LoginResponse, Omit<LoginRequest, "tenantId">>("/auth/login", {
      email: payload.email,
      password: payload.password,
    });
  },
  getTenantInfo: () => apiGet<unknown>("/tenants/me"),
};

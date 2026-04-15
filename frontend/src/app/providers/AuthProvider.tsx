import {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useState,
  type ReactNode,
} from "react";
import { authService } from "../../features/auth/services";
import type { LoginRequest, LoginResponse } from "../../features/auth/types";

type AuthContextValue = {
  token: string | null;
  tenantId: string | null;
  email: string | null;
  roles: string[];
  isAuthenticated: boolean;
  login: (payload: LoginRequest) => Promise<void>;
  logout: () => void;
};

export type { AuthContextValue };

const AUTH_STORAGE_KEY = "clinic-saas-auth";

type StoredAuth = {
  token: string;
  tenantId: string;
  email: string;
  roles: string[];
};

const AuthContext = createContext<AuthContextValue | null>(null);

function toStoredAuth(response: LoginResponse): StoredAuth {
  return {
    token: response.tokens.accessToken,
    tenantId: response.tenantId,
    email: response.email,
    roles: response.roles,
  };
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const [auth, setAuth] = useState<StoredAuth | null>(null);

  useEffect(() => {
    const raw = localStorage.getItem(AUTH_STORAGE_KEY);
    if (!raw) {
      return;
    }

    try {
      const stored = JSON.parse(raw) as StoredAuth;
      setAuth(stored);
    } catch {
      localStorage.removeItem(AUTH_STORAGE_KEY);
    }
  }, []);

  useEffect(() => {
    if (!auth) {
      localStorage.removeItem("cms.accessToken");
      localStorage.removeItem("cms.tenantId");
      return;
    }

    localStorage.setItem("cms.accessToken", auth.token);
    localStorage.setItem("cms.tenantId", auth.tenantId);
  }, [auth]);

  const login = useCallback(async (payload: LoginRequest) => {
    localStorage.setItem("cms.tenantId", payload.tenantId);
    const response = await authService.login(payload, payload.tenantId);

    const next = toStoredAuth(response);
    setAuth(next);
    localStorage.setItem(AUTH_STORAGE_KEY, JSON.stringify(next));
  }, []);

  const logout = useCallback(() => {
    setAuth(null);
    localStorage.removeItem(AUTH_STORAGE_KEY);
    localStorage.removeItem("cms.accessToken");
    localStorage.removeItem("cms.tenantId");
  }, []);

  const value = useMemo<AuthContextValue>(
    () => ({
      token: auth?.token ?? null,
      tenantId: auth?.tenantId ?? null,
      email: auth?.email ?? null,
      roles: auth?.roles ?? [],
      isAuthenticated: Boolean(auth?.token),
      login,
      logout,
    }),
    [auth, login, logout],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within AuthProvider.");
  }

  return context;
}

export type ApiResponse<T> = {
  success: boolean;
  data: T | null;
  errors: string[];
};

export type AuthTokens = {
  accessToken: string;
  expiresAtUtc: string;
};

export type AuthResponse = {
  userId: string;
  email: string;
  tenantId: string;
  roles: string[];
  tokens: AuthTokens;
};

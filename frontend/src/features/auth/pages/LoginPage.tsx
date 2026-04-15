import { LoginForm } from "../components/LoginForm";
import { authService } from "../services";
import type { RegisterUserPayload } from "../types";

export function LoginPage() {
  async function handleRegister(payload: RegisterUserPayload): Promise<void> {
    await authService.registerUser(payload);
  }

  return (
    <div className="auth-page">
      <div className="auth-card">
        <h1>Clinic SaaS</h1>
        <p>Sign in to manage your clinic tenant.</p>
        <LoginForm onRegister={handleRegister} />
      </div>
    </div>
  );
}

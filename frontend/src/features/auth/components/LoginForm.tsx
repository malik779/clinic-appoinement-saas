import { useState } from "react";
import { Button } from "primereact/button";
import { Dropdown } from "primereact/dropdown";
import { InputText } from "primereact/inputtext";
import { Password } from "primereact/password";
import { useAuth } from "../../../app/providers/AuthProvider";

const roleOptions = [
  { label: "Tenant Admin", value: "TenantAdmin" },
  { label: "Staff", value: "Staff" },
];

const modeCopy = {
  login: {
    kicker: "Secure clinic access",
    title: "Welcome back",
    description: "Sign in to your workspace and pick up where your team left off.",
    submitLabel: "Enter workspace",
    switchLabel: "Create account",
    helperText: "Protected by tenant-based access controls for every clinic team.",
  },
  register: {
    kicker: "Create your staff profile",
    title: "Set up your account",
    description: "Create a team member account and continue straight into your tenant.",
    submitLabel: "Create account and continue",
    switchLabel: "Back to sign in",
    helperText: "We will create your account first, then sign you in automatically.",
  },
} as const;

type UserRole = "TenantAdmin" | "Staff";

type LoginFormProps = {
  onRegister: (payload: {
    email: string;
    password: string;
    fullName: string;
    role: "TenantAdmin" | "Staff";
  }) => Promise<void>;
};

export function LoginForm({ onRegister }: LoginFormProps) {
  const { login } = useAuth();
  const [tenantId, setTenantId] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [fullName, setFullName] = useState("");
  const [role, setRole] = useState<UserRole>("Staff");
  const [mode, setMode] = useState<"login" | "register">("login");
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);
  const isRegisterMode = mode === "register";

  async function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setError(null);
    setLoading(true);

    try {
      if (!tenantId.trim() || !email.trim() || !password.trim()) {
        throw new Error("Tenant, email and password are required.");
      }

      if (isRegisterMode && !fullName.trim()) {
        throw new Error("Full name is required to create an account.");
      }

      if (isRegisterMode) {
        await onRegister({
          email,
          password,
          fullName,
          role,
        });
      }

      await login({
        tenantId,
        email,
        password,
      });
    } catch (exception) {
      setError(exception instanceof Error ? exception.message : "Request failed.");
    } finally {
      setLoading(false);
    }
  }

  return (
    <section className="auth-form-shell" aria-label="Authentication form">
      <div className="auth-form-header">
        <div>
          <span className="auth-form-kicker">{modeCopy[mode].kicker}</span>
          <h2>{modeCopy[mode].title}</h2>
          <p>{modeCopy[mode].description}</p>
        </div>
        <Button
          type="button"
          text
          className="auth-mode-button"
          label={modeCopy[mode].switchLabel}
          onClick={() => {
            setMode(isRegisterMode ? "login" : "register");
            setError(null);
          }}
        />
      </div>

      <form className="auth-form-grid" onSubmit={handleSubmit}>
        <div className="auth-field">
          <label htmlFor="tenantId">Tenant Id</label>
          <InputText
            id="tenantId"
            className="auth-input"
            value={tenantId}
            onChange={(event) => setTenantId(event.target.value)}
            autoComplete="organization"
            placeholder="Enter your clinic workspace ID"
          />
          <small>Use the tenant ID shared during onboarding.</small>
        </div>

        <div className="auth-field">
          <label htmlFor="email">Email</label>
          <InputText
            id="email"
            type="email"
            className="auth-input"
            value={email}
            onChange={(event) => setEmail(event.target.value)}
            autoComplete="email"
            placeholder="name@clinic.com"
          />
        </div>

        <div className="auth-field">
          <label htmlFor="password">Password</label>
          <Password
            inputId="password"
            className="auth-password"
            value={password}
            onChange={(event) => setPassword(event.target.value)}
            feedback={false}
            toggleMask
            inputClassName="auth-input"
            autoComplete={isRegisterMode ? "new-password" : "current-password"}
            placeholder="Enter your password"
          />
        </div>

        {isRegisterMode ? (
          <div className="auth-register-grid">
            <div className="auth-field">
              <label htmlFor="fullName">Full Name</label>
              <InputText
                id="fullName"
                className="auth-input"
                value={fullName}
                onChange={(event) => setFullName(event.target.value)}
                autoComplete="name"
                placeholder="Dr. Sarah Ahmed"
              />
            </div>
            <div className="auth-field">
              <label htmlFor="role">Role</label>
              <Dropdown
                id="role"
                className="auth-dropdown"
                value={role}
                options={roleOptions}
                onChange={(event) => setRole(event.value)}
                placeholder="Select a role"
              />
            </div>
          </div>
        ) : null}

        {error ? (
          <div className="auth-error-banner" role="alert">
            <i className="pi pi-exclamation-circle" aria-hidden="true" />
            <span>{error}</span>
          </div>
        ) : null}

        <div className="auth-actions">
          <Button
            type="submit"
            className="auth-submit-button"
            loading={loading}
            label={modeCopy[mode].submitLabel}
          />
          <p className="auth-helper-text">{modeCopy[mode].helperText}</p>
        </div>
      </form>
    </section>
  );
}
